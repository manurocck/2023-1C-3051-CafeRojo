
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Utils;

using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Elementos;

namespace TGC.MonoGame.TP{
    internal class EnemyCar : ElementoDinamico {
        internal override float Mass() => 0.0001f;
        internal override float Scale() => 1.4f;
        internal override IDrawer Drawer() => new TextureDrawer(TGCGame.GameContent.M_AutoEnemigo, TGCGame.GameContent.T_MarmolNegro);
        bool triggered = false;

        internal EnemyCar(float posX, float posY, float posZ, Vector3 rotacion) 
        {
            var PosicionInicial = new Vector3(posX, posY, posZ) * TGCGame.S_METRO;
            PosicionInicial.Y = 2f; // Hard-code de la altura


            var boxSize = TGCGame.GameContent.M_AutoEnemigo.Size() * 0.01f; //SIMU_BOX_SCALE Que va a ir a Content
            Shape = new Box(boxSize.X,boxSize.Y,boxSize.Z);

            this.AddToSimulation<Box>(PosicionInicial, Quaternion.Identity);
        }

        internal override void Update(float dTime, KeyboardState keyboard)
        {
            // if(this.LinearVelocity().Length() > 2f) triggered = true;
            triggered = this.LinearVelocity().Length() > 2f;
        }

        internal override void Draw() {
            if(!triggered) 
                base.Draw();
        } 
    }
}