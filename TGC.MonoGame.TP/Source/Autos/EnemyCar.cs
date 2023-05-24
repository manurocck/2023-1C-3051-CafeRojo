
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Utils;

using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Elementos;

namespace TGC.MonoGame.TP{
    internal class EnemyCar : ElementoDinamico {
        internal override float Mass() => 0.0001f; //Es indistinto
        internal override float Scale() => 1.4f;
        internal override IDrawer Drawer() => this.StateDrawer;
        private IDrawer StateDrawer = new TextureDrawer(TGCGame.GameContent.M_AutoEnemigo, TGCGame.GameContent.T_MarmolNegro);

        internal EnemyCar(float posX, float posY, float posZ, Vector3 rotacion) 
        {
            var PosicionInicial = new Vector3(posX, posY, posZ) * TGCGame.S_METRO;
            PosicionInicial.Y = 2f; // Hard-code de la altura

            var boxSize = TGCGame.GameContent.M_AutoEnemigo.Dimensiones() * 0.01f; //SIMU_BOX_SCALE Que va a ir a Content
            Shape = TGCGame.Simulation.LoadShape<Box>(new Box(boxSize.X,boxSize.Y,boxSize.Z));
            this.AddToSimulation(PosicionInicial, Quaternion.Identity);
        }

        internal override bool OnCollision(Elemento other)
        {
            if(other is not Auto _){
                return true;
            }
            
            StateDrawer = new TextureDrawer(TGCGame.GameContent.M_AutoEnemigo, TGCGame.GameContent.T_Ladrillos);
            return false;
        }
    }
}