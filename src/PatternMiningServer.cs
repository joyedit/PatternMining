using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PatternMining
{
    public class PatternMiningServer : ModSystem
    {
        private const string SpeedStatKey = "patternmining";
        private const string SpeedStatName = "miningSpeedMul";

        private ICoreServerAPI sapi;

        public override bool ShouldLoad(EnumAppSide side) => side == EnumAppSide.Server;

        public override void StartServerSide(ICoreServerAPI api)
        {
            sapi = api;
            ConfigLoader.LoadOrCreate(api);

            api.Event.BreakBlock += OnBreakBlock;
            api.Event.PlayerNowPlaying += OnPlayerNowPlaying;

            api.ChatCommands.Create("pm")
                .WithDescription("Toggle Pattern Mining on/off. Use Ctrl+P to cycle patterns.")
                .RequiresPrivilege(Privilege.chat)
                .HandleWith(OnCmdToggle);

            api.Network
                .RegisterChannel(NetworkConstants.ChannelName)
                .RegisterMessageType<PatternSelectMessage>()
                .SetMessageHandler<PatternSelectMessage>(OnPatternSelected);
        }

        private void OnPlayerNowPlaying(IServerPlayer player)
        {
            if (PlayerState.IsEnabled(player))
            {
                ApplySpeedModifier(player, PatternRegistry.Get(PlayerState.GetPatternIndex(player)));
            }
        }

        private TextCommandResult OnCmdToggle(TextCommandCallingArgs args)
        {
            IServerPlayer player = args.Caller.Player as IServerPlayer;
            if (player == null) return TextCommandResult.Error("Player only command.");

            bool newState = !PlayerState.IsEnabled(player);
            PlayerState.SetEnabled(player, newState);

            if (!newState)
            {
                RemoveSpeedModifier(player);
                return TextCommandResult.Success("Pattern Mining: OFF");
            }

            MiningPattern pat = PatternRegistry.Get(PlayerState.GetPatternIndex(player));
            ApplySpeedModifier(player, pat);
            return TextCommandResult.Success(
                $"Pattern Mining: ON — Current: {pat.Name}\n" +
                "Hold a pickaxe and press Ctrl+P to cycle patterns.\n" +
                "Sneak-mine to override and break a single block.");
        }

        private void OnPatternSelected(IServerPlayer player, PatternSelectMessage msg)
        {
            if (player == null || string.IsNullOrEmpty(msg?.PatternKey)) return;
            if (!PatternRegistry.TryFindIndex(msg.PatternKey, out int idx)) return;

            PlayerState.SetPatternIndex(player, idx);

            if (PlayerState.IsEnabled(player))
            {
                ApplySpeedModifier(player, PatternRegistry.Get(idx));
            }
        }

        private void ApplySpeedModifier(IServerPlayer player, MiningPattern pattern)
        {
            // Stat is additive over base 1.0: -0.3 means 70% speed.
            player?.Entity?.Stats.Set(SpeedStatName, SpeedStatKey, pattern.MiningSpeedMul - 1f, persistent: false);
        }

        private void RemoveSpeedModifier(IServerPlayer player)
        {
            player?.Entity?.Stats.Remove(SpeedStatName, SpeedStatKey);
        }

        private void OnBreakBlock(IServerPlayer player, BlockSelection blockSel,
                                  ref float dropQuantityMultiplier, ref EnumHandling handling)
        {
            if (!ShouldHandleBreak(player, blockSel)) return;

            ItemSlot hotbarSlot = player.InventoryManager.ActiveHotbarSlot;
            ItemStack heldItem = hotbarSlot?.Itemstack;
            if (heldItem?.Collectible?.Tool != EnumTool.Pickaxe) return;

            MiningPattern pattern = PatternRegistry.Get(PlayerState.GetPatternIndex(player));
            if (pattern.ExtraBlocks == 0) return;

            int broken = BreakExtraBlocks(player, blockSel, pattern, heldItem);
            if (broken == 0) return;

            int extraDamage = (int)Math.Ceiling(broken * pattern.DurabilityPenalty);
            heldItem.Collectible.DamageItem(player.Entity.World, player.Entity, hotbarSlot, extraDamage);
            hotbarSlot.MarkDirty();
        }

        private static bool ShouldHandleBreak(IServerPlayer player, BlockSelection blockSel)
        {
            if (player == null) return false;
            if (player.WorldData.CurrentGameMode == EnumGameMode.Spectator) return false;
            if (player.CurrentBlockSelection == null) return false;
            if (!player.CurrentBlockSelection.Position.Equals(blockSel.Position)) return false;
            if (!PlayerState.IsEnabled(player)) return false;
            if (player.Entity.Controls.Sneak) return false;
            return true;
        }

        private static int BreakExtraBlocks(IServerPlayer player, BlockSelection blockSel,
                                            MiningPattern pattern, ItemStack heldItem)
        {
            IWorldAccessor world = player.Entity.World;
            int toolTier = heldItem.Collectible.ToolTier;
            int broken = 0;

            foreach (Vec3i local in pattern.Offsets)
            {
                if (heldItem.Collectible.GetRemainingDurability(heldItem) <= 1) break;

                Vec3i worldOffset = OffsetMapper.ToWorld(local, blockSel.Face);
                BlockPos extraPos = blockSel.Position.AddCopy(worldOffset.X, worldOffset.Y, worldOffset.Z);
                Block extraBlock = world.BlockAccessor.GetBlock(extraPos);

                if (extraBlock.Id == 0) continue;
                if (extraBlock.BlockMaterial == EnumBlockMaterial.Air) continue;
                if (toolTier < extraBlock.RequiredMiningTier) continue;

                world.BlockAccessor.BreakBlock(extraPos, player, dropQuantityMultiplier: 1f);
                broken++;
            }

            return broken;
        }
    }
}
