using Microsoft.Xna.Framework;
using BepuVector3 = System.Numerics.Vector3;

namespace PistonDerby.Utils;

public static class Vector3Extensions {
    internal static BepuVector3 ToBepu(this Vector3 vector3) => new BepuVector3(vector3.X, vector3.Y, vector3.Z);
    internal static Vector3 ToVector3(this BepuVector3 bepuVector3) => new Vector3(bepuVector3.X, bepuVector3.Y, bepuVector3.Z);
    internal static Vector3 XZFoward(this Vector3 v) => (v.X==0 && v.Z ==0)? 
                                                        Vector3.Zero :
                                                        Vector3.Normalize(new Vector3(v.X,0,v.Z));
}