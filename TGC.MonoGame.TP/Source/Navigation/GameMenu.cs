using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Navigation;

namespace PistonDerby;

public class GameMenu{
    private bool Running = true;
    internal bool isRunning() => this.Running;
    private IMenuItem MenuState;
    public GameMenu(int width, int heigth){
        MenuState = new Presentation(width, heigth);
    }
    internal void Draw(GameTime gameTime){
        Running = MenuState.Draw(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds));
    }

    internal void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
    {
        MenuState = MenuState.Update(gameTime, keyboardState, mouseState);
    }
}