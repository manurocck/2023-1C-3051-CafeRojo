
using System;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Utils;

using PistonDerby.Drawers;
using PistonDerby.Elementos;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Autos.PowerUps;
internal class PowerUp : ElementoDinamico {
    private const float ANGULAR_SPEED = 0.5f * PistonDerby.S_METRO;
    internal override float Mass() => 1f; // Es indistinto
    // internal override float Scale() => 1.4f; // Scale vieja
    internal override float Scale() => PistonDerby.S_METRO * 0.15f;
    internal override IDrawer Drawer => this.StateDrawer;
    internal override Model Model => PistonDerby.GameContent.M_AutoEnemigo; // Es de antes
    private IDrawer StateDrawer = new TextureDrawer(PistonDerby.GameContent.T_MarmolNegro);
    public bool Dirty = false;
    private float StateTimer = 0;
    private float Clock = 0;

    internal PowerUp(float posX, float posY, float posZ, Vector3 rotacion) 
    {
        var PosicionInicial = new Vector3(posX, posY, posZ) * PistonDerby.S_METRO;
        PosicionInicial.Y = PistonDerby.S_METRO * 0.2f; // Hard-code de la altura

        Box box = new Box(Scale() * 1,Scale() * 1,Scale()* 1);
        Shape = PistonDerby.Simulation.LoadShape<Box>(box);
        this.AddToSimulation(PosicionInicial, Quaternion.Identity);
        // this.Body().Pose.Orientation = Quaternion.CreateFromYawPitchRoll(0,MathHelper.PiOver2,0).ToBepu();
        this.Body().BecomeKinematic();
    }
    internal override void Update(float dTime, KeyboardState _)
    {
        StateTimer += dTime;
        Clock += dTime;
        Body().Velocity.Angular = (Vector3.UnitY*ANGULAR_SPEED*dTime).ToBepu();
        Body().Velocity.Linear  = (Vector3.UnitY *(ANGULAR_SPEED/2)*(-(Clock%2-1))).ToBepu();

        if(StateTimer > 2){
            if(Dirty) {
                StateTimer = 0;
            }
            Dirty = false;
        }
    }

    internal override bool OnCollision(Elemento other)
    {
        if(other is Auto _){
            if(!Dirty) Dirty = !Dirty;
            return false;
        }

        
        return true;
    }

    internal override void Draw()
    {   
        if(Dirty) return;
        Effect shader = PistonDerby.GameContent.E_TextureMirror;
        Matrix world = Matrix.CreateFromQuaternion(Rotation()) * Matrix.CreateScale(Scale()) *  Matrix.CreateTranslation(Position()) ;

        shader.Parameters["Texture"].SetValue(PistonDerby.GameContent.T_MysteryBox);
        shader.Parameters["World"].SetValue(world); // chequear despues de la simu
        shader.Parameters["TilesBroad"].SetValue(1); // chequear despues de la simu
        shader.Parameters["TilesWide"].SetValue(1); // chequear despues de la simu

        PistonDerby.GameContent.G_Cubo.Draw(shader);
    }
}
