from pynput import mouse
import threading
import time

class MouseActivityDetector:
    def __init__(self, interval: float = 1.0):
        """
        マウスアクティビティを検出するクラス

        Args:
            interval (float): チェックインターバル（秒）。各チェックの間で一度でも
                              マウスが移動またはクリックされた場合、対応するメソッドが True を返す。
        """
        self._interval = interval
        self._moved = False
        self._clicked = False
        self._lock = threading.Lock()
        self._listener = mouse.Listener(
            on_move=self._on_move,
            on_click=self._on_click
        )
        self._listener.start()

    def _on_move(self, x, y):
        with self._lock:
            self._moved = True

    def _on_click(self, x, y, button, pressed):
        if pressed:
            with self._lock:
                self._clicked = True

    def is_moved(self) -> bool:
        """
        interval 秒の間にマウスが動いたかどうかを返す

        Returns:
            bool: 動いていれば True、そうでなければ False
        """
        return self._check_flag('_moved')

    def is_clicked(self) -> bool:
        """
        interval 秒の間にマウスがクリックされたかどうかを返す

        Returns:
            bool: クリックされていれば True、そうでなければ False
        """
        return self._check_flag('_clicked')

    def _check_flag(self, flag_name: str) -> bool:
        with self._lock:
            setattr(self, flag_name, False)
        time.sleep(self._interval)
        with self._lock:
            return getattr(self, flag_name)
