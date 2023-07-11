using System.Globalization;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PistonDerby.Drawers;
using PistonDerby.Elementos;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Utils;
using System;

namespace PistonDerby.Autos.PowerUps;
internal class MachineGun : ElementoDinamico {
    private const float SPEED = 0.5f * PistonDerby.S_METRO;
    internal override float Mass() => 0.1f;
    internal override float Scale() => 20;
    internal override IDrawer Drawer => this.StateDrawer;
    internal override Model Model => PistonDerby.GameContent.M_Lego;
    private IDrawer StateDrawer;
    private Auto Owner;

    private Vector3 RotacionInicial = new Vector3(MathHelper.PiOver2+MathHelper.PiOver4*0.25f, 0, 0);

    internal MachineGun(Vector3 posicionInicial, Vector3 forward, Auto owner){
        Cylinder hitRange = new Cylinder(50, 10000);
        Shape = PistonDerby.Simulation.LoadShape<Cylinder>(hitRange);
        Owner = owner;
        StateDrawer = new MachineGunDrawer(owner);

        this.AddToSimulation(posicionInicial +Vector3.UnitY*25f+Owner.Rotation().Forward()*80f, forward.ToQuaternion());
    }

    internal override void Update(float dTime, KeyboardState _){ }

    internal override bool OnCollision(Elemento other) => false;
    internal override bool OnCollision(Elemento other, Vector3 _, float depth)
    {
        if(other is Auto auto && auto != Owner){
            if(Owner.isShooting()) auto.HitByMachineGun(Owner.Rotation().Forward(), depth);
        }
        return false;
    }
    internal override void Draw() => this.StateDrawer.Draw(Model, this.World);

    internal void Update(Vector3 vector3, Quaternion rotation)
    {
        this.Body().Pose.Position = (Owner.Position()+rotation.Forward()*80f+Vector3.UnitY*25f).ToBepu();

        this.Body().Pose.Orientation = Owner.Rotation().ToBepu();
    }

    internal void Bloom(Camera camera)
    {
        Effect BloomEffect = PistonDerby.GameContent.E_BloomEffect;
        BloomEffect.Parameters["baseTexture"].SetValue(PistonDerby.GameContent.T_Marmol);
        Vector3 colorToBloom = (Owner.isShooting())? Vector3.Zero : Vector3.UnitX; 
        BloomEffect.Parameters["colorToBloom"].SetValue(colorToBloom); //bloom color rojo
        
        Matrix world = 
                         Matrix.CreateScale(1f, 200f, 1f)
                        * Matrix.CreateRotationX(MathHelper.PiOver2 + MathHelper.PiOver4 * 0.20f)
                        * Matrix.CreateFromQuaternion(Owner.Rotation())
                        * Matrix.CreateTranslation(Owner.Rotation().Forward()*80f)
                        * Matrix.CreateTranslation(Vector3.UnitY*25f)
                        * Matrix.CreateTranslation(Owner.Position());

        BloomEffect.Parameters["WorldViewProjection"].SetValue(world * camera.View * camera.Projection);
        
        PistonDerby.GameContent.G_Cilindro.Draw(BloomEffect);
    }
}