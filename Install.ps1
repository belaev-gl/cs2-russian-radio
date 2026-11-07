<#
.SYNOPSIS
  Install CS2 Russian Radio BepInEx plugin.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$GameRoot = ""
)

$ErrorActionPreference = "Stop"
try { [Console]::OutputEncoding = [System.Text.Encoding]::UTF8 } catch {}

if (-not $GameRoot) {
    $GameRoot = Read-Host "Путь к папке Cities Skylines II (где лежит Cities2.exe)"
}
$gameRoot = (Resolve-Path $GameRoot).Path
if (-not (Test-Path (Join-Path $gameRoot "Cities2.exe"))) {
    throw "Не найден Cities2.exe в '$gameRoot'."
}
if (-not (Test-Path (Join-Path $gameRoot "BepInEx\core\BepInEx.Core.dll"))) {
    throw "BepInEx не установлен. Сначала установи BepInEx 6 (Unity Mono, win-x64) в папку игры."
}

$src = Join-Path $PSScriptRoot "BepInEx\plugins\CS2RussianRadio"

# Если скрипт запущен без архива (например, из git-репо) — скачать последний релиз.
if (-not (Test-Path $src)) {
    Write-Host "Мод рядом со скриптом не найден, скачиваю последний релиз..." -ForegroundColor Yellow
    $tmp = Join-Path ([IO.Path]::GetTempPath()) ("cs2-russian-radio-" + [Guid]::NewGuid().ToString("N"))
    New-Item -ItemType Directory -Force -Path $tmp | Out-Null
    $zip = Join-Path $tmp "CS2RussianRadio_VoicePack.zip"
    $url = "https://github.com/belaev-gl/cs2-russian-radio/releases/latest/download/CS2RussianRadio_VoicePack_v1.0.0.zip"
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-WebRequest -Uri $url -OutFile $zip -UseBasicParsing
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [IO.Compression.ZipFile]::ExtractToDirectory($zip, $tmp)
    $src = Join-Path $tmp "BepInEx\plugins\CS2RussianRadio"
    if (-not (Test-Path $src)) { throw "После распаковки релиза не найдено: $src" }
}

$dst = Join-Path $gameRoot "BepInEx\plugins\CS2RussianRadio"

if (Test-Path $dst) { Remove-Item $dst -Recurse -Force }
Copy-Item $src $dst -Recurse -Force
Write-Host "Установлено: $dst" -ForegroundColor Green
Write-Host "Закрой игру перед установкой, если она запущена." -ForegroundColor Yellow
Write-Host "Проверка: BepInEx\LogOutput.log должен содержать 'loaded 283 clips'." -ForegroundColor Yellow
