from logging import Logger, Formatter, handlers
from datetime import datetime

from fastapi import APIRouter, Request
from fastapi.responses import JSONResponse


logger = Logger(__name__)
f = Formatter('%(asctime)s - [%(levelname)s] : %(message)s')
dt_str = datetime.now().strftime("%Y%m%d_%H%M%S")
handler = handlers.RotatingFileHandler(
    rf'./request_{dt_str}.log',
    mode="a",
    encoding="utf-8"
)
handler.setFormatter(f)
logger.addHandler(handler)

router = APIRouter(prefix="/api/status", tags=["endpoints"])


@router.get("/current")
async def current_status(request: Request):
    mon = request.app.state.monitor
    status = mon.snapshot()
    logger.info(f"Get request({status})")
    return JSONResponse(content=status)


@router.get("/set")
async def set_status(request: Request):
    mon = request.app.state.monitor
    mon.toggle_break()
    logger.info("Toggle status")
    return JSONResponse(content="OK")