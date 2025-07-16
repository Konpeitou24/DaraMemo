class AfkService:
    def __init__(self, afk_threshold: int = 300):
        """
        AFK（Away From Keyboard）サービスの初期化

        Args:
            afk_threshold (int): AFKとみなされる時間（秒単位、デフォルトは300秒）
        """
        if afk_threshold <= 0:
            raise ValueError("afk_threshold must be greater than 0")
        self.afk_threshold = afk_threshold
        self.last_activity_time = time.time()
        self.is_afk = False