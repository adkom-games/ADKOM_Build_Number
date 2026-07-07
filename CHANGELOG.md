# Changelog

All notable changes to this package are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-07-07

### Added
- `BuildNumberData` ScriptableObject that stores the project build number and
  exposes `CurrentBuildNumber` / `GetCurrent()` for runtime access via `Resources`.
- `BuildNumberAutoIncrement` editor automation that increments the build number
  after each successful script compilation, creating the backing asset on first run.
- `VerboseLogging` toggle for per-increment console logging.
- Packaged for installation via Unity Package Manager git URL.
