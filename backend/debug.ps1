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
    # Python スクリプトを実行
    python main.py @args
}
finally {
    # 元のカレントディレクトリに戻す
    Set-Location -Path $originalLocation
}
