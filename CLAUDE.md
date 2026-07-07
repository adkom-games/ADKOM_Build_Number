# ADKOM Build Number

A small Unity package that automatically increments a project-static build number
and exposes a static interface for retrieving the current value at runtime.

- Engine target: Unity **2021.3+** (`package.json`). Developed on **6000.3.19f1**, URP.
- Namespace: `ADKOM`. Asmdefs: `ADKOM.BuildNumber` (runtime) + `ADKOM.BuildNumber.Editor` (editor-only).
- **This repo IS the UPM package** — `package.json` is at the repo root, installable
  via git URL with **no `?path=`**: `https://github.com/adkom-games/ADKOM_Build_Number.git`.

## Repository layout

```
package.json  README.md  CHANGELOG.md  LICENSE.md   <- package manifest & docs (repo root)
Runtime/   ADKOM.BuildNumber.asmdef + BuildNumberData.cs
Editor/    ADKOM.BuildNumber.Editor.asmdef + BuildNumberAutoIncrement.cs
Dev~/      Unity test project (embeds the package via "file:../.."; the trailing
           "~" hides it from Unity's package importer). Open THIS in Unity to test.
```

- The package source lives at the repo root; **do not** move it back under a
  Unity project's `Assets/`.
- To develop/test: open `Dev~/` as a Unity project. Its `Packages/manifest.json`
  references the package with `"com.adkom.buildnumber": "file:../.."`.

## How it works

- **`BuildNumberData`** (`Runtime/BuildNumberData.cs`) — a `ScriptableObject`
  holding a single serialized `int buildNumber`. The concrete asset is created in
  the *consuming* project at `Assets/Settings/Resources/BuildNumberData.asset` so it
  loads via `Resources.Load` in the Editor, in Play mode, and in built players.
- **`BuildNumberAutoIncrement`** (`Editor/BuildNumberAutoIncrement.cs`) —
  `[InitializeOnLoad]` editor class that hooks `CompilationPipeline.compilationFinished`
  and calls `Increment()` after each *successful* compilation (skips when
  `scriptCompilationFailed`). Creates the asset (and its Resources folder) on first run.

## Public interface

Runtime code should use the static accessors on `BuildNumberData`:

```csharp
int build = ADKOM.BuildNumberData.CurrentBuildNumber;   // 0 if asset missing
ADKOM.BuildNumberData instance = ADKOM.BuildNumberData.GetCurrent(); // cached, may be null
```

- `BuildNumberData.CurrentBuildNumber` — the value, or `0` if no asset is found. Use this.
- `BuildNumberData.GetCurrent()` — cached asset instance (or `null`), Editor + player.
- `BuildNumberAutoIncrement.GetBuildNumber()` — **Editor-only** accessor via
  `AssetDatabase`. Do not use from runtime code; prefer `CurrentBuildNumber`.
- `BuildNumberAutoIncrement.VerboseLogging` — `EditorPrefs`-backed toggle for
  `[ADKOM]` increment/creation log messages.

## Conventions & gotchas

- The `.asset` is the source of truth for the number; it lives in the *consumer's*
  project (or `Dev~/` here), changes on every compile, and belongs to that project —
  not the package.
- Runtime asmdef must stay free of editor references — `AssetDatabase`/`UnityEditor`
  usage belongs only in the `.Editor` asmdef.
- Keep the runtime accessor null-safe (asset may be absent in a fresh clone until the
  first compile regenerates it).
