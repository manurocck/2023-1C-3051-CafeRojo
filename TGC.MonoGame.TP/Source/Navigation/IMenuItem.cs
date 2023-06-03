
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PistonDerby.Navigation;

public abstract class IMenuItem {
    internal static Matrix AjusteQuad() => Matrix.CreateTranslation(new Vector3(-0.5f,0,-0.5f)) //centrar 
                                         * Matrix.CreateRotationX(MathHelper.PiOver2) ;         //levantar
    internal static Matrix AjusteFinal(int z) => Matrix.CreateTranslation(-Vector3.UnitZ * (200f+z*5f));    //standard
    internal Matrix Down(float y) => Matrix.CreateTranslation(Vector3.UnitY * (-y * this.scaleFactor().Y) );
    internal Matrix Up(float y) => Matrix.CreateTranslation(Vector3.UnitY * (y * this.scaleFactor().Y) );
    internal Matrix Right(float x) => Matrix.CreateTranslation(Vector3.UnitX * (x * this.scaleFactor().X) );
    internal Matrix Left(float x) => Matrix.CreateTranslation(Vector3.UnitX * (-x * this.scaleFactor().X) );
    internal virtual Matrix QuadSize(float X,float Y) => Matrix.CreateScale(X * scaleFactor().X , Y *scaleFactor().Y , 0); //tamaño imágen
    internal virtual Matrix QuadSize(float X) => Matrix.CreateScale(X * scaleFactor().Y , X *scaleFactor().Y , 0); //tamaño imágen cuadrada
    internal abstract bool Draw(float secondsElapsed);

    internal virtual IMenuItem Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) => this;

    protected readonly (int width, int heigth) Window;
    private (float X, float Y) scaleFactor() => ((Window.width)*0.2f , (Window.heigth)*0.2f);
    internal Matrix HUDView = Matrix.CreateLookAt(Vector3.Zero,- Vector3.UnitZ, Vector3.UnitY);
    public IMenuItem ( int width, int heigth) => Window = (width , heigth);   

    internal Matrix FullScreenWorld() => AjusteQuad() * QuadSize(1,1) * AjusteFinal(1);
}