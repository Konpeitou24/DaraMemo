import time
import ctypes
import threading
from enum import Enum
from logging import Logger, Formatter, handlers
from datetime import datetime, timezone, timedelta


logger = Logger(__name__)
f = Formatter('%(asctime)s - [%(levelname)s] : %(message)s')
dt_str = datetime.now().strftime("%Y%m%d_%H%M%S")
handler = handlers.RotatingFileHandler(
    rf'../state_{dt_str}.log',
    mode="a",
    encoding="utf-8"
)
handler.setFormatter(f)
logger.addHandler(handler)


class IdleState(Enum):
    ACTIVE = "ACTIVE"
    AFK = "AFK"
    BREAK = "BREAK"


class TimeKeeper:
    def __init__(self, state: IdleState) -> None:
        self.active_time = timedelta(0)
        self.afk_time = timedelta(0)
        self.break_time = timedelta(0)

        self.std_active: datetime | None = None
        self.std_afk: datetime | None = None
        self.std_break: datetime | None = None

        self.refresh(state)

    @property
    def str_active(self) -> str:
        return f"{self.hh(self.active_time)}:{self.mm(self.active_time)}:{self.ss(self.active_time)}"

    @property
    def str_afk(self) -> str:
        return f"{self.hh(self.afk_time)}:{self.mm(self.afk_time)}:{self.ss(self.afk_time)}"

    @property
    def str_break(self) -> str:
        return f"{self.hh(self.break_time)}:{self.mm(self.break_time)}:{self.ss(self.break_time)}"

    @staticmethod
    def hh(target: timedelta) -> str:
        return f"{target.seconds // 3600:02}"

    @staticmethod
    def mm(target: timedelta) -> str:
        return f"{target.seconds // 60 % 60:02}"

    @staticmethod
    def ss(target: timedelta) -> str:
        return f"{target.seconds % 60:02}"

    def refresh(self, state: IdleState) -> None:
        now = datetime.now()
        if state == IdleState.ACTIVE:
            if self.std_active is None:
                self.std_active = now
                self.std_afk = self.std_break = None
            else:
                self.active_time += now - self.std_active
                self.std_active = now
        elif state == IdleState.AFK:
            if self.std_afk is None:
                self.std_afk = now
                self.std_active = self.std_break = None
            else:
                self.afk_time += now - self.std_afk
                self.std_afk = now
        elif state == IdleState.BREAK:
            if self.std_break is None:
                self.std_break = now
                self.std_active = self.std_afk = None
            else:
                self.break_time += now - self.std_break
                self.std_break = now

    def reset(self) -> None:
        self.active_time = timedelta(0)
        self.afk_time = timedelta(0)
        self.break_time = timedelta(0)

        self.std_active = None
        self.std_afk = None
        self.std_break = None


class LASTINPUTINFO(ctypes.Structure):
    _fields_ = [("cbSize", ctypes.c_uint), ("dwTime", ctypes.c_uint)]

def get_idle_seconds_windows() -> float:
    user32 = ctypes.windll.user32
    kernel32 = ctypes.windll.kernel32

    lii = LASTINPUTINFO()
    lii.cbSize = ctypes.sizeof(LASTINPUTINFO)
    if not user32.GetLastInputInfo(ctypes.byref(lii)):
        raise OSError("GetLastInputInfo failed")

    tick64 = ctypes.c_ulonglong(kernel32.GetTickCount64()).value
    now32 = tick64 & 0xFFFFFFFF
    last32 = lii.dwTime & 0xFFFFFFFF
    if now32 >= last32:
        idle_ms = now32 - last32
    else:
        idle_ms = (now32 + 2**32) - last32
    return idle_ms / 1000.0

class IdleMonitor:
    def __init__(self, threshold_idle_sec: int = 3, poll_interval: float = 1.0):
        self.threshold = float(threshold_idle_sec)
        self.interval = float(poll_interval)
        self._stop = threading.Event()
        self._lock = threading.Lock()
        self._thread: threading.Thread | None = None

        self._idle_seconds: float = 0.0
        self._state: IdleState = IdleState.ACTIVE
        self._last_change: datetime = datetime.now(timezone.utc)
        self._timekeeper = TimeKeeper(self._state)

    def snapshot(self) -> dict:
        with self._lock:
            return {
                "state": self._state.value,
                "active_time": self._timekeeper.str_active,
                "afk_time": self._timekeeper.str_afk,
                "break_time": self._timekeeper.str_break,
            }

    def start(self):
        if self._thread and self._thread.is_alive():
            return
        self._stop.clear()
        self._thread = threading.Thread(target=self._run, name="IdleMonitor", daemon=True)
        self._thread.start()

        logger.info(f"[{self._state.value}] Start monitoring")

    def stop(self, timeout: float = 5.0):
        with self._lock, open(f"../trace_{dt_str}.log", "a", encoding="utf-8") as log:
            print(f"AVTIVE,AFK,BREAK\n{self._timekeeper.str_active},{self._timekeeper.str_afk},{self._timekeeper.str_break}", file=log)

        self._stop.set()
        if self._thread:
            self._thread.join(timeout=timeout)

        logger.info(f"[{self._state.value}] Stop monitoring")

    def _run(self):
        while not self._stop.is_set():
            if self._state != IdleState.BREAK:
                try:
                    idle = get_idle_seconds_windows()
                except Exception:
                    time.sleep(2.0)
                    continue

                with self._lock:
                    self._idle_seconds = idle

                    now_state = IdleState.ACTIVE if idle < self.threshold else IdleState.AFK

                    if now_state != self._state:
                        self._state = now_state
                        self._last_change = datetime.now(timezone.utc)
                        logger.info(f"[{self._state.value}] Change status")

            with self._lock:
                self._timekeeper.refresh(self._state)

            logger.info(f"[{self._state.value}] Current status")

            time.sleep(self.interval)

    def toggle_break(self):
        with self._lock:
            if self._state == IdleState.BREAK:
                self._state = IdleState.ACTIVE
            else:
                self._state = IdleState.BREAK
            self._last_change = datetime.now(timezone.utc)
            logger.info(f"[{self._state.value}] Change status")

    def reset_timekeeper(self) -> None:
        with self._lock:
            self._timekeeper.reset()
