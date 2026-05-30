using Vintagestory.API.Common;

namespace PatternMining
{
    // Per-player state is stored in the player entity's WatchedAttributes,
    // which are auto-persisted with the player and survive disconnects.
    public static class PlayerState
    {
        private const string EnabledKey = "patternmining:enabled";
        private const string PatternKey = "patternmining:pattern";

        public static bool IsEnabled(IPlayer player)
        {
            return player?.Entity?.WatchedAttributes?.GetBool(EnabledKey, false) ?? false;
        }

        public static void SetEnabled(IPlayer player, bool enabled)
        {
            player?.Entity?.WatchedAttributes?.SetBool(EnabledKey, enabled);
            player?.Entity?.WatchedAttributes?.MarkPathDirty(EnabledKey);
        }

        public static int GetPatternIndex(IPlayer player)
        {
            return player?.Entity?.WatchedAttributes?.GetInt(PatternKey, 0) ?? 0;
        }

        public static void SetPatternIndex(IPlayer player, int index)
        {
            player?.Entity?.WatchedAttributes?.SetInt(PatternKey, index);
            player?.Entity?.WatchedAttributes?.MarkPathDirty(PatternKey);
        }
    }
}
