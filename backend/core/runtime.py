import threading
from app.application import Application
from api.api import Api
import logging

class Runtime:
    """アプリケーションとAPIサーバーの実行を管理するクラス"""

    application: Application
    """アプリケーションのインスタンス"""

    api: Api
    """APIサーバーのインスタンス"""

    _app_thread: threading.Thread
    """アプリケーションの実行を管理するスレッド"""

    def __init__(self):
        """Runtimeの初期化"""
        self.application = Application()
        self.api = Api()

    def run(self):
        logging.info("Runtime: starting Application and API")

        self.api.run()
        self._app_thread = threading.Thread(target=self.application.run)
        self._app_thread.start()

        try:
            while self._app_thread.is_alive():
                self._app_thread.join(timeout=0.5)  # Ctrl+Cを拾いやすくする
        except KeyboardInterrupt:
            logging.info("Runtime: Ctrl+C detected, stopping application...")
            self.application.shutdown()
        finally:
            logging.info("Runtime: Application stopped, shutting down API")
            self.api.shutdown()
            logging.info("Runtime: shutdown complete")
