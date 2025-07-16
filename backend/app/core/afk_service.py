import time
from shared.utils.non_blocking_timer import NonBlockingTimer  # 実装済みのクラスを利用
import logging
import threading

try:
    import pynput.mouse
    import pynput.keyboard
except ImportError:
    pynput = None  # 後でテスト時用にmockもできるように


class AfkService:
    def __init__(self, afk_threshold: int = 300):
        """
        AFK（Away From Keyboard）サービスの初期化

        Args:
            afk_threshold (int): AFKとみなされる時間（秒単位、デフォルトは300秒）
        """
        if afk_threshold <= 0:
            raise ValueError("afk_threshold must be greater than 0")
        self.afk_threshold = afk_threshold
        self.last_activity_time = time.time()
        self.is_afk = False
        self._lock = threading.Lock()

        # タイマーで1秒おきにAFKチェック
        self.timer = NonBlockingTimer(1.0, self._check_afk)

        # マウス・キーボード操作を監視するためのリスナー
        if pynput:
            self.mouse_listener = pynput.mouse.Listener(on_move=self._on_activity, on_click=self._on_activity, on_scroll=self._on_activity)
            self.keyboard_listener = pynput.keyboard.Listener(on_press=self._on_activity)
        else:
            self.mouse_listener = None
            self.keyboard_listener = None

    def start(self):
        """AFK監視を開始する"""
        self._reset_timer()
        self.timer.start()
        if self.mouse_listener:
            self.mouse_listener.start()
        if self.keyboard_listener:
            self.keyboard_listener.start()
        logging.info("AfkService started.")

    def stop(self):
        """AFK監視を停止する"""
        self.timer.stop()
        if self.mouse_listener:
            self.mouse_listener.stop()
        if self.keyboard_listener:
            self.keyboard_listener.stop()
        logging.info("AfkService stopped.")

    def _on_activity(self, *args, **kwargs):
        """操作があったときに呼ばれる"""
        with self._lock:
            self.last_activity_time = time.time()
            if self.is_afk:
                self.is_afk = False
                logging.info("User is back.")

    def _check_afk(self):
        """一定時間操作がなければAFKと判定する"""
        with self._lock:
            elapsed = time.time() - self.last_activity_time
            if not self.is_afk and elapsed >= self.afk_threshold:
                self.is_afk = True
                logging.info(f"User is now AFK (inactive for {int(elapsed)} seconds).")

    def _reset_timer(self):
        self.last_activity_time = time.time()
        self.is_afk = False
