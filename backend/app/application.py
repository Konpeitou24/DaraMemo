import logging
import time

from shared.utils.non_blocking_timer import NonBlockingTimer

class Application:
# region Application Class Properties
    """アプリケーションの実行管理を担当するクラス"""

    should_run: bool
    """アプリケーションの実行フラグ: Trueならアプリケーションは実行中"""

    tick_interval: float
    """アプリケーションの更新間隔（秒単位）"""

    timer: NonBlockingTimer

# endregion

# region Application Class Initialization
    def __init__(self, fps: float = 60):
        """
        アプリケーションの初期化

        Args:
            fps (float): 1秒あたりのフレーム（更新）回数（デフォルトは60）
        """
        if fps <= 0:
            raise ValueError("fps must be greater than 0")
        self.tick_interval = 1 / fps
        self.should_run = False

        self.timer = NonBlockingTimer(self.tick_interval, self._on_tick)
# endregion

# region Application Class Methods
    def run(self):
        """アプリケーションのメイン処理を実行する"""
        logging.info("Application is starting...")
        self.should_run = True
        self._main_loop()
    
    def shutdown(self):
        """アプリケーションの実行を停止する"""
        logging.info("Application is stopping...")
        self.timer.stop()
        self.should_run = False

    def _main_loop(self):
        """アプリケーションのメインループ"""
        while self.should_run:
            logging.info("Application is running...")
            time.sleep(self.tick_interval)
    def _on_tick(self):
        """タイマーのコールバック関数"""
        if self.should_run:
            logging.info("Timer tick occurred.")
# endregion
