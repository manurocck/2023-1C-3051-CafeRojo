using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Autos.PowerUps;
using PistonDerby.Drawers;
using PistonDerby.Elementos;
using PistonDerby.Utils;

namespace PistonDerby.Autos;
internal class AutoDummy : Auto
{
    private float Contador = 0;
    private bool isDead = false;
    private IDrawer[] DrawerStates;
    internal override IDrawer StateDrawer => CarDrawerState();
    internal AutoDummy(Vector3 posicionInicial) : base(posicionInicial){
        DrawerStates = new IDrawer[]{
                    new CarDrawer(this,new Vector3(0, 255f, 0)),
                    new DeadCarDrawer(this)}; 
    }
    private Random miRandom = new Random();

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
    private IDrawer CarDrawerState()
    {
        if(isDead) return DrawerStates[1];
        else return DrawerStates[0];
    }
    private void Die(){
        isDead = true;
        this.Body().Velocity.Angular = Vector3.Zero.ToBepu();
        this.Body().Velocity.Linear = Vector3.Zero.ToBepu();
        this.Body().BecomeKinematic();
    }
    public void Update(float dTime){
        if(Vida==0){
            if(!isDead) Die();
            return;
        } 
        Contador+=dTime;
        KeyboardState keyboard = DerrapeEnElLugar();
        
        base.Update(dTime, keyboard);
    }
    internal override bool OnCollision(Elemento other) => !isDead;
    internal override bool OnCollision(Elemento other, Vector3 normal, float profundidad)
    {
        if(isDead) return false;
        if(other is MachineGun bala){
            if(!Inmune()){
                this.ApplyLinearImpulse(-normal * profundidad);
            }
            return false;
        }

        return true;
    }
    public bool IsDead() => isDead;
}
