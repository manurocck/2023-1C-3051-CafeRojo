using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BepuPhysics;
using BepuPhysics.Collidables;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Elementos;
using TGC.MonoGame.TP.Drawers;

namespace TGC.MonoGame.TP
{
    internal class Auto : MDynamic, MDrawable
    { 
        private const float WHEEL_TURNING_LIMIT = 0.5f;
        private const float ANGULAR_SPEED = 900f;
        private const float LINEAR_SPEED = 80f;
        private const float WHEEL_ROTATION_FACTOR = 0.000008f; // Factor de ajuste para la rotación
        private const float JUMP_POWER = 1000f; // Factor de ajuste para la rotación
        private bool PuedeSaltar() => true;

        //MDynamic Properties
        public BodyHandle BodyHandle { set; get; }
        public IConvexShape Shape { set; get; } 
        public float Mass() => 3f;
        public float Scale() => 0.08f * TGCGame.S_METRO;

        //MDrawer Properties
        public IDrawer Drawer { set; get; } = new CarDrawer();
        Matrix MDrawable.World() => this.World();

        public Auto(Vector3 posicionInicial)
        {
            var boxSize = TGCGame.GameContent.M_Auto.Size() * 0.010f * this.Scale(); //SIMU_BOX_SCALE Que va a ir a Content
            Shape = new Box(boxSize.X,boxSize.Y,boxSize.Z);

            this.AddToSimulation<Box>(posicionInicial + new Vector3(0,TGCGame.S_METRO,0));
        }

        public void Update(float dTime, KeyboardState keyboard)
        {   
            CarDrawer pivot = (CarDrawer) Drawer;
            pivot.CarPosition = this.Position();
            // GIRO
            var velocidadActual = this.LinearVelocity();
            var coeficienteVelocidad = (Math.Abs(velocidadActual.X) + Math.Abs(velocidadActual.Y) + Math.Abs(velocidadActual.Z)) / 2000;  // Posiblemente podamos usar Length(velocidadActual) / Length(this.LinearVelocity())
            Console.WriteLine("Velocidad alcanzada :    . . . . . . {0:F}%", (coeficienteVelocidad * 100f));

            // ROTACION DE RUEDAS
            pivot.WheelTurning += -Math.Sign(pivot.WheelTurning) * WHEEL_TURNING_LIMIT * dTime;
            pivot.WheelTurning = Math.Clamp(pivot.WheelTurning + keyboard.TurningAxis() * WHEEL_TURNING_LIMIT * dTime * 4f, -WHEEL_TURNING_LIMIT, WHEEL_TURNING_LIMIT);

            pivot.WheelRotation += this.AngularVelocity().Y * WHEEL_ROTATION_FACTOR;

            // IMPULSOS
            Vector3 horizontalImpulse = this.Rotation().Forward() * keyboard.AccelerationSense() * LINEAR_SPEED;
            Vector3 verticalImpulse = PuedeSaltar() && keyboard.Jumped() ? this.Rotation().Up() * JUMP_POWER : Vector3.Zero ;
            Vector3 angularImpulse = new Vector3(0,ANGULAR_SPEED,0) * pivot.WheelTurning * Math.Min(coeficienteVelocidad * 4, 1);

            this.ApplyLinearImpulse(horizontalImpulse + verticalImpulse);
            this.ApplyAngularImpulse(angularImpulse);
        }
    }
}
    