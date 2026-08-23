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
Runtime/   ADKOM.BuildNumber.asmdef + BuildNumber.cs
Editor/    ADKOM.BuildNumber.Editor.asmdef + BuildNumberAutoIncrement.cs + BuildNumberMenu.cs
ADKOM_Build_Number_Dev~/   Minimal Unity test project (built-in render pipeline,
           legacy input, no URP/Input System/etc). Embeds the package via
           "file:../.."; the trailing "~" hides it from Unity's package importer.
           Add THIS folder to Unity Hub to test.
```

- The package source lives at the repo root; **do not** move it back under a
  Unity project's `Assets/`.
- To develop/test: add `ADKOM_Build_Number_Dev~/` to Unity Hub. Its
  `Packages/manifest.json` references the package with
  `"com.adkom.buildnumber": "file:../.."`.
- The package has **no dependencies** (`package.json`), and the dev project is kept minimal
  on purpose (built-in pipeline, legacy input). Don't re-add URP / Input System /
  other feature packages unless a test genuinely needs them.

## How it works

- **`BuildNumber`** (`Runtime/BuildNumber.cs`) — a static class; the **single**
  public entry point. Reads the number via `Resources.Load<TextAsset>` and caches it.
  `ResourceName` (`"BuildNumber"`) is the shared source of truth for the asset name.
- **`BuildNumberAutoIncrement`** (`Editor/BuildNumberAutoIncrement.cs`) —
  `[InitializeOnLoad]` editor class that hooks `CompilationPipeline.compilationFinished`
  and, after each *successful* compilation (skips when `scriptCompilationFailed`),
  reads the current value, increments, and writes it back. Creates the file (and its
  Resources folder) on first run.
- **`BuildNumberMenu`** (`Editor/BuildNumberMenu.cs`) — internal `[InitializeOnLoad]`
  class providing the **Tools > ADKOM Build Number** menu: a disabled
  "Current Build Number N" display item (registered via reflection on Unity's
  internal `Menu.AddMenuItem`, since `[MenuItem]` labels are compile-time constants;
  re-registered each domain reload and after manual changes), plus
  Increment/Decrement commands (decrement clamps at 0 and is disabled at 0).
- Storage is a **plain-text file** in the *consuming* project at
  `Assets/Settings/Resources/BuildNumber.txt` — it just contains the integer. Chosen
  over a ScriptableObject `.asset` so it's always text (never binary, never LFS)
  regardless of the consumer's serialization mode.

## Public interface

**One API, on purpose** — do not add parallel accessors:

```csharp
int build = ADKOM.BuildNumber.Current;   // 0 if the file doesn't exist yet
```

- `BuildNumber.Current` — the value, or `0` if not created yet. Editor + player.
- `BuildNumberAutoIncrement.VerboseLogging` — `EditorPrefs`-backed toggle for
  `[ADKOM]` increment log messages (editor-only, not part of the read API).

## Conventions & gotchas

- The `.txt` file is the source of truth for the number; it lives in the *consumer's*
  project (or `ADKOM_Build_Number_Dev~/` here), changes on every compile, and belongs to that project —
  not the package.
- Runtime asmdef must stay free of editor references — `AssetDatabase`/`UnityEditor`
  usage belongs only in the `.Editor` asmdef.
- `BuildNumber.Current` caches in a static field; that's safe because a domain reload
  (which clears it) coincides with the recompile that changes the value.
