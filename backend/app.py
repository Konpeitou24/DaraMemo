import threading
import app.entry_point as app_main
import api.entry_point as api_main

if __name__ == "__main__":
    # 処理をブロックせずにローカルのAPIサーバーを起動
    threading.Thread(target=api_main.start_api_server, daemon=True).start()
    # アプリケーションのメイン処理を実行
    app_main.run()
