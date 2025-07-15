import threading
from backend.app.application import Application
from backend.api.api import Api
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
        """アプリケーションとAPIサーバーを並列に起動し、監視する"""
        logging.info("Runtime: starting Application and API")

        # APIサーバーを先に起動（別スレッド）
        self.api.run()

        # アプリケーション側を別スレッドで起動（例: blockingなメインループ）
        self._app_thread = threading.Thread(target=self.application.run)
        self._app_thread.start()

        # アプリケーション終了を待機
        self._app_thread.join()

        # アプリケーションが止まったらAPIも止める
        logging.info("Runtime: Application stopped, shutting down API")
        self.api.shutdown()

        logging.info("Runtime: shutdown complete")

