# DaraMemo 状態管理 API 定義

> 目的: 「状態切替（ACTIVE/AFK/BREAK）」と「現在状態の取得」に絞った最小API定義。UIはボタン操作のみを想定し、作業結果の集計やグラフ表示はDBから取得すれば良いため、バックエンドからフロントエンドへの能動的通信は不要とする。

## 概要

1. AFK | BREAK | ACTIVE 状態に関するAPI
2. 作業時間や休憩時間など、時間に関するAPI
3. DBへ送信を行わせる、データ保存などに関するAPI



## 1. Frontend → Backend（REST / JSON）

### 1.1 状態取得（現在の状態を知る）

* **GET** `/api/v1/state/current?session_id=...`
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

* **推奨**: **POST** `/api/v1/states/set`

  * **Req**: `session_id`, `new_state: ACTIVE|AFK|BREAK`, `timestamp`, `reason?`, `idempotency_key?`
  * **Res**: `prev_segment?`, `new_segment`, `effective_state`, `server_time`, `idempotent`
* **糖衣**（任意）:

  * **POST** `/states/active` ／ `/states/afk` ／ `/states/break`

### 1.3 タイムライン参照（FE計算用・軽量）

* **GET** `/api/v1/sessions/timeline?session_id=...`
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
* **備考**: フロントエンドは `now - session_started_at - Σ(breakDurations)` で\*\*現在の作業時間（概算）\*\*を算出可能。`end:null` は進行中の休憩を意味する。



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

* **UI要件**: フロントエンドは「状態切替ボタン」のみ。グラフ等の可視化は **DB から直接取得**して別途表示（APIの範囲外）。
* **BE→FE通信**: **不要**。Push（SSE/WebSocket）は採用しない。
* **状態参照**: 必要時のみ `GET /state/current` を呼び出す（定期ポーリングは任意、既定は不採用）。
* **将来拡張候補**: 自動AFK検知や複数クライアント同期が必要になった場合に限り、SSE等のPushを検討。



## 5. データ保存 API（セーブ指示）

* **POST** `/api/v1/sessions/end`

  * **Req**: `session_id`, `ended_at`, `summary{work_seconds, afk_seconds, break_seconds}`
  * **Res**: `session_id`, `stored`
* **備考**: 作業終了時に呼び出し、集計データをDBへ保存する。セーブ操作の明示的なトリガーとなる。



## まとめ

必要機能として

* 休憩モード・作業再開等、モード切替
* タイムライン取得
* 作業終了時にセーブを行う指示
