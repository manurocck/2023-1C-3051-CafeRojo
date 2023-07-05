using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Autos.PowerUps;
using PistonDerby.Elementos;

namespace PistonDerby.Autos;
internal class AutoAI : Auto
{
    private float Contador = 0;
    private Random miRandom = new Random();
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
            if(!Inmune()){
                this.ApplyLinearImpulse(-normal * profundidad);
            }
            return false;
        }

        return true;
    }
    private KeyboardState MoverHaciaObjetivo(Vector3 objetivo){
        List<Keys> listOfKeys = new List<Keys>();

        Vector3 posicionActual = this.Position();

        // Proyectar la posición actual y el objetivo en el plano XZ
        Vector3 posicionActualXZ = new Vector3(posicionActual.X, 0, posicionActual.Z);
        Vector3 objetivoXZ = new Vector3(objetivo.X, 0, objetivo.Z);

        // Calcular la dirección hacia el objetivo en el plano XZ
        Vector3 direccion = objetivoXZ - posicionActualXZ;
        direccion.Normalize();

        // Determinar las teclas correspondientes a la dirección
        if (direccion.Z > 0)
        {
            // Mover hacia adelante
            listOfKeys.Add(Keys.W);
        }
        else
        {
            // Mover hacia atrás
            listOfKeys.Add(Keys.S);
        }
    
        if (direccion.X > 0)
        {
            // Mover hacia la derecha
            listOfKeys.Add(Keys.D);
        }
        else
        {
            // Mover hacia la izquierda
            listOfKeys.Add(Keys.A);
        }
    

        return new KeyboardState(listOfKeys.ToArray());
    }

}
