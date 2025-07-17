from shared.utils.notifier import Notifier


class NotifierTest:
    def __init__(self):
        self.notifier = Notifier("Hello")  # Initialize notifier instance

    def run_tests(self):
        print("Running notifier tests...")
        self.notifier.run_toast("Test notification")  # Example test