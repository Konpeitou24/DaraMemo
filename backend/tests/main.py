from tests.notifier_test import NotifierTest


class Main:
    def __init__(self):
        pass

def main():
    # テストクラスのインスタンス化と、エントリポイント実行は残すこと。
    notifier_test = NotifierTest()
    notifier_test.run_tests()
    
if __name__ == "__main__":
    main()