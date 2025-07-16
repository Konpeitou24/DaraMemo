from pynput import mouse
import threading

class MouseActivityDetector:
    def __init__(self):
        self._moving = False
        self._lock = threading.Lock()
        self._listener = mouse.Listener(on_move=self._on_move)
        self._listener.start()

    def _on_move(self, x, y):
        with self._lock:
            self._moving = True

    def is_mouse_moving(self) -> bool:
        with self._lock:
            return self._moving

    def reset(self):
        with self._lock:
            self._moving = False
