import logging
import threading
import time
from typing import Callable, overload, List, Union

class NonBlockingTimer:
    """非ブロッキングタイマーを実装するクラス"""

    interval: float
    """タイマーの間隔（秒単位）"""

    callbacks: List[Callable]
    """タイマーが終了したときに呼び出されるコールバック関数のリスト"""

    should_tick: bool
    """タイマーが実行中かどうかのフラグ"""
    
    @overload
    def __init__(self, interval: float, callback: Callable):
        """
        コンストラクタ
        
        :param interval: タイマーの間隔（秒単位）
        :param callback: タイマーが終了したときに呼び出されるコールバック関数
        """

    
    @overload
    def __init__(self, interval: float, callbacks: List[Callable]):
        """
        コンストラクタ
        
        :param interval: タイマーの間隔（秒単位）
        :param callbacks: タイマーが終了したときに呼び出されるコールバック関数のリスト
        """

    def __init__(self, interval: float, callback: Union[Callable, List[Callable]]):


        """
        コンストラクタ
        
        :param interval: タイマーの間隔（秒単位）
        :param callback: タイマーが終了したときに呼び出されるコールバック関数またはそのリスト
        """
        if interval <= 0:
            raise ValueError("Interval must be greater than 0")
        
        self.interval = interval
        self.callbacks = callback if isinstance(callback, list) else [callback]
        self.thread = None
        self.should_tick = False
    
    def start(self) -> None:
        """タイマーを開始する"""
        if self.should_tick and self.thread and self.thread.is_alive():
            return  # すでに動いているなら何もしない
        self.should_tick = True
        self.thread = threading.Thread(target=self._main_loop, daemon=True)
        self.thread.start()

    
    def stop(self) -> None:
        """
        タイマーを停止する
        """
        self.should_tick = False

        # スレッドが存在し、まだ動いている場合のみ join() を呼ぶ
        if self.thread is not None and self.thread.is_alive():
            self.thread.join(timeout=self.interval + 0.1)  # 少し余裕を持たせる (0.1秒)
            if self.thread.is_alive():
                logging.warning("Timer thread did not stop within expected time.")
        return None


    def _main_loop(self) -> None:
        """
        タイマーのメインループ
        """
        while self.should_tick:
            time.sleep(self.interval)
            self._run_callbacks()
        return None

    def _run_callbacks(self) -> None:
        def safe_callback(cb):
            try:
                cb()
            except Exception as e:
                logging.error(f"Callback error: {e}")

        for callback in self.callbacks:
            thread = threading.Thread(target=lambda: safe_callback(callback), daemon=True)
            thread.start()
