import logging
import time

class Application:
# region Appication Class Properties
    """アプリケーションの実行管理を担当するクラス"""

    should_run: bool
    """アプリケーションの実行フラグ: Trueならアプリケーションは実行中"""

    tick_interval: float
    """アプリケーションの更新間隔（秒単位）"""

# endregion

# region Appication Class Initialization
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
# endregion

# region Appication Class Methods
    def run(self):
        """アプリケーションのメイン処理を実行する"""
        logging.info("Application is starting...")
        self.should_run = True
        self._main_loop()
    
    def shutdown(self):
        """アプリケーションの実行を停止する"""
        logging.info("Application is stopping...")
        self.should_run = False

    def _main_loop(self):
        """アプリケーションのメインループ"""
        while self.should_run:
            logging.info("Application is running...")
            time.sleep(self.tick_interval)
# endregion
