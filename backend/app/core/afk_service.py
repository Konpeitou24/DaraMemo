
from app.core.keyboard_activity_detector import KeyboardActivityDetector
from app.core.mouse_activity_detector import MouseActivityDetector

class AfkService:
    interval: float
    """AFK状態を監視するサービスの間隔（秒単位）"""

    keyboard_detector: KeyboardActivityDetector
    """キーボードアクティビティを検出するためのインスタンス"""

    mouse_detector: MouseActivityDetector
    """マウスアクティビティを検出するためのインスタンス"""

    def __init__(self, interval: float):
        """
        AFK状態を監視するサービスの初期化

        Args:
            interval (float): AFK状態をチェックする間隔（秒単位）
        """
        if interval <= 0:
            raise ValueError("Interval must be greater than 0")
        
        self.interval = interval
        self.keyboard_detector = KeyboardActivityDetector(interval)
        self.mouse_detector = MouseActivityDetector(interval)
