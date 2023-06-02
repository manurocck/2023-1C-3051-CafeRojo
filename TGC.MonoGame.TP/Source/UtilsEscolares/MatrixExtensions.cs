using Microsoft.Xna.Framework;

namespace PistonDerby.Utils;

internal static class MatrixExtensions {
    internal static Matrix TransformToTranslation(this Matrix transform) => Matrix.CreateTranslation(transform.M14, transform.M24, transform.M34);
    internal static Matrix TransformToScale(this Matrix transform) => Matrix.CreateScale(transform.GetScale());
    internal static Matrix TransformToRotation(this Matrix transform) => transform * Matrix.Invert(transform.TransformToScale()) * -transform.TransformToTranslation();

    internal static Matrix DeEscalateTransform(this Matrix transform) => transform * Matrix.Invert(transform.TransformToScale());

    private static Vector3 GetScale(this Matrix transform) => new Vector3(
                    new Vector3(transform.M11, transform.M12, transform.M13).Length(), 
                    new Vector3(transform.M21, transform.M22, transform.M23).Length(), 
                    new Vector3(transform.M31, transform.M32, transform.M33).Length());
}