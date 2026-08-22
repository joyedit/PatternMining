# Changelog

## 3.2.2 — 2026-08-22

- Compatibility release for Vintage Story 1.22.7. Rebuilt against the 1.22.7
  assemblies; no code changes.

## 3.2.1 — 2026-07-23

- Compatibility release for Vintage Story 1.22.5. Rebuilt against the 1.22.5
  assemblies; no code changes.

## 3.2.0
- Added a small on-screen indicator: a 3x3 dot grid that lights up the shape of the active pattern while Pattern Mining is enabled, and hides when it's off. Turn it off with `ShowHud: false` in `patternmining.json`.

## 3.1.1
- Fixed a desync where the chat could show one pattern (e.g. Vertical) while the server mined a different one (e.g. Tunnel), causing unexpected extra blocks to break. The server is now the single source of truth for the active pattern: the hotkey asks the server to advance and the client only displays what the server confirms. The active pattern is also re-synced to the client on join.

## 3.1.0
- The "Cycle Mining Pattern" hotkey is now configurable in `patternmining.json` (`CycleHotkey`, `CycleHotkeyCtrl`, `CycleHotkeyShift`, `CycleHotkeyAlt`). It remains rebindable in-game via Settings > Controls, which overrides the config default.

## 3.0.1
- Tested and confirmed compatible with Vintage Story 1.22.3. No gameplay changes.

## 3.0.0
- Tool wear and mining speed per pattern now configurable via `patternmining.json`.
