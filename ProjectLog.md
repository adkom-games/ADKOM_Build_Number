# ADKOM Build Number — Project Evolution Log

A running record of development activity and decisions. Newest entry on top. This log was reconstructed from git history on 2026-07-08; entries dated before then are back-filled summaries of what the commit history shows.

---

# Engineering Log

## 2026-07-07 — Text-backed API and 1.1.0 release

Refactored the package down to a **single text-backed API** and bumped the version to **1.1.0**. The build number is now persisted as plain text and read through one small static surface (`ADKOM.BuildNumber.Current`), rather than a binary ScriptableObject — no LFS, no serialization-mode sensitivity. An editor hook auto-increments the number after each successful script compile.

## 2026-07-07 — Trim the test/dev project and give it a distinct Hub name

- Stripped URP, the Input System, and other unnecessary dependencies out of the bundled test project so the dev harness is minimal (removed the sample scene, URP render/volume assets, and the generated input actions — roughly a thousand lines of scaffolding).
- Renamed the dev project directory to `ADKOM_Build_Number_Dev~` so it shows up as a distinct entry in Unity Hub and is excluded from the published package (the trailing `~` hides it from Unity's importer).

## 2026-07-07 — Initial package extraction

First commit: **ADKOM Build Number** established as a UPM package (`com.adkom.buildnumber`), git-URL installable from `adkom-games/ADKOM_Build_Number`. Ships the editor auto-increment behaviour, the runtime read API, a README, CHANGELOG, and an MIT LICENSE.

---

# Design Log

## 2026-07-07 — Plain text over ScriptableObject

Chose to store the build number as a plain-text `TextAsset` rather than a binary `.asset`. Rationale: a text file never needs Git LFS, is unaffected by the project's asset serialization mode, produces clean diffs, and is trivial to read via `Resources.Load<TextAsset>`. This drove the 1.1.0 single-API refactor.
