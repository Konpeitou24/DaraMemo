import logging
import time

class Application:
    """アプリケーションの実行管理を担当するクラス"""
    should_run: bool
    """アプリケーションの実行フラグ: Trueならアプリケーションは実行中"""
    def __init__(self, tick_interval: float = 1.0):
        """
        アプリケーションの初期化

        Args:
            tick_interval (float): メインループの1フレームあたりの秒数（デフォルトは1秒）
        """
        self.tick_interval = tick_interval

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
