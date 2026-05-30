# Changelog

## 3.1.1
- Fixed a desync where the chat could show one pattern (e.g. Vertical) while the server mined a different one (e.g. Tunnel), causing unexpected extra blocks to break. The server is now the single source of truth for the active pattern: the hotkey asks the server to advance and the client only displays what the server confirms. The active pattern is also re-synced to the client on join.

## 3.1.0
- The "Cycle Mining Pattern" hotkey is now configurable in `patternmining.json` (`CycleHotkey`, `CycleHotkeyCtrl`, `CycleHotkeyShift`, `CycleHotkeyAlt`). It remains rebindable in-game via Settings > Controls, which overrides the config default.

## 3.0.1
- Tested and confirmed compatible with Vintage Story 1.22.3. No gameplay changes.

## 3.0.0
- Tool wear and mining speed per pattern now configurable via `patternmining.json`.
