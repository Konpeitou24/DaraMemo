# debug.ps1

このスクリプトは、DaraMemo プロジェクトの Python バックエンドを **簡易的にデバッグ実行**するための PowerShell スクリプトです。  
仮想環境の有効化・エントリポイントの実行・ディレクトリの復元までを一括で行う構成となっています。

## 主な構成

- **カレントディレクトリの保存・復元**  
  スクリプト実行前のディレクトリを保存し、処理後に元に戻すことで、他の作業ディレクトリへの影響を防ぎます。

- **プロジェクトルートへの移動**  
  `$PSScriptRoot` を用いて、スクリプトの格納ディレクトリ（通常は `public/`）の1階層上、つまり**プロジェクトのルート**に移動します。

- **仮想環境の自動検出と有効化**  
  以下の複数の候補から仮想環境の `Activate.ps1` を自動で探し、有効化を試みます（存在しない場合はその旨を出力）：
  - `.venv/Scripts/Activate.ps1`
  - `venv/Scripts/Activate.ps1`
  - `env/Scripts/Activate.ps1`
  - `.env/Scripts/Activate.ps1`

- **main.py の起動**  
  仮想環境が有効化された状態で `main.py` を実行します。`@args` を付けることで、スクリプトへの引数をそのまま Python 側に渡すことが可能です。

- **finally ブロックによる復元処理**  
  仮想環境の有効化や Python 実行が失敗した場合でも、必ず元のディレクトリへ戻るように設計されています。

## 用語解説

- **PowerShell**  
  Windows に標準搭載されているコマンドラインシェルで、スクリプトを使った自動化や設定変更が可能です。`.ps1` 拡張子のファイルがスクリプトになります。

- **仮想環境（venv）**  
  Python プロジェクトごとに依存ライブラリのバージョンを分離・管理する仕組みです。本プロジェクトでも、`requirements.txt` に記載された依存関係をこの環境内でインストール・実行します。

- **Activate.ps1**  
  PowerShell 上で Python 仮想環境を有効化するためのスクリプトです。有効化により、以降の `python` コマンドが仮想環境内の実行環境を使うようになります。

- **@args**  
  PowerShell の構文で、スクリプトに渡されたすべての引数をそのまま `python main.py` に引き渡すために使われます。


## 互換性と注意点

- **PowerShell 7 以降の利用を推奨**  
  本スクリプトは、一部の環境（特に古い PowerShell 5.x）では正常に動作しないことがあります。  
  `$PSScriptRoot` の動作やエラーハンドリングの互換性を考慮し、**PowerShell 7 以降の使用を推奨**します。  
  PowerShell のバージョンは以下のコマンドで確認できます：

  ```powershell
  $PSVersionTable.PSVersion
  ```

  必要に応じて以下の公式サイトから最新版をインストールしてください：  
  https://github.com/PowerShell/PowerShell

- **Windows 環境専用スクリプト**  
  本スクリプトは Windows を前提とした `.ps1` 形式であり、Mac や Linux では動作しません。Unix系OSでは手動で仮想環境を有効化して `python main.py` を実行してください。

- **実行ポリシーに注意**  
  PowerShell の実行ポリシーが制限されている場合、`.ps1` ファイルを実行できないことがあります。以下のコマンドでポリシーを確認し、必要に応じて緩和してください（管理者権限）：

  ```powershell
  Get-ExecutionPolicy
  Set-ExecutionPolicy RemoteSigned
  ```


## 補足

このスクリプトは開発用の簡易実行手段として提供されており、デプロイ環境での本番運用は想定していません。  
将来的に `Makefile` や `invoke`、`.bat`/`.sh` 形式のクロスプラットフォーム対応も検討可能です。
