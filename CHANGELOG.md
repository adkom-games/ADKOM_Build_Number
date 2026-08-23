# Changelog

All notable changes to this package are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.0] - 2026-08-23

### Added
- **Tools > ADKOM Build Number** editor menu:
  - **Current Build Number N** — disabled display item showing the live value.
  - **Increment Build Number** / **Decrement Build Number** — manually adjust the
    stored number (decrement is clamped at 0 and disabled when already 0).

## [1.1.0] - 2026-07-07

### Changed
- **Single public API:** all runtime access now goes through `ADKOM.BuildNumber.Current`.
  The previous `BuildNumberData` accessors and the editor-only `GetBuildNumber()`
  have been removed to eliminate confusion over which API to use.
- **Storage is now a plain-text file** (`Assets/Settings/Resources/BuildNumber.txt`)
  instead of a `ScriptableObject` `.asset`. This guarantees the build-number file is
  always text — never binary, never needing Git LFS — regardless of the consuming
  project's asset serialization mode.

### Removed
- `BuildNumberData` ScriptableObject and its `CurrentBuildNumber` / `GetCurrent()` /
  `Increment()` members.
- `BuildNumberAutoIncrement.GetBuildNumber()`.

## [1.0.0] - 2026-07-07

### Added
- `BuildNumberData` ScriptableObject that stores the project build number and
  exposes `CurrentBuildNumber` / `GetCurrent()` for runtime access via `Resources`.
- `BuildNumberAutoIncrement` editor automation that increments the build number
  after each successful script compilation, creating the backing asset on first run.
- `VerboseLogging` toggle for per-increment console logging.
- Packaged for installation via Unity Package Manager git URL.
