using Microsoft.Xna.Framework;
using PistonDerby.HUD.Elements;

namespace PistonDerby.HUD;

public class CarHUD 
{
    internal Vector3 FollowedPosition;
    internal BulletAmmo BulletAmmo;
    internal HealthBar HealthBar;
    internal TurboBar TurboBar;
    protected Matrix HUDView;
    internal (int Width, int Heigth) Window;
    
    //      Debería haber un Drawable por elementoHUD así se dibujan todos
    //      Los Effects de los Drawables no se pueden usar para los HUDS porque modifican la View.
    //
    // private List<IDrawableHUD> ElementosHUD = new List<IDrawable>();

    public CarHUD(int width, int heigth)
    {
        Window.Width = width; // 1280
        Window.Heigth = heigth; // 720

        HealthBar = new HealthBar(width, heigth);
        TurboBar  = new TurboBar(width, heigth);
        BulletAmmo = new BulletAmmo(width, heigth);
    }

    public void Update(Matrix followedWorld, float vida, float turbo)
    {   
        FollowedPosition = followedWorld.Translation;
        HUDView = Matrix.CreateLookAt(FollowedPosition, FollowedPosition - Vector3.UnitZ, Vector3.UnitY);

        HealthBar.Update(FollowedPosition, vida);
        TurboBar.Update(FollowedPosition, turbo);
        BulletAmmo.Update(FollowedPosition);
    }
    public void Draw()
    {
        PistonDerby.GameContent.HE_HealthHUD.Parameters["View"].SetValue(HUDView);            // al loadContent
        PistonDerby.GameContent.HE_TurboHUD.Parameters["View"].SetValue(HUDView);            // al loadContent
        PistonDerby.GameContent.E_TextureShader.Parameters["View"].SetValue(HUDView);            // al loadContent
        PistonDerby.GameContent.HE_TextureHUD.Parameters["View"].SetValue(HUDView);            // al loadContent
       
        HealthBar.Draw();
        TurboBar.Draw();
        BulletAmmo.Draw();
    }
}