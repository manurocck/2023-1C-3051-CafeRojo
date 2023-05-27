

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.HUD.Elements;
public abstract class IAmmoHUD
{
    internal static Matrix AjusteQuad() => Matrix.CreateTranslation(new Vector3(-0.5f,0,-0.5f)) //centrar 
                                         * Matrix.CreateRotationX(MathHelper.PiOver2) ;         //levantar
    internal static Matrix AjusteFinal() => Matrix.CreateTranslation(-Vector3.UnitZ * 250f);    //standard
    internal abstract (float Ancho, float Alto) QuadSize();
    internal abstract (float X, float Y) Ubicacion();
    internal abstract Texture2D Texture();
    internal Effect Efecto() => PistonDerby.GameContent.HE_TextureHUD;
    internal void Initialize() => this.Efecto().Parameters["Texture"].SetValue(this.Texture());
}
