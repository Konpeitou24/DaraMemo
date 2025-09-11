import time
import ctypes
import threading
from enum import Enum
from functools import lru_cache
from logging import Logger, Formatter, handlers
from datetime import datetime, timezone
from zoneinfo import ZoneInfo


logger = Logger(__name__)
f = Formatter('%(asctime)s - [%(levelname)s] : %(message)s')
handler = handlers.RotatingFileHandler(
    rf'./state_{datetime.now(ZoneInfo("Asia/Tokyo")).isoformat()}.log',
    mode="a",
    encoding="utf-8"
)
handler.setFormatter(f)
logger.addHandler(handler)


class IdleState(Enum):
    ACTIVE = "ACTIVE"
    AFK = "AFK"
    BREAK = "BREAK"


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
    def __init__(self, threshold_idle_sec: int = 300, poll_interval: float = 1.0):
        self.threshold = float(threshold_idle_sec)
        self.interval = float(poll_interval)
        self._stop = threading.Event()
        self._lock = threading.Lock()
        self._thread: threading.Thread | None = None

        self._idle_seconds: float = 0.0
        self._state: IdleState = IdleState.ACTIVE
        self._last_change: datetime = datetime.now(timezone.utc)
        self._is_break: bool = False

    def snapshot(self) -> dict:
        with self._lock:
            return {
                "state": self._state,
                "idle_seconds": round(self._idle_seconds, 3),
                "last_change": self._last_change.isoformat(),
                "threshold_idle": self.threshold,
            }

    def start(self):
        if self._thread and self._thread.is_alive():
            return
        self._stop.clear()
        self._thread = threading.Thread(target=self._run, name="IdleMonitor", daemon=True)
        self._thread.start()

    def stop(self, timeout: float = 5.0):
        self._stop.set()
        if self._thread:
            self._thread.join(timeout=timeout)

    def _run(self):
        logger.info(f"[{self._state.value}] Start monitoring idle time")
        while not self._stop.is_set():
            try:
                idle = get_idle_seconds_windows()
            except Exception:
                time.sleep(2.0)
                continue

            now_state = IdleState.ACTIVE if idle < self.threshold else IdleState.AFK
            if self._is_break:
                now_state = IdleState.BREAK
            with self._lock:
                self._idle_seconds = idle
                if now_state != self._state:
                    self._state = now_state
                    self._last_change = datetime.now(timezone.utc)
                    logger.info(f"[{self._state.value}] Change status")

            time.sleep(self.interval)

    def toggle_break(self):
        with self._lock:
            self._is_break = not self._is_break


@lru_cache(maxsize=1)
def get_monitor(*, threshold_idle_sec: int = 300, poll_interval: float = 1.0) -> IdleMonitor:
    return IdleMonitor(threshold_idle_sec=threshold_idle_sec, poll_interval=poll_interval)
