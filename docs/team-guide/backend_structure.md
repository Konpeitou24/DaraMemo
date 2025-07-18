# バックエンド構成概要（DaraMemo）

本ドキュメントは、`backend/` ディレクトリ以下の構成と各フォルダの責務、起動構成の概要をまとめたものです。

---

## ディレクトリ構成と役割

```
backend/
├── api/               # REST API（Flaskベース）
│   ├── routes/        # 各APIエンドポイントのルーティング定義
│   └── api.py         # Flaskアプリの初期化処理
│
├── app/               # 常駐アプリケーション（AFK検知）
│   ├── core/          # キーボード/マウスの検出やAFK判定ロジック
│   └── application.py # アプリケーション側エントリポイント
│
├── shared/            # APIとアプリで共通利用するユーティリティ
│   ├── constants/     # 定数や設定値
│   ├── db/            # データアクセス層（PostgreSQL）
│   ├── models/        # DTO / エンティティ定義
│   └── utils/         # 汎用ツール（Timer、通知など）
```

---

## 実行構成

### メインエントリポイント
`main.py` がアプリケーションの全体起動用スクリプトです。

- `app.application.Application` を起動（AFK監視）
- 並列で `api.api.create_app()` を使って Flask サーバーをバックグラウンド起動
- 終了時には両者を安全に停止させる処理を含んでいます

---

## 起動の仕組み（例）

```
# main.py（抜粋）
from multiprocessing import Process
from app.application import Application
from api.api import create_app

# Flask API を別プロセスで起動
api_process = Process(target=lambda: create_app().run())
api_process.start()

# AFK監視アプリを実行
app = Application()
app.run()

# 終了処理
api_process.terminate()
```

---

## クラス設計の概要（抜粋）

- `afk_service.py`：AFK時間の集計を行うサービス層
- `keyboard_activity_detector.py`：キーボードの入力有無を検知
- `mouse_activity_detector.py`：マウス操作の有無を検知
- `non_blocking_timer.py`：一定間隔で非同期に関数を実行する仕組み
- `afk_repository.py`：AFKログを PostgreSQL に永続化するリポジトリ層

---

## 今後の追加方針（任意）

- API のエンドポイントは `routes/` 配下に分割して追加
- 共通ロジックは `shared/` に集約（特に `utils/` と `models/`）
- テストコードは `test/` に配置予定（未整備）

---

以上の構成をベースに、個別機能の追加・分担・レビューを行ってください。
