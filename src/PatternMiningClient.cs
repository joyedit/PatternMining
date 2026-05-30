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
        private int selectedIndex;

        public override bool ShouldLoad(EnumAppSide side) => side == EnumAppSide.Client;

        public override void StartClientSide(ICoreClientAPI api)
        {
            capi = api;
            ConfigLoader.LoadOrCreate(api);

            api.Input.RegisterHotKey(
                HotkeyCode,
                "Cycle Mining Pattern",
                GlKeys.P,
                HotkeyType.GUIOrOtherControls,
                ctrlPressed: true);
            api.Input.SetHotKeyHandler(HotkeyCode, OnCycleHotkey);

            channel = api.Network
                .RegisterChannel(NetworkConstants.ChannelName)
                .RegisterMessageType<PatternSelectMessage>();
        }

        private bool OnCycleHotkey(KeyCombination _)
        {
            ItemSlot slot = capi.World.Player?.InventoryManager?.ActiveHotbarSlot;
            if (slot?.Itemstack?.Collectible?.Tool != EnumTool.Pickaxe) return false;

            selectedIndex = (selectedIndex + 1) % PatternRegistry.Count;
            MiningPattern pat = PatternRegistry.Get(selectedIndex);

            ShowPatternInChat(pat);
            channel.SendPacket(new PatternSelectMessage { PatternKey = pat.Key });
            return true;
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
