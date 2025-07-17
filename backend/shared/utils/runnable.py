import inspect
import threading

def runnable(func):
    """
    デコレータ：このメソッドを「一括実行可能（runnable）」としてマークする。

    run_all / run_all_async によって対象として認識され、実行される。
    """
    func._is_runnable = True
    return func


class Runnable:
    """
    @runnable が付いたメソッドを一括で実行するための Mixin。

    同期・非同期両対応で、開発ツールや処理バッチの整理に活用できる。
    """

    def run_all(self):
        """
        @runnable が付いた全メソッドを「同期で」順次実行する。

        inspect.getmembers によって実行対象を厳密に絞っている：
        - 対象はインスタンスメソッドのみ
        - デコレータによって _is_runnable が付いている関数のみ実行
        """
        for name, method in inspect.getmembers(self, predicate=inspect.ismethod):
            if getattr(method, "_is_runnable", False):
                method()

    async def run_all_async(self):
        """
        @runnable が付いた全メソッドを「非同期で」順次実行する。

        inspect.getmembers によって実行対象を厳密に絞っている。
        - 非同期関数は await を使って実行
        - 通常関数は同期的に呼び出す
        """
        for name, method in inspect.getmembers(self, predicate=inspect.ismethod):
            if getattr(method, "_is_runnable", False):
                if inspect.iscoroutinefunction(method):
                    await method()
                else:
                    method()
    def run_all_threaded(self):
        """
        @runnable が付いた全メソッドを、各スレッドで並列実行する。

        - 同期関数のみ対象（非同期関数には非対応）
        - CPUバウンドでもIOバウンドでも並列に処理される
        - スレッドの開始順に関係なく実行される（順序保証なし）
        """
        threads = []

        for name, method in inspect.getmembers(self, predicate=inspect.ismethod):
            if getattr(method, "_is_runnable", False):
                if inspect.iscoroutinefunction(method):
                    continue
                def runner(method=method, name=name):
                    method()
                thread = threading.Thread(target=runner)
                threads.append(thread)
                thread.start()

        for t in threads:
            t.join()