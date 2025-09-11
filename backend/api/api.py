from typing import Callable
import logging
import threading
import asyncio
from contextlib import asynccontextmanager

from fastapi import FastAPI
import uvicorn

from api.routes.health import router as health_router
from api.routes.endpoints import router as endpoints_router
from api.Observer import get_monitor


@asynccontextmanager
async def lifespan(app: FastAPI):
    mon = get_monitor(threshold_idle_sec=300, poll_interval=1.0)
    mon.start()
    try:
        yield
    finally:
        mon.stop()


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
        self.app = FastAPI(title="DaraMemo API", version="1.0.0", lifespan=lifespan)
        self._register_routes()
        self._server: uvicorn.Server | None = None
        self._thread: threading.Thread | None = None

    def _register_routes(self):
        """FastAPIアプリケーションにルートを登録する（同期/非同期どちらのハンドラにも対応）"""
        self.app.include_router(health_router) # ヘルスチェック用のルータを追加
        self.app.include_router(endpoints_router)  # エンドポイント用ルータを追加
        logging.info("Registered routes")


# 起動と停止

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