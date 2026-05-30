using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace PatternMining
{
    public class PatternMiningClient : ModSystem
    {
        private const string HotkeyCode = "patternminingcycle";

        private ICoreClientAPI capi;
        private IClientNetworkChannel channel;

        public override bool ShouldLoad(EnumAppSide side) => side == EnumAppSide.Client;

        public override void StartClientSide(ICoreClientAPI api)
        {
            capi = api;
            PatternMiningConfig config = ConfigLoader.LoadOrCreate(api);

            api.Input.RegisterHotKey(
                HotkeyCode,
                "Cycle Mining Pattern",
                ParseKey(api, config.CycleHotkey),
                HotkeyType.GUIOrOtherControls,
                altPressed: config.CycleHotkeyAlt,
                ctrlPressed: config.CycleHotkeyCtrl,
                shiftPressed: config.CycleHotkeyShift);
            api.Input.SetHotKeyHandler(HotkeyCode, OnCycleHotkey);

            channel = api.Network
                .RegisterChannel(NetworkConstants.ChannelName)
                .RegisterMessageType<CyclePatternRequest>()
                .RegisterMessageType<PatternSelectMessage>()
                .SetMessageHandler<PatternSelectMessage>(OnPatternFromServer);
        }

        private static GlKeys ParseKey(ICoreClientAPI api, string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && Enum.TryParse(name, ignoreCase: true, out GlKeys key))
                return key;

            api.Logger.Warning("[patternmining] Unknown CycleHotkey '{0}' in patternmining.json; defaulting to P. Use a GlKeys name (e.g. P, M, Comma).", name);
            return GlKeys.P;
        }

        private bool OnCycleHotkey(KeyCombination _)
        {
            ItemSlot slot = capi.World.Player?.InventoryManager?.ActiveHotbarSlot;
            if (slot?.Itemstack?.Collectible?.Tool != EnumTool.Pickaxe) return false;

            // The server owns the current pattern. Ask it to advance; it replies with
            // the new pattern, which we then display. This keeps the chat display and
            // the server's mining behavior from ever drifting out of sync.
            channel.SendPacket(new CyclePatternRequest());
            return true;
        }

        private void OnPatternFromServer(PatternSelectMessage msg)
        {
            if (msg == null || !PatternRegistry.TryFindIndex(msg.PatternKey, out int idx)) return;
            ShowPatternInChat(PatternRegistry.Get(idx));
        }

        private void ShowPatternInChat(MiningPattern pat)
        {
            StringBuilder sb = new();
            sb.AppendLine("--- Mining Pattern ---");
            sb.AppendLine(pat.Name);
            foreach (string line in pat.Diagram) sb.AppendLine(line);

            if (pat.DurabilityPenalty > 1.0f)
                sb.AppendLine($"Tool wear: +{(int)((pat.DurabilityPenalty - 1f) * 100)}% per extra block");
            else
                sb.AppendLine("Tool wear: normal");

            if (pat.MiningSpeedMul < 1.0f)
                sb.AppendLine($"Mining speed: {(int)(pat.MiningSpeedMul * 100)}%");
            else
                sb.AppendLine("Mining speed: normal");

            sb.Append("----------------------");
            capi.ShowChatMessage(sb.ToString());
        }
    }
}
