using System;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Utils;

using PistonDerby.Drawers;
using PistonDerby.Elementos;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Autos.PowerUps;
internal class Misil : ElementoDinamico {
    private const float SPEED = 0.5f * PistonDerby.S_METRO;
    internal override float Mass() => 1f;
    internal override float Scale() => PistonDerby.S_METRO * 0.005f;
    internal override IDrawer Drawer => this.StateDrawer;
    internal override Model Model => PistonDerby.GameContent.M_Misil;
    private IDrawer StateDrawer = new TextureDrawer(PistonDerby.GameContent.T_MisilLanzado);
    private float Clock = 0;
    private bool impacto = false;

    private Quaternion Rotation;

    internal Misil(Vector3 posicionInicial, Quaternion rotation){
        Rotation = rotation;
        Box box = new Box(Scale() * 10,Scale() * 10,Scale()* 50);
        Shape = PistonDerby.Simulation.LoadShape<Box>(box);

        this.AddToSimulation(posicionInicial + new Vector3(0, 20f, 0), Rotation);
        this.Body().BecomeKinematic();
    }

    internal override void Update(float dTime, KeyboardState _)
    {
        Vector3 horizontalImpulse = Rotation.Forward() * 30f;
        this.Body().Velocity.Linear = (this.Rotation().Forward() * 200).ToBepu();
        this.Body().Velocity.Angular = (Vector3.UnitY *(0.5f)*(-(Clock%2))).ToBepu();
        Clock += dTime;
    }

    internal override bool OnCollision(Elemento other)
    {

        impacto = true;
        this.Body().Velocity = Vector3.Zero.ToBepu();
        this.Body().SetShape(new TypedIndex()); //Esto habria que cambiarlo por una eliminacion de la instancia

        return true;
    }
    internal override void Draw()
    { 
        if(impacto) return;
        else base.Draw();
    }

}