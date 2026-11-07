# CS2 Russian Radio

Русская озвучка радио для Cities: Skylines II. 283 клипа (~124 мин): новости, погода, экстренные оповещения, PSA, болталки ведущих, реклама. BepInEx-плагин подменяет `AudioClip` в рантайме — файлы игры не изменяются, integrity check не затрагивается.

Russian radio voiceover for Cities: Skylines II. 283 clips (~124 min): news, weather, alerts, PSAs, host segments, commercials. A BepInEx plugin swaps `AudioClip` at runtime — game files stay untouched.

## Требования / Requirements

- Windows
- Cities: Skylines II (проверено на 1.5.7f1 / tested on 1.5.7f1)
- [BepInEx 6 — Unity Mono, win-x64](https://github.com/BepInEx/BepInEx/releases) в папке игры (запусти игру один раз после установки)

## Установка / Install

1. Скачай архив из [Releases](https://github.com/belaev-gl/cs2-russian-radio/releases/latest).
2. Распакуй. Запусти PowerShell в папке архива:

```powershell
.\Install.ps1 -GameRoot "C:\Путь\К\Cities Skylines II"
```

3. Запусти игру, включи радио.

Проверка / Verify — `<Игра>\BepInEx\LogOutput.log`:

```text
[CS2RussianRadio] loaded 283 clips, keys 563
[CS2RussianRadio] replaced 'old-weather-003' -> 'old-weather-003'
```

Имена реально проигранных клипов: `<Игра>\BepInEx\plugins\CS2RussianRadio\clip_names.log`.

## Удаление / Uninstall

```powershell
.\Uninstall.ps1 -GameRoot "C:\Путь\К\Cities Skylines II"
```

Или удали `<Игра>\BepInEx\plugins\CS2RussianRadio`.

## Сборка / Build

Собирается на любой машине с игрой и BepInEx 6:

```powershell
dotnet build src\CS2RussianRadio.csproj -c Release -p:GameRoot="C:\Путь\К\Cities Skylines II"
```

Готовая DLL: `src\bin\Release\netstandard2.1\CS2RussianRadio.dll` → положить в `<Игра>\BepInEx\plugins\CS2RussianRadio\`, рядом папку `music\` с OGG из релиза.

## Состав пакета

- `BepInEx\plugins\CS2RussianRadio\CS2RussianRadio.dll`
- `BepInEx\plugins\CS2RussianRadio\music\` — 283 OGG
- Музыкальные треки и 3 инструментальных джингла остаются оригинальными

## Дисклеймер

Неофициальный AI-мод. Не связан с Paradox Interactive или Colossal Order. Нужна лицензионная копия игры. Условия: [EULA-RU.txt](EULA-RU.txt), сторонние компоненты: [THIRD-PARTY-NOTICES.txt](THIRD-PARTY-NOTICES.txt).
