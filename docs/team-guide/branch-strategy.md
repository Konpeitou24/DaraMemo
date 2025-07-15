# Git ブランチ戦略（DaraMemo ハッカソン）

本プロジェクトは3人チームによる短期集中開発のため、**スピードと品質の両立**を目指して、簡略化された Git Flow ベースのブランチ戦略を採用します。

---

## 🧭 ブランチ構成図

```
main
└─ develop
    ├─ feature/...
    ├─ fix/...
    ├─ docs/...
    └─ refactor/...
```

---

## 🔢 ブランチの役割

| ブランチ名     | 用途                      | 備考                                         |
|----------------|---------------------------|----------------------------------------------|
| `main`         | 最終成果物用ブランチ      | 発表直前にマージ。常に動作可能な状態を保つ |
| `develop`      | 開発統合ブランチ          | 作業ブランチのマージ先                       |
| `feature/*`       | 機能追加用の作業ブランチ  | 例: `feature/toast-notification`              |
| `fix/*`        | バグ修正用の作業ブランチ  | 例: `fix/api-response-bug`                 |
| `docs/*`       | ドキュメント更新用        | 例: `docs/architecture-summary`            |
| `refactor/*`   | リファクタリング用        | 例: `refactor/backend-cleanup`             |

---

## 🛠 運用ルール

- すべての作業は `develop` からブランチを切ること
- コミットは原則 [Conventional Commits](https://www.conventionalcommits.org/ja/v1.0.0/) に従う
  - 例: `feature(frontend): トースト通知を実装`
- Pull Request（PR）で `develop` にマージ
- `main` は保護ブランチとし、PR経由でのみ更新可とする

---

## 💡 開発Tips

| 内容 | コマンド例 |
|------|------------|
| 作業ブランチの作成 | `git checkout -b feature/xxx` |
| リモートの最新取得 | `git fetch --prune` |
| `develop` の最新取得 | `git pull origin develop` |

---

## 🔐 ブランチ保護ルール（GitHub 設定）

- `main` は直接 push 不可。PR 経由でのみマージ可能にする
- 必要に応じて `develop` も保護対象とする

---

## ✅ まとめ

簡潔なブランチ戦略をもとに、スムーズで衝突の少ないチーム開発を行いましょう。

