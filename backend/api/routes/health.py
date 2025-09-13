from fastapi import APIRouter, Response
from fastapi.responses import JSONResponse
from datetime import datetime, timezone

router = APIRouter(tags=["health"])


def now_iso() -> str:
    """現在時刻をUTC ISO8601文字列で返す"""
    return datetime.now(timezone.utc).isoformat()


@router.get("/livez")
async def livez():
    # プロセスが生きているか（常に200）
    payload = {
        "status": "ok",
        "component": "api",
        "time": now_iso()
    }
    return JSONResponse(content=payload, headers={"Cache-Control": "no-store"})


@router.get("/readyz")
async def readyz():
    # TODO: app.state.* に依存チェックを追加する（DB, AFKサービスなど）
    checks = {
        "api": True,
    }
    overall = all(checks.values())
    status_code = 200 if overall else 503
    payload = {
        "status": "ok" if overall else "unavailable",
        "time": now_iso(),
        "checks": checks,
    }
    return JSONResponse(content=payload, status_code=status_code, headers={"Cache-Control": "no-store"})


@router.get("/healthz")
async def healthz():
    # live と ready をまとめた全体的なヘルス
    payload = {
        "live": True,
        "ready": True,  # 実際は readyz と同じ判定を使うのがベスト
        "time": now_iso(),
        "version": "1.0.0",
    }
    return JSONResponse(content=payload, headers={"Cache-Control": "no-store"})