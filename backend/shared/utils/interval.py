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


# ✅ クラス定義後に定数を追加（安全）
Interval.ONE_SECOND = Interval.from_seconds(1)
Interval.HALF_SECOND = Interval.from_seconds(0.5)
Interval.ONE_MINUTE = Interval.from_minutes(1)
Interval.FIVE_MINUTES = Interval.from_minutes(5)
Interval.TEN_SECONDS = Interval.from_seconds(10)
Interval.ONE_HOUR = Interval.from_hours(1)
Interval.THIRTY_SECONDS = Interval.from_seconds(30)
Interval.TWO_MINUTES = Interval.from_minutes(2)
