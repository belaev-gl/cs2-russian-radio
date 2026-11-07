# CS2 Russian Radio

Полная русская озвучка игрового радио Cities: Skylines II: **283 клипа, ~124 минуты** — новости, погода, экстренные оповещения, PSA, болталки ведущих и реклама на пяти станциях. Работает как BepInEx-плагин: подменяет аудиоклипы в рантайме, **файлы игры не изменяются**, content integrity check не затрагивается.

> **Версия игры:** проверено на **Cities: Skylines II 1.5.7f1** (Unity 2022.3.71f1). На других версиях не проверялось.

> ▶️ **[Слушать демо прямо на сайте](https://belaev-gl.github.io/cs2-russian-radio/)** — страница с плеером, скачивать не нужно.

### Пример прослушки → в игре

**🔊 [Общая демо-подборка, 66 сек](samples/demo-montage.mp3)** — все станции в одном файле.

| Файл | Тип | Станция |
|---|---|---|
| [demo-montage.mp3](samples/demo-montage.mp3) | демо (погода → alert → PSA → болталки) | все |
| [weather-female-secondmoon-01.ogg](samples/weather-female-secondmoon-01.ogg) | погода | The Second Moon |
| [alert-female-secondmoon-01.ogg](samples/alert-female-secondmoon-01.ogg) | экстренное оповещение | The Second Moon |
| [psa-male-secondmoon-03.ogg](samples/psa-male-secondmoon-03.ogg) | PSA | The Second Moon |
| [short-blurb-secondmoon-01.ogg](samples/short-blurb-secondmoon-01.ogg) | короткая подводка | The Second Moon |
| [host-female-atmosphericpiano-vo1.ogg](samples/host-female-atmosphericpiano-vo1.ogg) | ведущая | Atmospheric Piano |
| [host-female-cloudlounge-vo1.ogg](samples/host-female-cloudlounge-vo1.ogg) | ведущая | Cloud Lounge FM |
| [host-male-smoothvibe-vo12.ogg](samples/host-male-smoothvibe-vo12.ogg) | ведущий | Smooth Vibe FM |
| [host-male-feelgoodfunk-vo1.ogg](samples/host-male-feelgoodfunk-vo1.ogg) | ведущий | Feelgood Funk |

## Как это сделано

1. **Транскрипция** оригиналов — Whisper.
2. **Перевод** сценариев на русский — DeepSeek V4.1 Flash (контекст всей радиопрограммы, не построчно).
3. **Озвучка** — два голоса Gemini TTS: женский Kore, мужской Puck. Один ведущий не «переезжает» со станции на станцию — за каждым закреплён свой голос.
4. **Подгонка под оригинал** — тональность темпа, LUFS, длина, каналы и sample rate как у исходных файлов; в Phase 2 русская речь смешана с оригинальными инструменталами (Demucs).
5. **OGG Vorbis с игровыми тегами** (`radio channel`, `radio station`, `alert type` и т.д.) — игра воспринимает их как свои.

Музыкальные треки и 3 инструментальных джингла без речи остаются оригинальными.

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

Скрипт также можно запустить прямо из git-клона — тогда он сам скачает архив последнего релиза.

3. Запусти игру, включи радио.

Проверка / Verify — `<Игра>\BepInEx\LogOutput.log`:

```text
[CS2RussianRadio] loaded 283 clips, keys 563
[CS2RussianRadio] replaced 'old-weather-003' -> 'old-weather-003'
```

Имена реально проигранных клипов логируются в `<Игра>\BepInEx\plugins\CS2RussianRadio\clip_names.log` — если какой-то клип не заменился, это сразу видно.

## Удаление / Uninstall

```powershell
.\Uninstall.ps1 -GameRoot "C:\Путь\К\Cities Skylines II"
```

Или удали `<Игра>\BepInEx\plugins\CS2RussianRadio` — мод целиком живёт в одной папке, никаких правок системных файлов игры.

## Сборка / Build

Собирается на любой машине с игрой и BepInEx 6:

```powershell
dotnet build src\CS2RussianRadio.csproj -c Release -p:GameRoot="C:\Путь\К\Cities Skylines II"
```

Готовая DLL: `src\bin\Release\netstandard2.1\CS2RussianRadio.dll` → положить в `<Игра>\BepInEx\plugins\CS2RussianRadio\`, рядом папку `music\` с OGG из релиза.

## Ограничения

- Это AI-озвучка: интонации синтетические, возможны ошибки перевода.
- Подмена происходит по имени клипа; если в будущих версиях игры появятся новые клипы — их не будет в паке до обновления.
- Все 283 клипа грузятся в память при старте (~1.3 ГБ PCM).
- Совместимость подтверждена только для **1.5.7f1**; после крупного обновления игры может потребоваться обновление BepInEx и плагина.

## Дисклеймер

Неофициальный AI-мод. Не связан с Paradox Interactive или Colossal Order. Нужна лицензионная копия игры. Условия: [EULA-RU.txt](EULA-RU.txt), сторонние компоненты: [THIRD-PARTY-NOTICES.txt](THIRD-PARTY-NOTICES.txt).
