using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.HUD;

public class PistonDerbyHUD 
{
    internal Vector3 FollowedPosition;
    internal Matrix HUDView;
    internal Matrix QuadWorld;
    internal (int Width, int Height) Window;
    private Matrix AjusteQuad => Matrix.CreateTranslation(new Vector3(-0.5f,0,-0.5f)) * Matrix.CreateRotationX(MathHelper.PiOver2) ;
    private Matrix AjusteFinal => Matrix.CreateTranslation(-Vector3.UnitZ * PistonDerby.S_METRO);
    
    //      Debería haber un Drawable por elementoHUD así se dibujan todos
    //      Los Effects de los Drawables no se pueden usar para los HUDS porque modifican la View.
    //
    // private List<IDrawableHUD> ElementosHUD = new List<IDrawable>();

    public PistonDerbyHUD(int width, int height)
    {
        Window.Width = width;
        Window.Height = height;
    }

    public void Update(Matrix followedWorld)
    {   
        FollowedPosition = followedWorld.Translation;
        HUDView = Matrix.CreateLookAt(FollowedPosition, FollowedPosition - Vector3.UnitZ, Vector3.UnitY);

        this.DrawBar(0, -6);
    }
    public void Draw()
    {
        // foreach(IDrawableHUD elem in ElementosHUD) elem.Draw(QuadWorld);

        // BarDrawableHUD
        //
        float vida = 0.75f;
        Effect efecto = PistonDerby.GameContent.H_BarraEffect;    // debería ser EffectHUB
        efecto.Parameters["View"].SetValue(HUDView);            // al loadContent
        efecto.Parameters["World"].SetValue(QuadWorld);
        efecto.Parameters["Texture"]?.SetValue(PistonDerby.GameContent.T_Ladrillos);
        efecto.Parameters["PorcentajeBarra"]?.SetValue(vida);
       
        PistonDerby.GameContent.G_Quad.Draw(efecto);
    }

    public void DrawBar(float posX, float posY)
    {
        float anchoQuad = (float)Window.Width  *0.0125f;
        float altoQuad  = (float)Window.Height *0.001f;
        Matrix movimientoHorizontal = Matrix.CreateTranslation(Vector3.UnitX*posX);
        Matrix movimientoVertical = Matrix.CreateTranslation(Vector3.UnitY*posY);

        QuadWorld =  AjusteQuad *      // levanta el quad 
                     Matrix.CreateScale(anchoQuad,altoQuad,0) *         // ajusta pq se ve muy grande
                     movimientoVertical * movimientoHorizontal *        // ubicación de hud
                     Matrix.CreateTranslation(FollowedPosition) *    
                     AjusteFinal;     // un poquito más para atrás
    }
}