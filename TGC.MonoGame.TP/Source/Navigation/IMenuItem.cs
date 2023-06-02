
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Navigation;

public abstract class IMenuItem {
    internal static Matrix AjusteQuad() => Matrix.CreateTranslation(new Vector3(-0.5f,0,-0.5f)) //centrar 
                                         * Matrix.CreateRotationX(MathHelper.PiOver2) ;         //levantar
    internal static Matrix AjusteFinal(int z) => Matrix.CreateTranslation(-Vector3.UnitZ * (200f+z*5f));    //standard
    internal virtual Matrix QuadSize(float X,float Y) => Matrix.CreateScale(X * scaleFactor.X , Y *scaleFactor.Y , 0); //tamaño imágen
    internal abstract bool Draw(float secondsElapsed);

    internal virtual IMenuItem Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) => this;

    private (float X, float Y) scaleFactor;
    internal Matrix HUDView = Matrix.CreateLookAt(Vector3.Zero,- Vector3.UnitZ, Vector3.UnitY);
    public IMenuItem ( int width, int heigth) => this.scaleFactor = ((width)*0.2f , (heigth)*0.2f);   

    internal Matrix FullScreenWorld() => AjusteQuad() * QuadSize(1,1) * AjusteFinal(1);
}