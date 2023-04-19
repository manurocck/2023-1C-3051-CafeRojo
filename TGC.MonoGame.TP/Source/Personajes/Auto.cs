using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class Auto : IElementoDinamico
    { 
        private Vector3 Position;
        private Vector3 Velocity;
        private float AccelerationMagnitude = 2500f;
        private float Rotation;
        private float JumpPower = 50000f;
        private float Turning = 0f;

        public Auto(Vector3 posicionInicial, Vector3 rotacion) : base(posicionInicial, rotacion)
        {
            Model = TGCGame.GameContent.M_Auto;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float accelerationSense = 0f;
            Vector3 acceleration = Vector3.Zero;
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);


            // GRAVEDAD
            float floor = 0f;
            Vector3 Gravity = -Vector3.Up * 15f;
            Matrix MatrixRotation = Matrix.CreateRotationY(Rotation);
  
            // GIRO
            Turning += keyboardState.IsKeyDown(Keys.A) ? 1f : 0;
            Turning -= keyboardState.IsKeyDown(Keys.D) ? 1f : 0;
            Rotation = Turning * dTime;

            if(Position.Y<floor){
                Position = new Vector3(Position.X, floor, Position.Z);
                Velocity = new Vector3(Velocity.X, 0, Velocity.Z);
            
                // ACELERACION
                if (keyboardState.IsKeyDown(Keys.W))
                    accelerationSense = 1f;
                else if (keyboardState.IsKeyDown(Keys.S))
                    accelerationSense = -0.5f; //Reversa mas lenta

                Vector3 accelerationDirection = -MatrixRotation.Forward;
                acceleration = accelerationDirection * accelerationSense * AccelerationMagnitude;

                // ROZAMIENTO
                float u = -1.35f; //Coeficiente de Rozamiento
                if (keyboardState.IsKeyDown(Keys.LeftShift)) // LShift para Frenar
                    u*=2;
                Vector3 Friction = new Vector3(Velocity.X, 0, Velocity.Z) * u * dTime;
                Velocity += Friction;
            }
            else {
                Velocity += Gravity;
            }
            
            // SALTO
            if (keyboardState.IsKeyDown(Keys.Space) && Position.Y==floor)
                Velocity += Vector3.Up * JumpPower * dTime;

            Velocity += acceleration * dTime;
            Position += Velocity * dTime;
            
            // MATRIZ DE MUNDO
            World = 
                Matrix.CreateScale(0.75f) * 
                MatrixRotation *
                Matrix.CreateTranslation(Position);
        }
    }
}