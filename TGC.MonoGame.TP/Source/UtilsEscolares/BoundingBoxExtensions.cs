using Microsoft.Xna.Framework;
using BepuBoundingBox = BepuUtilities.BoundingBox;

namespace PistonDerby.Utils;

internal static class BoundingBoxExtensions{
    internal static BepuBoundingBox ToBepu(this BoundingBox bb) => new BepuBoundingBox(bb.Min.ToBepu(), bb.Max.ToBepu());
    internal static BoundingBox ToBoundingBox(this BepuBoundingBox bb) => new BoundingBox(bb.Min, bb.Max);
}