using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.HUD.Elements;

public class TurboBar : IBarHUD {
    internal override Effect Efecto() => PistonDerby.GameContent.HE_TurboHUD;
    internal override (float X, float Y) Ubicacion() => (0, -7);

    public TurboBar(float width, float heigth) : base(width, heigth){}
}