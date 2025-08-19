# DaraMemo 状態管理 API 定義

- 「状態切替（ACTIVE/AFK/BREAK）」と「現在状態の取得」に絞った最小API定義。
- フロントエンド機能に追従するようなAPIの作成を求める。
  - バックエンド処理の停止のリクエスト
  - 休憩状態へ変更要求、また作業状態への変更要求。
  - 現在の状態の通知（ACTIVE/AFK/BREAK）はWebSocketを通じて行われる。
- 状態は（ACTIVE/AFK/BREAK）として、大文字小文字を問わないとする。受け取り側はすべて大文字に変換後処理を行う。

## 概要

1. AFK | BREAK | ACTIVE 状態に関するAPI
2. 作業時間や休憩時間など、時間に関するAPI
3. セーブや終了に関するAPI
4. Afk,Break,Active　状態の変化があった際、または数秒に一回WebSocketを通じて送信するAPI

## 1. Frontend → Backend（REST / JSON）

### 1.1 状態取得（現在の状態を知る）

* **GET** `/api/state/current?session_id=...`
* **Res（例）**

  ```json
  {
    "effective_state": "ACTIVE|AFK|BREAK",
    "segment": { "state": "ACTIVE|AFK|BREAK", "started_at": "date-time" },
    "server_time": "date-time"
  }
  ```
* **備考**: UI初期表示や必要に応じて参照するために使用。

### 1.2 状態切替

* **推奨**: **POST** `/api/states/set`

  * **Req**: `session_id`, `new_state: ACTIVE|AFK|BREAK`, `timestamp`, `reason?`, `idempotency_key?`
  * **Res**: `prev_segment?`, `new_segment`, `effective_state`, `server_time`, `idempotent`
* **糖衣**（任意）:

  * **POST** `/states/active` ／ `/states/afk` ／ `/states/break`

### 1.3 タイムライン参照（FE計算用・軽量）

* **GET** `/api/sessions/timeline?session_id=...`
* **Res（例）**

  ```json
  {
    "session_id": "sess_01H...XYZ",
    "session_started_at": "date-time",
    "breaks": [
      { "start": "date-time", "end": "date-time" },
      { "start": "date-time", "end": null }
    ],
    "afks": [
      { "start": "date-time", "end": "date-time" },
      { "start": "date-time", "end": null }
    ]
  }
  ```
* **備考**: フロントエンドは `now - session_started_at - Σ(breakDurations)` で**現在の作業時間（概算）**を算出可能。`end:null` は進行中の休憩を意味する。

## 2. エラーモデル（共通）

```json
{
  "error": {
    "code": "invalid_argument|not_found|conflict",
    "message": "human readable",
    "details": { }
  }
}
```


## 3. 最小実装セット（優先度）

1. `POST /states/set`（または糖衣3種）
2. `GET /state/current`
3. `GET /sessions/timeline`（FE計算用：開始時刻・休憩区間・AFK区間）


## 4. スコープ・方針（今回のハッカソン）

* **UI要件**: フロントエンドは「状態切替ボタン」のみ。グラフ等の可視化は **DB から直接取得** して別途表示（APIの範囲外）。
* **BE→FE通信**: ~~**不要。**Push（SSE/WebSocket）は採用しない。~~
  * 必要。状態変化を検知し、最新の状態（ACTIVE/AFK/BREAK）をJson形式で送信する。
* **状態参照**: 必要時のみ `GET /state/current` を呼び出す（定期ポーリングは任意、既定は不採用）。
  * 定期ポーリング不要、状態変化通知をWebSocketを通じて採用するので。
* **将来拡張候補**: 自動AFK検知や複数クライアント同期が必要になった場合に限り、SSE等のPushを検討。
## 5. データ保存&終了 API（セーブ指示）

* **POST** `/api/sessions/save`
* **POST** `/api/sessions/end`

  * **Req**: `session_id`, `ended_at`, `summary{work_seconds, afk_seconds, break_seconds}`
  * **save Res**: `session_id`, `stored`, `saved_at`
  * **end Res**: `session_id`, `stored`
* **備考**: 作業終了時に呼び出し、~~集計データをDBへ保存する。~~ ログへ保存する。セーブ操作の明示的なトリガーとなる。
* もしデータを保存できたら保存パスなどをレスポンスしても便利だと思う。

> DB送信実装は不要。DBは将来的な実装予定とし、今回はDBサーバーへのデータ保存は行わないものとする。
> 代わりにログを保存するように。

## 6. 状態変化通知

WebSocketを通じて状態変化通知を行う。

内容は見られても大丈夫なのでオープンで行う。

```t
# endpoint
ws://<hostip>/ws/state
```

- send_text **定期実行**
```json
{ "type": "ticked", "status": "active", "server_time": "2025-08-19T00:10:05Z" }
```

- send_text **変更時実行**
```json
{ "type": "changed", "status": "break", "server_time": "2025-08-20T09:30:05Z" }
```

## まとめ

必要機能として

* 休憩モード・作業再開等、モード切替
* タイムライン取得
* 作業終了時にセーブを行う指示
* 状態変化通知