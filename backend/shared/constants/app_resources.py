from enum import Enum

class AppResources(Enum, str):
    """アプリケーションのリソースを定義するEnumクラス"""
    # Define your application resources here
    TITLE = "DaraMemo"
    """アプリケーションのタイトルを保持するためのプロパティ"""
    AFK_THRESHOLD = 300
    """AFKとみなされる時間（秒単位）を保持するためのプロパティ"""