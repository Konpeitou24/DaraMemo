from plyer import notification
from backend.shared.constants.app_resources import AppResources
class Notifier:
    """様々な通知を行うためのクラス"""
    title: str
    """通知時のタイトルを保持するためのプロパティ"""
    def __init__(self, title:str):
        self.title = title
    def run_toast(self, message:str, timeout:int=10):
        """トースト通知を表示するメソッド"""
        notification.notify(
            title=self.title,
            message=message,
            app_name=AppResources.Title.value,
            timeout=timeout
        )