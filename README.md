# ADKOM Build Number

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE.md)
[![Unity](https://img.shields.io/badge/Unity-2021.3%2B-black.svg?logo=unity)](https://unity.com)
[![UPM](https://img.shields.io/badge/UPM-com.adkom.buildnumber-blue.svg)](#installation)
[![Ko-fi](https://img.shields.io/badge/Ko--fi-support-ff5e5b.svg?logo=kofi&logoColor=white)](https://ko-fi.com/adkomgames)

Automatically increments a project-static build number after each successful
script compilation, and exposes a simple static API for reading the current
build number at runtime.

A tiny, zero-dependency Unity package that does one thing correctly: the number
lives as plain text in **your** project (no Git LFS, serialization-mode
agnostic), each project keeps its own independent counter, and one line of code
reads it anywhere — Editor, Play mode, or built players.

## Installation

In Unity, open **Window → Package Manager → + → Install package from git URL…**
and enter:

```
https://github.com/adkom-games/ADKOM_Build_Number.git#1.2.0
```

Or add it directly to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.adkom.buildnumber": "https://github.com/adkom-games/ADKOM_Build_Number.git#1.2.0"
  }
}
```

The `#1.2.0` suffix pins the install to that git tag for reproducible builds; drop
it to track the default branch tip.

## Usage

There is a single public API — read the current build number from anywhere in
your runtime code:

```csharp
using ADKOM;

int build = BuildNumber.Current; // 0 if the asset doesn't exist yet
Debug.Log($"Build {build}");
```

`BuildNumber.Current` returns the current number (or `0` if it hasn't been created
yet) and works in the Editor, in Play mode, and in built players. That's the whole
public surface.

## How it works

- The number is stored as a **plain-text file** the package creates in **your**
  project at `Assets/Settings/Resources/BuildNumber.txt` (the file simply contains
  the integer). Being plain text, it never needs Git LFS and is unaffected by your
  project's asset serialization mode.
- Being under a `Resources/` folder, it loads at runtime via `Resources.Load` as a
  `TextAsset`.
- `BuildNumberAutoIncrement` (Editor-only) hooks `CompilationPipeline.compilationFinished`
  and increments the number after every **successful** compile. Failed compiles
  are skipped.
- Because the file lives in the consuming project (not the package), each project
  keeps its own independent build counter, and it will change on every compile —
  expect it to show up frequently in source control.

### Editor menu

**Tools → ADKOM Build Number** shows the current value and lets you adjust it
manually:

- **Current Build Number N** — display only (always disabled).
- **Increment Build Number** — adds 1 to the stored number.
- **Decrement Build Number** — subtracts 1; clamped at 0 and disabled when the
  number is already 0.

### Verbose logging

Set `BuildNumberAutoIncrement.VerboseLogging = true` (an `EditorPrefs`-backed
toggle) to log each increment to the Console.

## Requirements

Unity 2021.3 or newer.

## More from ADKOM

Made by [A Different Kind Of Mind Games](https://www.adkomgames.com) — a small
studio building video games, and the tools that make building them nicer.
Everything we publish is MIT licensed and installable straight from GitHub.

- [ADKOM Text Editor](https://github.com/adkom-games/ADKOM_TextEditor) — a real
  IDE-grade code editor living inside the Unity Editor
- [ADKOM Inifile](https://github.com/adkom-games/ADKOM_Inifile) — dependency-free
  INI reader + self-documenting INI generator
- [ADKOM Localization](https://github.com/adkom-games/ADKOM_Localization) —
  lightweight UI Toolkit localization with automatic RTL mirroring

If a tool saves you time, a coffee is always appreciated —
[ko-fi.com/adkomgames](https://ko-fi.com/adkomgames) — but the code is free, forever.

## License

MIT — see [LICENSE.md](LICENSE.md).
