using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Utils;

public static class KeyboardExtensions {
    private static float GetAxis(this KeyboardState keyboard, Keys positiveKey, Keys negativeKey) {
        var positive = keyboard.IsKeyDown(positiveKey) ? 1 : 0;
        var negative = keyboard.IsKeyDown(negativeKey) ? 1 : 0;
        return positive - negative;
    }

    internal static float AccelerationSense(this KeyboardState keyboard) => keyboard.GetAxis(Keys.W, Keys.S);
    internal static float TurningAxis(this KeyboardState keyboard) => keyboard.GetAxis(Keys.A, Keys.D);
    internal static bool Jumped(this KeyboardState keyboard) => keyboard.IsKeyDown(Keys.Escape);
}

