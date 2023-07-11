using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Autos;
using PistonDerby.Geometries;
using PistonDerby.Utils;

namespace PistonDerby.Drawers;
internal class MachineGunDrawer : IDrawer
{
    private CylinderPrimitive BulletCylinder = PistonDerby.GameContent.G_Cilindro;
    internal float WheelRotation() => Auto.WheelRotation;
    internal Auto Auto;

    internal MachineGunDrawer(Auto auto){
        Auto = auto;  
    }
    void IDrawer.Draw(Model _, Matrix _2){   
        DrawBulletCyllinder(Auto.Position(), Auto.Rotation());
    }

    private void DrawBulletCyllinder(Vector3 position, Quaternion rotation){
        Effect bulletEffect = this.Auto.isShooting()?
                             PistonDerby.GameContent.E_BulletShader : 
                             PistonDerby.GameContent.E_BasicShader;

        Matrix world = 
                         Matrix.CreateScale(1f, 200f, 1f)
                        * Matrix.CreateRotationX(MathHelper.PiOver2 + MathHelper.PiOver4 * 0.20f)
                        * Matrix.CreateFromQuaternion(rotation)
                        * Matrix.CreateTranslation(rotation.Forward()*80f)
                        * Matrix.CreateTranslation(Vector3.UnitY*25f)
                        * Matrix.CreateTranslation(position);
        

        bulletEffect.Parameters["World"]?.SetValue(world);
        bulletEffect.Parameters["DiffuseColor"]?.SetValue(Color.DarkRed.ToVector3());
        BulletCylinder.Draw(bulletEffect);
    }
    
}



