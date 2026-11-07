<#
.SYNOPSIS
  Remove CS2 Russian Radio plugin.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$GameRoot = ""
)
$ErrorActionPreference = "Stop"
try { [Console]::OutputEncoding = [System.Text.Encoding]::UTF8 } catch {}
if (-not $GameRoot) { $GameRoot = Read-Host "Путь к папке Cities Skylines II" }
$gameRoot = (Resolve-Path $GameRoot).Path
$dst = Join-Path $gameRoot "BepInEx\plugins\CS2RussianRadio"
if (-not (Test-Path $dst)) { Write-Host "Плагин не установлен." -ForegroundColor Yellow; return }
Remove-Item $dst -Recurse -Force
Write-Host "Удалено: $dst" -ForegroundColor Green
