using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BepuPhysics.Collidables;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Elementos;
using TGC.MonoGame.TP.Drawers;

namespace TGC.MonoGame.TP;
internal class Auto : ElementoDinamico { 
    private const float WHEEL_TURNING_LIMIT = 0.5f;
    private const float ANGULAR_SPEED = 30f;
    private const float LINEAR_SPEED = 30f;
    private const float WHEEL_ROTATION_FACTOR = 0.000008f; // Factor de ajuste para la rotación
    private const float JUMP_POWER = 10000f; // Factor de ajuste para la rotación
    private bool PuedeSaltar() => true;
    internal override float Mass() => 1f;
    internal override float Scale() => 0.08f * TGCGame.S_METRO;
    internal override IDrawer Drawer() => new CarDrawer(this);
    internal float WheelTurning = 0f; 
    internal float WheelRotation = 0f;

    internal Auto(Vector3 posicionInicial) {
        var boxSize = TGCGame.GameContent.M_Auto.Dimensiones() * 0.010f * this.Scale(); //SIMU_BOX_SCALE Que va a ir a Content
        Shape = TGCGame.Simulation.LoadShape<Box>(new Box(boxSize.X,boxSize.Y,boxSize.Z));
        this.AddToSimulation(posicionInicial + new Vector3(0,TGCGame.S_METRO,0), Quaternion.Identity);
    }

    internal override void Update(float dTime, KeyboardState keyboard) {   
        CarDrawer pivot = (CarDrawer) Drawer();
        pivot.CarPosition = this.Position();

        if(keyboard.GetPressedKeyCount()>0) this.Awake();

        // GIRO
        var velocidadActual = this.LinearVelocity();
        var coeficienteVelocidad = (Math.Abs(velocidadActual.X) + Math.Abs(velocidadActual.Y) + Math.Abs(velocidadActual.Z)) / 2000;  // Posiblemente podamos usar Length(velocidadActual) / Length(this.LinearVelocity())
        //Console.WriteLine("Velocidad alcanzada :    . . . . . . {0:F}%", (coeficienteVelocidad * 100f));

        // ROTACION DE RUEDAS
        WheelTurning += -Math.Sign(WheelTurning) * WHEEL_TURNING_LIMIT * dTime;
        WheelTurning = Math.Clamp(WheelTurning + keyboard.TurningAxis() * WHEEL_TURNING_LIMIT * dTime * 4f, -WHEEL_TURNING_LIMIT, WHEEL_TURNING_LIMIT);

        WheelRotation += this.AngularVelocity().Y * WHEEL_ROTATION_FACTOR;

        // IMPULSOS
        Vector3 horizontalImpulse = this.Rotation().Forward() * keyboard.AccelerationSense() * LINEAR_SPEED;
        Vector3 verticalImpulse = PuedeSaltar() && keyboard.Jumped() ? this.Rotation().Up() * JUMP_POWER : Vector3.Zero ;
        Vector3 angularImpulse = this.Rotation().Up() * ANGULAR_SPEED * ANGULAR_SPEED * WheelTurning; /* (* Math.Min(coeficienteVelocidad * 4, 1)) */ // Si le ponemos eso, no se despega de la pared

        // SUUUUPER interesante lo que pasa con este offset (investigar)
        // Vector3 offset = TGCGame.GameContent.M_Auto.Size(); // tracción delantera
        float offsetAmount = 2f;
        this.ApplyLinearImpulse(horizontalImpulse + verticalImpulse, offsetAmount);
        this.ApplyAngularImpulse(angularImpulse);
    }
}
