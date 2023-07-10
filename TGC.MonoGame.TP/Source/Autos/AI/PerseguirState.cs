
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Utils;

namespace PistonDerby.Autos.AI;
internal class PerseguirState : AIState
{
    private const float VELOCIDAD_MAX = 300;
    private bool Perseguir = true;
    private float TiempoFinalReacomodo = 0;

    internal override List<Keys> movimiento(Vector3 objetivo, AutoAI autoAI){
        if(Perseguir){
            Console.WriteLine("Persiguiendo ... ");
            return perseguir(objetivo, autoAI);
        }
        else{
            Console.WriteLine("Reacomodando ... ");
            if(ActualSeconds<TiempoFinalReacomodo) 
                TiempoFinalReacomodo=ActualSeconds+2;
            else 
                Perseguir = true;
            
            return reacomodar();
        }
    }
    private List<Keys> reacomodar(){
        List<Keys> listOfKeys = new List<Keys>();
        
        listOfKeys.Add(Keys.D);
        listOfKeys.Add(Keys.S);
        
        return listOfKeys;
    }
    private List<Keys> perseguir(Vector3 objetivo, AutoAI autoAI)
    {
        if(autoAI.LinearVelocity().Length()==0)
            Perseguir = false;
        List<Keys> listOfKeys = new List<Keys>();

        Vector3 posicionActual = autoAI.Position();

        // Proyectar la posición actual y el objetivo en el plano XZ
        Vector3 posicionActualXZ = new Vector3(posicionActual.X, 0, posicionActual.Z);
        Vector3 objetivoXZ = new Vector3(objetivo.X, 0, objetivo.Z);

        // Calcular la dirección hacia el objetivo en el plano XZ
        Vector3 direccion = objetivoXZ - posicionActualXZ;
        
        // Vector adelante del AI
        Vector3 fowardVector = QuaternionExtensions.Forward((autoAI.Body().Pose.Orientation.ToQuaternion()));
        
        // Ángulo al vector direccion.  (ojo si vale 0 el denominador)
        // Valores desde 0 : justo adelante a 3.14: justo atras
        float angulo = MathF.Acos(direccion.DotProduct(fowardVector)/(direccion.Length()*fowardVector.Length()));
        
        if(angulo > 0.9)
            listOfKeys.Add(Keys.A);
        if(autoAI.LinearVelocity().Length() < VELOCIDAD_MAX){
            // Mover hacia adelante
            listOfKeys.Add(Keys.W);
        }    
        return listOfKeys;    
    }
}