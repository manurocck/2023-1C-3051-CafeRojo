using Microsoft.Xna.Framework;
using BepuQuaternion = System.Numerics.Quaternion;

namespace TGC.MonoGame.TP.Utils {

    public static class QuaternionExtensions {
        internal static Quaternion ToQuaternion(this BepuQuaternion bepuQuaternion) => new Quaternion(bepuQuaternion.X, bepuQuaternion.Y, bepuQuaternion.Z, bepuQuaternion.W);
        internal static BepuQuaternion ToBepu(this Quaternion quaternion) => new BepuQuaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        internal static Vector3 Forward(this Quaternion rotation) => Vector3.Normalize(new Vector3(
            2 * (rotation.X * rotation.Z + rotation.W * rotation.Y),
            2 * (rotation.Y * rotation.Z - rotation.W * rotation.X),
            1 - 2 * (rotation.X * rotation.X + rotation.Y * rotation.Y)
        ));
        internal static Vector3 Forward(this BepuQuaternion rotation) => rotation.ToQuaternion().Forward();

        internal static Vector3 Up(this Quaternion rotation) => Vector3.Normalize(new Vector3(
            2 * (rotation.X * rotation.Y - rotation.W * rotation.Z),
            1 - 2 * (rotation.X * rotation.X + rotation.Z * rotation.Z),
            2 * (rotation.Y * rotation.Z + rotation.W * rotation.X)
        ));
        internal static Vector3 Up(this BepuQuaternion rotation) => rotation.ToQuaternion().Up();

        internal static Vector3 Left(this Quaternion rotation) => Vector3.Normalize(new Vector3(
            1 - 2 * (rotation.Y * rotation.Y + rotation.Z * rotation.Z),
            2 * (rotation.X * rotation.Y + rotation.W * rotation.Z),
            2 * (rotation.X * rotation.Z - rotation.W * rotation.Y)
        ));
        internal static Vector3 Left(this BepuQuaternion rotation) => rotation.ToQuaternion().Left();
    }
} 