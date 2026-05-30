# Pattern Mining (formerly Vertical Mining)

**Mod ID:** patternmining &nbsp;•&nbsp; **Version:** 3.1.0 &nbsp;•&nbsp; **Side:** Client and Server
**Hotkeys:** Ctrl+P (cycle patterns, rebindable) &nbsp;•&nbsp; **Commands:** /pm (toggle on/off)

## Description

Pattern Mining expands on the original Vertical Mining concept by giving you five
distinct mining patterns to choose from, all accessible through a simple hotkey.
Whether you're shaft mining, clearing tunnels, or carving out rooms, there's a
pattern to match your task. Larger patterns come with trade-offs — increased tool
wear and slower mining speed — keeping things balanced and grounded in realistic
gameplay. As of 3.0.0, those trade-offs are fully configurable.

## Mining Patterns

| Pattern | Shape | Extra Blocks | Tool Wear | Mining Speed |
|---|---|---|---|---|
| Vertical 1x2 | Center + below | 1 | Normal | 100% |
| Column 1x3 | Center + above + below | 2 | +10% | 85% |
| Horizontal 3x1 | Center + left + right | 2 | +15% | 85% |
| Cross | Center + above + below + left + right | 4 | +20% | 70% |
| Tunnel 3x3 | Full 3x3 face | 8 | +25% | 50% |

## Pattern Diagrams

```
Vertical 1x2    Column 1x3     Horizontal 3x1     Cross          Tunnel 3x3
  _ _ _            _ x _           _ _ _           _ x _           x x x
  _ x _            _ x _           x x x           x x x           x x x
  _ x _            _ x _           _ _ _           _ x _           x x x
```

## How to Use

1. Type `/pm` in chat to toggle Pattern Mining on.
2. Hold a pickaxe and press **Ctrl+P** to cycle through the five mining patterns.
   The selected pattern and its stats are displayed in the chat window.
3. Mine any block — the extra blocks in your selected pattern will break
   automatically, based on which face of the block you hit.
4. Hold **Sneak** while mining to temporarily override the pattern and mine only a
   single block.
5. Type `/pm` again to toggle Pattern Mining off. Mining speed returns to normal.

## Configuration

Tool wear and mining speed for every pattern are configurable. On first run the mod
writes a `patternmining.json` config file with the default values; edit it to tune
the penalties to your taste. Each pattern is keyed by its internal id:

| Pattern | Config key |
|---|---|
| Vertical 1x2 | `vert2` |
| Column 1x3 | `col3` |
| Horizontal 3x1 | `horiz3` |
| Cross | `cross` |
| Tunnel 3x3 | `tunnel` |

For each pattern you can set:

- `DurabilityPenalty` — durability damage multiplier per extra block broken
  (`1.0` = normal, `1.25` = +25%).
- `MiningSpeedMul` — mining speed while the pattern is active (`1.0` = 100%,
  `0.5` = 50%).

If the config file is missing or malformed, the mod falls back to the built-in
defaults shown in the table above.

### Changing the hotkey

The "Cycle Mining Pattern" key can be changed two ways:

- **In-game (recommended):** Settings → Controls → search "Cycle Mining Pattern" and
  rebind it like any other control. This takes priority over the config default.
- **Config file:** edit `patternmining.json`. The defaults are:
  - `CycleHotkey` — the key name (a `GlKeys` value, e.g. `P`, `M`, `Comma`). Default `P`.
  - `CycleHotkeyCtrl` — require Ctrl. Default `true`.
  - `CycleHotkeyShift` — require Shift. Default `false`.
  - `CycleHotkeyAlt` — require Alt. Default `false`.

  If `CycleHotkey` isn't a recognized key name, the mod logs a warning and falls back
  to Ctrl+P.

## Crafting

No crafting required. The mod uses a chat command and hotkey to control.

## Notes

- Defaults to **OFF** for new players.
- Only works with pickaxes.
- A pickaxe must be highlighted in order to cycle through mining patterns with Ctrl+P.
- Extra blocks must be within your tool's mining tier to be broken.
- Horizontal patterns (Horizontal, Cross, Tunnel) are orientation-aware — left and
  right are based on which face of the block you're hitting, so the pattern always
  aligns with the wall in front of you.
- Your pickaxe will not break from pattern mining — if it reaches 1 durability
  remaining, extra blocks stop breaking.
- Mining speed reduction is applied as a player stat modifier while Pattern Mining
  is active. It is removed when you toggle off with `/pm`.

## Disclaimer

This mod was vibe-coded by a hobbyist. While it works great in my testing, I'm not a
professional developer — please back up your worlds in case something goes sideways
and use at your own risk!

I will answer questions and resolve bugs as best I can. I made this mod to use myself
and thought I'd share it with others. Please update where needed.
</content>
</invoke>
