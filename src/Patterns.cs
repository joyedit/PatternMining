using System.Collections.Generic;
using Vintagestory.API.MathTools;

namespace PatternMining
{
    // Patterns are defined as viewed from the front (the face being mined).
    // The grid is 3x3, with the center being the block the player hits.
    // Offsets use (h, v, 0) where h = -1 left / +1 right on the face,
    // v = -1 down / +1 up on the face. OffsetMapper turns these into
    // world (x, y, z) based on which face was hit.

    public sealed class MiningPattern
    {
        public string Name { get; }
        public string Key { get; }
        public IReadOnlyList<Vec3i> Offsets { get; }
        public IReadOnlyList<string> Diagram { get; }
        public float DurabilityPenalty { get; set; }
        public float MiningSpeedMul { get; set; }

        public int ExtraBlocks => Offsets.Count;

        public MiningPattern(string name, string key, Vec3i[] offsets, string[] diagram,
                             float durabilityPenalty, float miningSpeedMul)
        {
            Name = name;
            Key = key;
            Offsets = offsets;
            Diagram = diagram;
            DurabilityPenalty = durabilityPenalty;
            MiningSpeedMul = miningSpeedMul;
        }
    }

    public static class PatternRegistry
    {
        public static readonly IReadOnlyList<MiningPattern> Patterns = new MiningPattern[]
        {
            new MiningPattern(
                "Vertical 1x2", "vert2",
                new[] { new Vec3i(0, -1, 0) },
                new[] {
                    "  _ _ _",
                    "  _ x _",
                    "  _ x _",
                },
                durabilityPenalty: 1.00f, miningSpeedMul: 1.00f),

            new MiningPattern(
                "Column 1x3", "col3",
                new[] { new Vec3i(0, 1, 0), new Vec3i(0, -1, 0) },
                new[] {
                    "  _ x _",
                    "  _ x _",
                    "  _ x _",
                },
                durabilityPenalty: 1.10f, miningSpeedMul: 0.85f),

            new MiningPattern(
                "Horizontal 3x1", "horiz3",
                new[] { new Vec3i(-1, 0, 0), new Vec3i(1, 0, 0) },
                new[] {
                    "  _ _ _",
                    "  x x x",
                    "  _ _ _",
                },
                durabilityPenalty: 1.15f, miningSpeedMul: 0.85f),

            new MiningPattern(
                "Cross", "cross",
                new[] {
                    new Vec3i(0, 1, 0), new Vec3i(0, -1, 0),
                    new Vec3i(-1, 0, 0), new Vec3i(1, 0, 0),
                },
                new[] {
                    "  _ x _",
                    "  x x x",
                    "  _ x _",
                },
                durabilityPenalty: 1.20f, miningSpeedMul: 0.70f),

            new MiningPattern(
                "Tunnel 3x3", "tunnel",
                new[] {
                    new Vec3i(-1, 1, 0), new Vec3i(0, 1, 0), new Vec3i(1, 1, 0),
                    new Vec3i(-1, 0, 0),                     new Vec3i(1, 0, 0),
                    new Vec3i(-1, -1, 0), new Vec3i(0, -1, 0), new Vec3i(1, -1, 0),
                },
                new[] {
                    "  x x x",
                    "  x x x",
                    "  x x x",
                },
                durabilityPenalty: 1.25f, miningSpeedMul: 0.50f),
        };

        public static int Count => Patterns.Count;

        public static MiningPattern Get(int index)
        {
            if (index < 0 || index >= Patterns.Count) return Patterns[0];
            return Patterns[index];
        }

        public static bool TryFindIndex(string key, out int index)
        {
            for (int i = 0; i < Patterns.Count; i++)
            {
                if (Patterns[i].Key == key) { index = i; return true; }
            }
            index = 0;
            return false;
        }
    }
}
