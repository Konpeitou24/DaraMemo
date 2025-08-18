from typing import Callable
import logging
import threading
import asyncio

from fastapi import FastAPI
import uvicorn


class Api:
    """APIサーバーのエントリポイント（FastAPI版）"""

    routes: list[tuple[str, Callable]] = [
        # 例: ("/health", lambda: {"ok": True}),
        # 例: ("/set-break", set_break_handler),
    ]
    """APIサーバーの、どのパスにどのハンドラを対応させるかの情報（GET想定）"""

    app: FastAPI
    """FastAPIアプリケーションのインスタンス"""

    def __init__(self):
        """FastAPIアプリケーションの初期化とルーティングの設定"""
        self.app = FastAPI(title="DaraMemo API", version="1.0.0")
        self._register_routes()
        self._server: uvicorn.Server | None = None
        self._thread: threading.Thread | None = None

    def _register_routes(self):
        """FastAPIアプリケーションにルートを登録する（同期/非同期どちらのハンドラにも対応）"""
        for path, handler in self.routes:
            if asyncio.iscoroutinefunction(handler):
                # async def handler(...)
                self.app.add_api_route(path, handler, methods=["GET"])
            else:
                # def handler(...): -> wrap して非同期化
                async def _wrap(h=handler):
                    return h()
                self.app.add_api_route(path, _wrap, methods=["GET"])

    def run(self, host: str = "127.0.0.1", port: int = 5000):
        """APIサーバーを非ブロッキングで起動する（別スレッド）"""
        logging.info("Starting API server...")
        config = uvicorn.Config(
            app=self.app,
            host=host,
            port=port,
            log_level="info",
        )
        self._server = uvicorn.Server(config)

        def _serve():
            asyncio.run(self._server.serve())

        self._thread = threading.Thread(target=_serve, daemon=True)
        self._thread.start()

    def shutdown(self):
        """APIサーバーを安全に停止する"""
        logging.info("Shutting down API server...")
        if self._server is not None:
            self._server.should_exit = True
        if self._thread is not None:
            self._thread.join()
        logging.info("API server stopped.")