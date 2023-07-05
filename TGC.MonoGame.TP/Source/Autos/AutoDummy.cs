using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Autos.PowerUps;
using PistonDerby.Elementos;

namespace PistonDerby.Autos;
internal class AutoDummy : Auto
{
    private float Contador = 0;
    private Random miRandom = new Random();
    internal AutoDummy(Vector3 posicionInicial) : base(posicionInicial){}

    private KeyboardState DerrapeEnElLugar(){
        List<Keys> listOfKeys = new List<Keys>();

        if( (Contador%3)>.75 && miRandom.NextSingle() < 0.85f) listOfKeys.Add(Keys.W);
        if( (Contador%4)<2 && miRandom.NextSingle() < 0.75f) listOfKeys.Add(Keys.A);
        else {
            listOfKeys.Add(Keys.D);
            listOfKeys.Add(Keys.S);
        }
        return new KeyboardState(listOfKeys.ToArray());
    }
    public void Update(float dTime){
        Contador+=dTime;
        KeyboardState keyboard = DerrapeEnElLugar();
        
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
    
}
