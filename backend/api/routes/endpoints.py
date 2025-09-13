import signal
import asyncio
from datetime import datetime
from logging import Logger, Formatter, handlers

from fastapi import APIRouter, Request
from fastapi.responses import JSONResponse


logger = Logger(__name__)
f = Formatter('%(asctime)s - [%(levelname)s] : %(message)s')
dt_str = datetime.now().strftime("%Y%m%d_%H%M%S")
handler = handlers.RotatingFileHandler(
    rf'../request_{dt_str}.log',
    mode="a",
    encoding="utf-8"
)
handler.setFormatter(f)
logger.addHandler(handler)

router = APIRouter(prefix="/api/status", tags=["endpoints"])


@router.get("/current")
async def current_status(request: Request):
    status = request.app.state.monitor.snapshot()
    logger.info(f"Get request({status["state"]})")
    return JSONResponse(content=status["state"])


@router.get("/set")
async def set_status(request: Request):
    request.app.state.monitor.toggle_break()
    logger.info("Get toggle status")
    return JSONResponse(content="OK")


@router.get("/record")
async def record_status(request: Request):
    status = request.app.state.monitor.snapshot()
    logger.info(f"Get record({status["state"]})")
    return JSONResponse(content=status)


@router.get("/reset")
async def reset_status(request: Request):
    request.app.state.monitor.reset_timekeeper()
    logger.info("Get reset")
    return JSONResponse(content="OK")


@router.get("/kill")
async def kill_request():

    async def _stop():
        await asyncio.sleep(0.5)
        signal.raise_signal(signal.SIGINT)

    logger.info("Get kill")
    asyncio.create_task(_stop())
    return JSONResponse(content="OK")
