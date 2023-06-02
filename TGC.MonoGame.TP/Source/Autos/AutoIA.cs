using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.HUD;

namespace PistonDerby.Autos;
internal class AutoIA : Auto
{
    internal AutoIA(Vector3 posicionInicial) : base(posicionInicial){}

    public void Update(float dTime){
        // Keys[] listOfKeys = {Keys.W, Keys.A};

        List<Keys> listOfKeys = new List<Keys>();
        listOfKeys.Add(Keys.W);
        if(Random.Shared.NextSingle() < 0.45f) listOfKeys.Add(Keys.A);

        KeyboardState keyboard = new KeyboardState(listOfKeys.ToArray());
        base.Update(dTime, keyboard);
    }
}
