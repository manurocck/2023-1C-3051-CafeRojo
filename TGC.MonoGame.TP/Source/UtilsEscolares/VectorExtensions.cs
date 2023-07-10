using Microsoft.Xna.Framework;
using BepuVector3 = System.Numerics.Vector3;

namespace PistonDerby.Utils;

public static class Vector3Extensions {
    internal static BepuVector3 ToBepu(this Vector3 vector3) => new BepuVector3(vector3.X, vector3.Y, vector3.Z);
    internal static Vector3 ToVector3(this BepuVector3 bepuVector3) => new Vector3(bepuVector3.X, bepuVector3.Y, bepuVector3.Z);
    internal static Quaternion ToQuaternion(this Vector3 vector3) => Quaternion.CreateFromYawPitchRoll(vector3.Y, vector3.X, vector3.Z);
    internal static float DotProduct(this Vector2 vector2, Vector2 other) => vector2.X * other.X + vector2.Y * other.Y;
    internal static float DotProduct(this Vector3 vector2, Vector3 other) => vector2.X * other.X + vector2.Y * other.Y + vector2.Z * other.Z;

}