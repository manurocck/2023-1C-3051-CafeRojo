
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Utils;

using PistonDerby.Drawers;
using PistonDerby.Elementos;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Autos.PowerUps;
internal class PowerUp : ElementoDinamico {
    internal override float Mass() => 0.0001f; //Es indistinto
    internal override float Scale() => 1.4f;
    internal override IDrawer Drawer => this.StateDrawer;
    internal override Model Model => PistonDerby.GameContent.M_AutoEnemigo;
    private IDrawer StateDrawer = new TextureDrawer(PistonDerby.GameContent.T_MarmolNegro);
    public bool Dirty = false;
    private float StateTimer = 0;

    internal PowerUp(float posX, float posY, float posZ, Vector3 rotacion) 
    {
        var PosicionInicial = new Vector3(posX, posY, posZ) * PistonDerby.S_METRO;
        PosicionInicial.Y = 2f; // Hard-code de la altura

        Box box = PistonDerby.GameContent.M_AutoEnemigo.GeneraterBox(this.Scale());
        Shape = PistonDerby.Simulation.LoadShape<Box>(box);
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
            if(!Dirty) StateDrawer = new TextureDrawer(PistonDerby.GameContent.T_MarmolNegro); 
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
}
