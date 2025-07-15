import logging

class Application:
    """アプリケーションのルーティング情報"""
    should_run: bool
    """アプリケーションの実行フラグ: Trueならアプリケーションは実行中"""
    def __init__(self):
        """アプリケーションの初期化"""
        self.should_run = False

    def run(self):
        """アプリケーションのメイン処理を実行する"""
        logging.info("Application is starting...")
        self._main_loop()
    
    def stop(self):
        """アプリケーションの実行を停止する"""
        logging.info("Application is stopping...")
        self.should_run = False

    def _main_loop(self):
        """アプリケーションのメインループ"""
        while self.should_run:
            pass
