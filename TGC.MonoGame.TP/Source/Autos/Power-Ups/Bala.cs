using System;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Utils;

using PistonDerby.Drawers;
using PistonDerby.Elementos;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Autos.PowerUps;
internal class Bala : ElementoDinamico {
    private const float SPEED = 0.5f * PistonDerby.S_METRO;
    internal override float Mass() => 0.1f;
    internal override float Scale() => PistonDerby.S_METRO * 0.002f;
    internal override IDrawer Drawer => this.StateDrawer;
    internal override Model Model => PistonDerby.GameContent.M_Lego;
    private IDrawer StateDrawer = new ColorDrawer(Color.DarkRed);
    private float Clock = 0;
    private bool impacto = false;

    private Vector3 Forward;

    internal Bala(Vector3 posicionInicial, Vector3 forward){
        this.Forward = forward;
        Box box = new Box(Scale() * 1,Scale() * 1,Scale()* 1);
        Shape = PistonDerby.Simulation.LoadShape<Box>(box);
        this.AddToSimulation(posicionInicial + new Vector3(0, 10f, 0), Quaternion.Identity);
    }

    internal override void Update(float dTime, KeyboardState _)
    {
        if(!impacto)this.Body().Velocity = (Forward * 200000 * dTime).ToBepu();
    }

    internal override bool OnCollision(Elemento other)
    {
        impacto = true;
        this.Body().Velocity = Vector3.Zero.ToBepu();
        this.Body().MotionState.Velocity.Angular = Vector3.Zero.ToBepu();
        this.Body().MotionState.Velocity.Linear = Vector3.Zero.ToBepu();
        this.Body().SetShape(new TypedIndex()); //Esto habria que cambiarlo por una eliminacion de la instancia
        this.Body().BecomeKinematic();

        return true;
    }
    internal override void Draw()
    { 
        if(impacto) return;
        else base.Draw();
    }

}