from pynput import keyboard
import threading
import time

class KeyboardActivityDetector:
    def __init__(self, interval: float = 1.0):
        """
        キーボード入力のアクティビティを検知するクラス

        Args:
            interval (float): アクティビティ検出のインターバル（秒）。
                              is_pressed() を呼び出した時点から interval 秒の間に
                              1度でもキー入力があれば True を返す。
        """
        self._interval = interval
        self._pressed = False
        self._lock = threading.Lock()
        self._listener = keyboard.Listener(on_press=self._on_press)
        self._listener.start()

    def _on_press(self, key):
        with self._lock:
            self._pressed = True

    def is_pressed(self) -> bool:
        """
        interval 秒の間にキーが1度でも押された場合に True を返す。
        チェックのたびにフラグはリセットされる。

        Returns:
            bool: キーが押されたかどうか
        """
        with self._lock:
            self._pressed = False
        time.sleep(self._interval)
        with self._lock:
            return self._pressed
