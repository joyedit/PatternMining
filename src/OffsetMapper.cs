using Vintagestory.API.MathTools;

namespace PatternMining
{
    // Maps a pattern's face-local (h, v) offset into a world (x, y, z) offset
    // based on which face of the block was hit.
    //
    //   h = -1 left  / +1 right  on the face grid
    //   v = -1 down  / +1 up     on the face grid
    //
    // For the four side faces, v is world Y. For UP/DOWN, the pattern lies
    // flat on the horizontal plane and v becomes a Z axis component.
    public static class OffsetMapper
    {
        public static Vec3i ToWorld(Vec3i local, BlockFacing face)
        {
            int h = local.X;
            int v = local.Y;

            if (face == BlockFacing.NORTH) return new Vec3i(-h, v, 0);
            if (face == BlockFacing.SOUTH) return new Vec3i( h, v, 0);
            if (face == BlockFacing.EAST)  return new Vec3i( 0, v, h);
            if (face == BlockFacing.WEST)  return new Vec3i( 0, v, -h);
            if (face == BlockFacing.UP)    return new Vec3i( h, 0, v);
            if (face == BlockFacing.DOWN)  return new Vec3i( h, 0, -v);

            return new Vec3i(h, v, 0);
        }
    }
}
