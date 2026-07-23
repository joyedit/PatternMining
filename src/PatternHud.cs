using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace PatternMining
{
    // A tiny 3x3 dot matrix shown while Pattern Mining is enabled. The lit cells
    // ('•') trace the shape of the active pattern; unlit cells ('·') form the grid
    // for reference. Driven entirely by the client from synced player state — see
    // PatternMiningClient — so it never needs its own network traffic.
    public class PatternHud : HudElement
    {
        private const char Active = '•';   // • bullet
        private const char Inactive = '·'; // · middle dot

        // Placement is computed from the live frame size (in unscaled GUI units)
        // rather than dialog alignment, which a bare HUD element doesn't honor.
        // The hotbar is centered along the bottom; the off-hand slot sits at its
        // left end, so we offset left of screen-center and ride just above the
        // bottom edge. Tweak these two if it doesn't line up on your resolution.
        private const double OffsetLeftOfCenter = 420; // grid's right edge, left of center
        private const double OffsetFromBottom = 10;    // grid's bottom, up from screen bottom

        public PatternHud(ICoreClientAPI capi) : base(capi) { }

        // Visibility is controlled by mod state, not a keybind.
        public override string ToggleKeyCombinationCode => null;

        public void ShowPattern(MiningPattern pattern)
        {
            Compose(pattern);
            if (!IsOpened()) TryOpen();
        }

        public void HideHud()
        {
            if (IsOpened()) TryClose();
        }

        private void Compose(MiningPattern pattern)
        {
            string[] rows = BuildDotRows(pattern);

            const double rowHeight = 20;
            const double width = 52;
            double height = rows.Length * rowHeight;

            CairoFont font = CairoFont.WhiteSmallText();

            // Convert the pixel frame size to unscaled GUI units; ElementBounds
            // multiplies these back up by the GUI scale at render time.
            float scale = RuntimeEnv.GUIScale;
            double screenWidth = capi.Render.FrameWidth / scale;
            double screenHeight = capi.Render.FrameHeight / scale;

            double x = screenWidth / 2.0 - OffsetLeftOfCenter - width;
            double y = screenHeight - OffsetFromBottom - height;

            ElementBounds parent = ElementBounds.Fixed(x, y, width, height);

            GuiComposer composer = capi.Gui.CreateCompo("patternmininghud", parent);
            for (int i = 0; i < rows.Length; i++)
            {
                ElementBounds line = ElementBounds.Fixed(0, i * rowHeight, width, rowHeight);
                composer.AddStaticText(rows[i], font, line, "row" + i);
            }

            SingleComposer = composer.Compose();
        }

        // Turns a pattern's front-view diagram ("_ x _" rows) into spaced dot rows
        // ("· • ·") so the HUD reads as the same shape shown when cycling.
        private static string[] BuildDotRows(MiningPattern pattern)
        {
            var rows = new string[pattern.Diagram.Count];
            for (int i = 0; i < pattern.Diagram.Count; i++)
            {
                var sb = new StringBuilder();
                foreach (char ch in pattern.Diagram[i])
                {
                    if (ch == 'x') { if (sb.Length > 0) sb.Append(' '); sb.Append(Active); }
                    else if (ch == '_') { if (sb.Length > 0) sb.Append(' '); sb.Append(Inactive); }
                }
                rows[i] = sb.ToString();
            }
            return rows;
        }
    }
}
