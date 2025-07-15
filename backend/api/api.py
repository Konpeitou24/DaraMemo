from typing import Callable
from flask import Flask
import logging
from werkzeug.serving import make_server
import threading

class Api:
    """APIサーバーのエントリポイント"""

    routes: list[tuple[str, Callable]] = [
        
        # ここにルートを追加
        # ("/", home_handler),
    ]
    """APIサーバーの、どのコマンドをどの関数を対応させるかの情報"""

    
    app: Flask
    """Flaskアプリケーションのインスタンス"""

    def __init__(self):
        """Flaskアプリケーションの初期化とルーティングの設定"""
        self.app = Flask(__name__)
        self._register_routes()
        self._server = None
        self._thread = None
        self._shutdown_flag = threading.Event()

    def _register_routes(self):
        """Flaskアプリケーションにルートを登録する"""
        for path, handler in self.routes:
            self.app.add_url_rule(path, view_func=handler)

    def run(self):
        """APIサーバーを非ブロッキングで起動する"""
        logging.info("Starting API server...")
        self._server = make_server("127.0.0.1", 5000, self.app)
        self._server.timeout = 1  # 応答性向上のため短めに
        self._thread = threading.Thread(target=self._serve_loop)
        self._thread.start()

    def _serve_loop(self):
        """APIサーバーのリクエストを処理するループ"""
        while not self._shutdown_flag.is_set():
            self._server.handle_request()
        logging.info("API server stopped.")

    def shutdown(self):
        """APIサーバーを安全に停止する"""
        logging.info("Shutting down API server...")
        self._shutdown_flag.set()
        if self._thread:
            self._thread.join()
