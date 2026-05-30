using System.Collections.Generic;
using Vintagestory.API.Common;

namespace PatternMining
{
    public class PatternConfigEntry
    {
        public float DurabilityPenalty { get; set; }
        public float MiningSpeedMul { get; set; }
    }

    public class PatternMiningConfig
    {
        public string CycleHotkey { get; set; } = "P";
        public bool CycleHotkeyCtrl { get; set; } = true;
        public bool CycleHotkeyShift { get; set; } = false;
        public bool CycleHotkeyAlt { get; set; } = false;

        public Dictionary<string, PatternConfigEntry> Patterns { get; set; } = new();
    }

    public static class ConfigLoader
    {
        public const string FileName = "patternmining.json";

        public static PatternMiningConfig LoadOrCreate(ICoreAPI api)
        {
            PatternMiningConfig config = null;
            try { config = api.LoadModConfig<PatternMiningConfig>(FileName); }
            catch { /* malformed JSON — fall through to defaults */ }

            if (config == null)
            {
                config = BuildDefault();
                api.StoreModConfig(config, FileName);
            }

            Apply(config);
            return config;
        }

        private static PatternMiningConfig BuildDefault()
        {
            PatternMiningConfig cfg = new();
            foreach (MiningPattern p in PatternRegistry.Patterns)
            {
                cfg.Patterns[p.Key] = new PatternConfigEntry
                {
                    DurabilityPenalty = p.DurabilityPenalty,
                    MiningSpeedMul = p.MiningSpeedMul,
                };
            }
            return cfg;
        }

        private static void Apply(PatternMiningConfig cfg)
        {
            foreach (MiningPattern p in PatternRegistry.Patterns)
            {
                if (cfg.Patterns.TryGetValue(p.Key, out PatternConfigEntry entry))
                {
                    p.DurabilityPenalty = entry.DurabilityPenalty;
                    p.MiningSpeedMul = entry.MiningSpeedMul;
                }
            }
        }
    }
}
