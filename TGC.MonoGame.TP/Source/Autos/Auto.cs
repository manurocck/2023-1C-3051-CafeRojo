using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Utils;
using PistonDerby.Elementos;
using PistonDerby.Drawers;
using PistonDerby.HUD;
using PistonDerby.Collisions;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Autos.PowerUps;

namespace PistonDerby.Autos;
internal class Auto : ElementoDinamico { 
    #region Config
    private const float WHEEL_TURNING_LIMIT = 0.5f;
    private const float ANGULAR_SPEED = 1.8f * PistonDerby.S_METRO;
    private const float LINEAR_SPEED = 0.12f * PistonDerby.S_METRO;
    private const float JUMP_POWER = 1f * PistonDerby.S_METRO;
    private const float WHEEL_ROTATION_FACTOR = 0.000008f; // Factor de ajuste para la rotación (setead para S_METRO = 250f) puede fallar (o no) si se cambia
    #endregion Settings
    internal override Model Model => PistonDerby.GameContent.M_Auto;
    internal override IDrawer Drawer => new CarDrawer(this);
    internal override float Scale() => 0.08f * PistonDerby.S_METRO;
    internal override float Mass() => 1f;
    private CarHUD DisplayEstado;
    private float Vida = 1;
    private float Turbo = 1;
    private bool PuedeSaltar = true;
    private float TimerVolcado = 0;
    private float TimerInmune = 3;
    internal float WheelTurning = 0f;
    internal float WheelRotation = 0f;

    internal Auto(Vector3 posicionInicial) { // Auto(Vector3 posicionInicial, CarHUD EstadoInicial ) 
        Shape = PistonDerby.Simulation.LoadShape(ShapeType.BOX, this.Model, this.Scale());
        this.AddToSimulation(posicionInicial + new Vector3(200,PistonDerby.S_METRO,200), Quaternion.Identity);
    }
    internal void AsociarHUD(CarHUD EstadoInicial) => this.DisplayEstado = EstadoInicial;

    internal override void Update(float dTime, KeyboardState keyboard) {

        if(keyboard.GetPressedKeyCount()>0) this.Awake();
        
        TimerInmune+=dTime;
        Turbo = Math.Max(Turbo - 0.5f*dTime, 0);
        this.DisplayEstado?.Update(this.World, Vida, Turbo); // la vida y el turbo están en coeficientes entre 0 y 1
        
        if(TimerVolcado>2) this.Awake();
        if(!this.Body().Awake) return;

        if(keyboard.IsKeyDown(Keys.LeftShift)) DisplayEstado?.BulletAmmo.PullingTrigger(dTime);
        if(keyboard.IsKeyUp(Keys.LeftShift)) DisplayEstado?.BulletAmmo.ReleasingTrigger();

        //  ESTADO ACTUAL
        //
        //      velocidadActual  :  Vector velocidad real (con sentido y magnitud)
        //      porcentajeActual :  Relación entre la Velocidad actual y la velocidad máxima. Entre 0 y 1.
        //      cajaSombra :        Actualiza su posición junto con la del auto
        //
        Vector3 velocidadActual = this.LinearVelocity();
        float porcentajeVelocidad = velocidadActual.Length() / 2000;
        PuedeSaltar = this.SobreSombra();


        //  CORRECCIONES AUTOMÁTICAS
        //
        //      WheelTurning :      Hay que tratar de devolver las ruedas a su lugar. 
        //                          Al giro de las ruedas, le resta un delta del límite.
        //                          Se lo debe restringir (clamp) a sus límites {- WHEEL_TURNING_LIMIT ; WHEEL_TURNING_LIMIT}
        //
        WheelTurning += -Math.Sign(WheelTurning) * WHEEL_TURNING_LIMIT * dTime;
        WheelTurning = Math.Clamp(WheelTurning + keyboard.TurningAxis() * WHEEL_TURNING_LIMIT * dTime * 4f, 
                                            -WHEEL_TURNING_LIMIT, WHEEL_TURNING_LIMIT);
        //
        //      WheelRotation :     Las ruedas tienen que girar sobre el vector "Up" del auto
        //                          una vez que son colocadas en su posición.
        //                          Se determina según su velocidad angular y un factor mágico de ajuste.
        //
        WheelRotation += this.AngularVelocity().Y * WHEEL_ROTATION_FACTOR;

        
        //  CONTROLES
        //
        //      AngularImpulse :    El giro se aplica sobre el Vector "Up" del auto.
        //                          Toma potencia y dirección según WheelTurning.
        //                          Visualmente es como si fuera un trompo.
        //
        Vector3 angularImpulse = this.Rotation().Up() * (ANGULAR_SPEED * 2) * WheelTurning; /* (* Math.Min(porcentajeVelocidad * 4, 1)) */ // Si le ponemos eso, no se despega de la pared
        this.ApplyAngularImpulse(angularImpulse);
        //
        //      HorizontalImpulse : El impulso se aplica desde la "trompa" del auto (offsetAmount).
        //                          Si está en el piso, la dirección será la de la rotación actual.
        //                          Si no está en el piso, va a ser con la que venía. 
        //                          El sentido lo determinan las teclas W y S con AccelerationSense().
        //
        Vector3 horizontalImpulse = (PuedeSaltar)? this.Rotation().Forward() : Vector3.Zero /* velocidadActual.XZFoward() */;
        horizontalImpulse *= keyboard.AccelerationSense() * LINEAR_SPEED ;
        float offsetAmount = 2f; //habría que generalizarlo para ubicar exáctamente en donde están las ruedas o un poquito más adelante
        this.ApplyLinearImpulse(horizontalImpulse, offsetAmount);
        //
        //      VerticalImpulse :   A desarrollar.
        //
        if(PuedeSaltar && keyboard.Jumped())
        {
            TimerVolcado = 0;
            Vector3 verticalImpulse = Vector3.UnitY * JUMP_POWER ;
            this.ApplyLinearImpulse(verticalImpulse);

            PuedeSaltar = false;
        }else if(!PuedeSaltar){
            TimerVolcado += dTime;
            if(TimerVolcado > 2){
                this.Awake(); // no llega nunca por el return del principio
                TimerVolcado = 0;
                if(this.Body().Pose.Orientation.Up().Y>0){
                    Vector3 offsetDirection;
                    Vector3 correctiveImpulse = -Body().Pose.Orientation.Up() * JUMP_POWER * 0.75f;
                    if(this.Body().Pose.Orientation.Left().Y < 0){
                        offsetDirection = -Body().Pose.Orientation.Left();
                    }else{
                        offsetDirection = Body().Pose.Orientation.Left();
                    }
                    this.ApplyImpulse(correctiveImpulse, offsetDirection * this.Scale());
                }else{
                    Vector3 correctiveImpulse = -Body().Pose.Orientation.Up() * JUMP_POWER * 0.75f;
                    this.ApplyImpulse(correctiveImpulse, Body().Pose.Orientation.Left() * this.Scale());
                }
            }
        }
    }

    internal override bool OnCollision(Elemento other)
    {
        if(other is PowerUp enemigo){
            if(enemigo.Dirty){
                Console.WriteLine("Toqué un Power-Up");
                Turbo = 1;
            }

            if(TimerInmune > 3){
                var hitDamage = 0.1f;
                TimerInmune = 0;
                Vida = (Vida>=hitDamage)? Vida-hitDamage : 0;
                Console.WriteLine("> > > > > > > > > > > > >");
                Console.WriteLine(">");
                Console.WriteLine("> > Recibí daño un enemy car");
                Console.WriteLine("> > Vida actual : {0:F}", Vida*100);
                Console.WriteLine(">");
                Console.WriteLine("> > > > > > > > > > > > >");
            }
        }
        if(other is Piso _){
            Console.WriteLine("Toqué el piso");
            PuedeSaltar = true;
        }
        if(other is ElementoEstatico _){
            Console.WriteLine("Toqué un elemento estático");
        }

        return true;
    }
    private bool SobreSombra() // debería adaptarse para que pueda saltar desde la mesa también
    { 
        BoundingBox sombraAcual = new BoundingBox(this.Body().BoundingBox.Min, this.Body().BoundingBox.Max);
        float alturaBoxSombra = sombraAcual.Max.Y - sombraAcual.Min.Y;
        
        sombraAcual.Min.Y = -alturaBoxSombra*0.5f;
        sombraAcual.Max.Y = -alturaBoxSombra*0.5f;
                    
        return sombraAcual.Intersects(this.Body().BoundingBox.ToBoundingBox()) && this.Body().Pose.Orientation.Up().Y>0;
    }
}
