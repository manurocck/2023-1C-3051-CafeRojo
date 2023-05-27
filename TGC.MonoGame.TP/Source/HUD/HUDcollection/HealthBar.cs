using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.HUD.Elements;

public class HealthBar : IBarHUD {
    internal override Effect Efecto() => PistonDerby.GameContent.HE_HealthHUD;
    internal override (float X, float Y) Ubicacion() => (0, -6);

    public HealthBar(float width, float heigth) : base(width, heigth){}

}