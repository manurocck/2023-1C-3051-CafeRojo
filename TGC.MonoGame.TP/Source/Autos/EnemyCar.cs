
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using PistonDerby.Utils;

using PistonDerby.Drawers;
using PistonDerby.Elementos;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby;
internal class EnemyCar : ElementoDinamico {
    internal override float Mass() => 0.0001f; //Es indistinto
    internal override float Scale() => 1.4f;
    internal override IDrawer Drawer() => this.StateDrawer;
    private IDrawer StateDrawer = new TextureDrawer(PistonDerby.GameContent.M_AutoEnemigo, PistonDerby.GameContent.T_MarmolNegro);
    private bool Dirty = false;
    private float StateTimer = 0;

    internal EnemyCar(float posX, float posY, float posZ, Vector3 rotacion) 
    {
        var PosicionInicial = new Vector3(posX, posY, posZ) * PistonDerby.S_METRO;
        PosicionInicial.Y = 2f; // Hard-code de la altura

        var boxSize = PistonDerby.GameContent.M_AutoEnemigo.Dimensiones() * 0.01f; //SIMU_BOX_SCALE Que va a ir a Content
        Shape = PistonDerby.Simulation.LoadShape<Box>(new Box(boxSize.X,boxSize.Y,boxSize.Z));
        this.AddToSimulation(PosicionInicial, Quaternion.Identity);
    }
    internal override void Update(float dTime, KeyboardState _)
    {
        StateTimer += dTime;
        
        if(StateTimer > 2){
            if(Dirty) {
                StateDrawer = new NotDrawer();
                StateTimer = 0;
            }
            if(!Dirty) StateDrawer = new TextureDrawer(PistonDerby.GameContent.M_AutoEnemigo, PistonDerby.GameContent.T_MarmolNegro); 
            Dirty = false;
        }
    }

    internal override bool OnCollision(Elemento other)
    {
        if(other is not Auto _){
            return true;
        }

        if(!Dirty) Dirty = !Dirty;
        
        return false;
    }
}
