<#
.SYNOPSIS
    DaraMemo アプリケーションのビルドスクリプト
.DESCRIPTION
    Nuitka を使って Python スクリプトを Windows 向けにビルドし、/bin に出力します。
#>

# 元のカレントディレクトリを保存
$originalLocation = Get-Location
Set-Location $PSScriptRoot
Write-Host "[INFO] スクリプトのディレクトリに移動: $PSScriptRoot"

$venvActivated = $false

try {
    # 仮想環境の候補パス一覧
    $venvCandidates = @(
        ".venv\Scripts\Activate.ps1",
        "venv\Scripts\Activate.ps1",
        "env\Scripts\Activate.ps1",
        ".env\Scripts\Activate.ps1"
    )

    foreach ($activatePath in $venvCandidates) {
        $fullPath = Join-Path $PSScriptRoot $activatePath
        if (Test-Path $fullPath) {
            Write-Host "[INFO] 仮想環境が見つかりました。仮想環境を有効化します: $fullPath"
            & $fullPath
            $venvActivated = $true
            break
        } else {
            Write-Host "[WARN] 仮想環境が見つかりません: $fullPath"
        }
    }

    if (-not $venvActivated) {
        Write-Host "[ERROR] 仮想環境が見つかりませんでした。ビルドを中止します。"
        return
    }

    # bin ディレクトリがなければ作成
    if (-not (Test-Path -Path "./bin")) {
        New-Item -ItemType Directory -Path "./bin" | Out-Null
        Write-Host "[INFO] ./bin ディレクトリを作成しました。"
    }

    # Nuitka ビルドコマンド
    Write-Host "[INFO] Nuitka によるビルドを開始します..."
    python -m nuitka ./main.py `
        --onefile `
        --windows-console-mode=disable `
        --output-dir=bin
    Write-Host "[SUCCESS] ビルドが完了しました。./bin に出力されました。"
}
finally {
    Set-Location -Path $originalLocation
    Write-Host "[INFO] 元のディレクトリに戻りました: $originalLocation"
}
