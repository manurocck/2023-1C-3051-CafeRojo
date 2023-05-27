using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.HUD.Elements;
public abstract class IBarHUD
{
    internal static Matrix AjusteQuad() => Matrix.CreateTranslation(new Vector3(-0.5f,0,-0.5f)) // centrar quad 
                                            * Matrix.CreateRotationX(MathHelper.PiOver2) ;         // levantar quad
    internal static Matrix AjusteFinal() => Matrix.CreateTranslation(-Vector3.UnitZ * 250f);    // ubicación pantalla HUD (puede ser usado como un z-index)
    internal abstract Effect Efecto();
    private (float Ancho, float Alto) QuadSize() => (Window.Width*0.0125f,Window.Heigth*0.001f);
    internal abstract (float X, float Y) Ubicacion();
    private (float Width, float Heigth) Window;
    
    public IBarHUD(float width, float heigth){
        Window.Width    = width ;
        Window.Heigth   = heigth;
    }
    
    public void Draw() => PistonDerby.GameContent.G_Quad.Draw(this.Efecto());

    public void Update(Vector3 followedPosition, float porcentajeBarra){
        Matrix movimientoHorizontal = Matrix.CreateTranslation(Vector3.UnitX*Ubicacion().X);
        Matrix movimientoVertical = Matrix.CreateTranslation(Vector3.UnitY*Ubicacion().Y);

        Matrix QuadWorld =  AjusteQuad() *                                      // levanta el quad 
                            Matrix.CreateScale(QuadSize().Ancho,QuadSize().Alto,0) *  // tamaño barra
                            movimientoVertical * movimientoHorizontal *         // ubicación de hud
                            Matrix.CreateTranslation(followedPosition) *        // seguimiento de cámara
                            AjusteFinal();                                      // un poquito más para atrás

        this.Efecto().Parameters["World"]?.SetValue(QuadWorld);
        this.Efecto().Parameters["PorcentajeBarra"]?.SetValue(porcentajeBarra);
    }
}
