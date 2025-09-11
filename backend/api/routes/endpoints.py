from logging import Logger, Formatter, handlers
from datetime import datetime
from zoneinfo import ZoneInfo

from fastapi import APIRouter
from fastapi.responses import JSONResponse
from api.Observer import get_monitor


logger = Logger(__name__)
f = Formatter('%(asctime)s - [%(levelname)s] : %(message)s')
handler = handlers.RotatingFileHandler(
    rf'./request_{datetime.now(ZoneInfo("Asia/Tokyo")).isoformat()}.log',
    mode="a",
    encoding="utf-8"
)
handler.setFormatter(f)
logger.addHandler(handler)

router = APIRouter(tags=["endpoints"])


@router.get("/api/status/current")
async def current_status():
    mon = get_monitor()
    status = mon.snapshot()
    logger.info(f"Check current status")
    return JSONResponse(content=status["state"].value)


@router.get("/api/status/set")
async def set_status():
    mon = get_monitor()
    mon.toggle_break()
    logger.info("Toggle status")
    return JSONResponse(content="OK")