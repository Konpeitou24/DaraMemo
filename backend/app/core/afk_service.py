
class AfkService:
    interval: float
    """AFK状態を監視するサービスの間隔（秒単位）"""

    def __init__(self, interval: float):
        """
        AFK状態を監視するサービスの初期化

        Args:
            interval (float): AFK状態をチェックする間隔（秒単位）
        """
        if interval <= 0:
            raise ValueError("Interval must be greater than 0")
        
        self.interval = interval

