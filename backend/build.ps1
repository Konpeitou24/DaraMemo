<#
.SYNOPSIS
    DaraMemo アプリケーションのビルドスクリプト
.DESCRIPTION
    Nuitka を使って Python スクリプトを Windows 向けにビルドし、/bin に出力します。
#>

#----------------------#
# 設定
#----------------------#
$entryPoint = "backend/main.py"
$pythonExe = "venv\Scripts\python.exe"
$buildDir = "bin"

# binフォルダがなければ作成
if (!(Test-Path $buildDir)) {
    Write-Host "📁 bin フォルダを作成します..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $buildDir | Out-Null
}

$nuitkaFlags = @(
    "--standalone",
    "--onefile",
    "--windows-disable-console",
    "--output-dir=$buildDir"
)

#----------------------#
# 実行
#----------------------#
Write-Host "==> ビルドを開始します..." -ForegroundColor Cyan

if (!(Test-Path $pythonExe)) {
    Write-Error "Python 仮想環境が見つかりません: $pythonExe"
    exit 1
}

& $pythonExe -m nuitka $nuitkaFlags $entryPoint

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ ビルド完了: $buildDir" -ForegroundColor Green
} else {
    Write-Error "❌ ビルドに失敗しました"
}
