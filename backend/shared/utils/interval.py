from enum import Enum
import logging

class IntervalUnit(Enum):
    SECONDS = 'seconds'
    MINUTES = 'minutes'
    MILLISECONDS = 'milliseconds'
    HOURS = 'hours'

class Interval:
    """アプリケーションのインターバルを定義するクラス（秒単位で返す）"""

    @staticmethod
    def from_minutes(minutes: float) -> float:
        Interval._validate_input(minutes, IntervalUnit.MINUTES)
        return minutes * 60.0

    @staticmethod
    def from_seconds(seconds: float) -> float:
        Interval._validate_input(seconds, IntervalUnit.SECONDS)
        return seconds

    @staticmethod
    def from_milliseconds(ms: float) -> float:
        Interval._validate_input(ms, IntervalUnit.MILLISECONDS)
        return ms / 1000.0

    @staticmethod
    def from_hours(hours: float) -> float:
        Interval._validate_input(hours, IntervalUnit.HOURS)
        return hours * 3600.0

    @staticmethod
    def _validate_input(value: float, unit: IntervalUnit) -> None:
        if value < 0:
            logging.warning(f"Negative interval value detected for {unit.value}: {value}")
            raise ValueError(f"{unit.value.capitalize()} must be non-negative")

    # よく使う定数（秒単位）
    ONE_SECOND = from_seconds.__func__(1)
    HALF_SECOND = from_seconds.__func__(0.5)
    ONE_MINUTE = from_minutes.__func__(1)
    FIVE_MINUTES = from_minutes.__func__(5)
    TEN_SECONDS = from_seconds.__func__(10)
    ONE_HOUR = from_hours.__func__(1)
    THIRTY_SECONDS = from_seconds.__func__(30)
    TWO_MINUTES = from_minutes.__func__(2)
