# start.ps1

# スクリプトのあるディレクトリに移動
Set-Location -Path $PSScriptRoot 
if ($?) {
    # Python 実行
    python ./main.py
} else {
    Write-Host "Failed to change directory to $PSScriptRoot"
}
