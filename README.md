# ADKOM Build Number

Automatically increments a project-static build number after each successful
script compilation, and exposes a simple static API for reading the current
build number at runtime.

## Installation

In Unity, open **Window → Package Manager → + → Install package from git URL…**
and enter:

```
https://github.com/adkom-games/ADKOM_Build_Number.git
```

Or add it directly to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.adkom.buildnumber": "https://github.com/adkom-games/ADKOM_Build_Number.git"
  }
}
```

To pin a version, append `#1.0.0` (a git tag) to the URL.

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

### Verbose logging

Set `BuildNumberAutoIncrement.VerboseLogging = true` (an `EditorPrefs`-backed
toggle) to log each increment to the Console.

## Requirements

Unity 2021.3 or newer.

## License

MIT — see [LICENSE.md](LICENSE.md).
