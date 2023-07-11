
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Autos.AI;
using PistonDerby.Autos.PowerUps;
using PistonDerby.Drawers;
using PistonDerby.Elementos;

namespace PistonDerby.Autos;
internal class AutoAI : Auto
{
    private AIState AIState = new PerseguirState();
    internal override IDrawer StateDrawer => new CarDrawer(this, new Vector3(255f, 0.5f, 0));
    private float Contador = 0;
    private float DistanciaAlEnemigo = 0;
    
    private Auto target;
    internal AutoAI(Auto auto, Vector3 posicionInicial) : base(posicionInicial){
        target = auto;
    }
    public void Update(float dTime){
        Contador+=dTime;
        KeyboardState keyboard = MoverHaciaObjetivo(target.Position());
        
        base.Update(dTime, keyboard);
    }
    internal override bool OnCollision(Elemento other, Vector3 normal, float profundidad)
    {
        if(other is MachineGun bala){
            // if(!Inmune()){
            //     this.ApplyLinearImpulse(-normal * profundidad);
            // }
            return false;
        }

        return true;
    }
    private KeyboardState MoverHaciaObjetivo(Vector3 objetivo){
        return new KeyboardState(AIState.movimiento(objetivo, this).ToArray());
    }

    internal void RecordTime(float totalSeconds) => AIState.ActualTime(totalSeconds);
}
