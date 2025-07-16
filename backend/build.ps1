<#
.SYNOPSIS
    DaraMemo アプリケーションのビルドスクリプト
.DESCRIPTION
    Nuitka を使って Python スクリプトを Windows 向けにビルドし、/bin に出力します。
#>

# 元のカレントディレクトリを保存
$originalLocation = Get-Location

# スクリプトのあるディレクトリの1階層上（プロジェクトルート）に移動
Set-Location $PSScriptRoot
Write-Host $PSScriptRoot

try {
    # 仮想環境の候補パス一覧
    $venvCandidates = @(
        ".venv\Scripts\Activate.ps1",   # 一般的なパターン1
        "venv\Scripts\Activate.ps1",    # venvという名前（VSCodeなど）
        "env\Scripts\Activate.ps1",     # 古いプロジェクトや他ツールで作成
        ".env\Scripts\Activate.ps1"     # 隠しディレクトリ形式
    )

    foreach ($activatePath in $venvCandidates) {
        $joinedPath = Join-Path $PSScriptRoot $activatePath
        if (Test-Path $joinedPath) {
            Write-Host "仮想環境を有効化します: $joinedPath"
            & $joinedPath
            break
        } else {
            Write-Host "仮想環境が見つかりませんでした（試行済み: $joinedPath）"
        }
    }
    # bin ディレクトリが存在しない場合は作成
    if (-not (Test-Path -Path "./bin")) {
        New-Item -ItemType Directory -Path "./bin"
    }
    # Nuitka を使って Python スクリプトをビルド
    python -m nuitka .\main.py `
        --standalone `
        --onefile `
        --include-data-dir=templates=templates `
        --include-data-dir=static=static `
        --output-dir=./bin `
        --windows-disable-console `
        --enable-plugin=flask `
        --enable-plugin=pylint-warnings `
        --nofollow-import-to=tkinter `
        --include-package=jinja2 `
        --include-package-data=jinja2 `
        --include-package-data=flask
}
finally {
    # 元のカレントディレクトリに戻す
    Set-Location -Path $originalLocation
}
