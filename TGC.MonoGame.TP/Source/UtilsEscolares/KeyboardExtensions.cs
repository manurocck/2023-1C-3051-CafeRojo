using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Utils;

public static class KeyboardExtensions {
    private static float GetAxis(this KeyboardState keyboard, Keys positiveKey, Keys negativeKey) {
        var positive = keyboard.IsKeyDown(positiveKey) ? 1 : 0;
        var negative = keyboard.IsKeyDown(negativeKey) ? 1 : 0;
        return positive - negative;
    }

    /// <summary> Devuelve: 1 si esta acelerando; -1 si está yendo marcha atrás; 0 si no está haciendo nada </summary>
    internal static float AccelerationSense(this KeyboardState keyboard) => keyboard.GetAxis(Keys.W, Keys.S);
    internal static float TurningAxis(this KeyboardState keyboard) => keyboard.GetAxis(Keys.A, Keys.D);
    internal static bool Jumped(this KeyboardState keyboard) => keyboard.IsKeyDown(Keys.Space);
    internal static bool Turbo(this KeyboardState keyboard) => keyboard.IsKeyDown(Keys.O);
    internal static bool MachineGunTrigger(this KeyboardState keyboard) => keyboard.IsKeyDown(Keys.P);
    internal static bool MissileTrigger(this KeyboardState keyboard) => keyboard.IsKeyDown(Keys.E);
}

