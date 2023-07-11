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
using BepuPhysics.Collidables;

namespace PistonDerby.Autos;
internal class Auto : ElementoDinamico { 
    #region Config
    private const float WHEEL_TURNING_LIMIT = 0.5f;
    private const float ANGULAR_SPEED = 1.8f * PistonDerby.S_METRO;
    private const float LINEAR_SPEED = 0.12f * PistonDerby.S_METRO;
    private const float JUMP_POWER = 2f * PistonDerby.S_METRO;
    private const float WHEEL_ROTATION_FACTOR = 0.000008f; // Factor de ajuste para la rotación (setead para S_METRO = 250f) puede fallar (o no) si se cambia
    private const float WHEEL_FLOOR_ROTATION_FACTOR = 0.01f;
    #endregion Settings
    internal override Model Model => PistonDerby.GameContent.M_Auto;
    internal override IDrawer Drawer => StateDrawer;
    internal virtual IDrawer StateDrawer => new CarDrawer(this);
    internal override float Scale() => 0.08f * PistonDerby.S_METRO;
    internal override float Mass() => 1f;
    internal bool isShooting() => shootingState;
    internal bool shootingState = false;
    private CarHUD DisplayEstado;
    private MachineGun MachineGun;
    public float Vida = 1;
    private float Turbo = 1;
    private bool PuedeSaltar = true;
    private float TimerVolcado = 0;
    private float TimerInmune = 3;
    internal float WheelTurning = 0f;
    internal float WheelRotation = 0f;
    internal float WheelFloorRotation = 0f;
    private int retardo = 0;
    private bool MisilCargado = true;

    internal Auto(Vector3 posicionInicial) { // Auto(Vector3 posicionInicial, CarHUD EstadoInicial ) 
        Shape = PistonDerby.Simulation.LoadShape(ShapeType.BOX, this.Model, this.Scale());
        this.AddToSimulation(posicionInicial + new Vector3(200,PistonDerby.S_METRO,200), Quaternion.Identity);
        Body().Collidable.Continuity = ContinuousDetection.Continuous();

        MachineGun = new MachineGun(posicionInicial, this.Rotation().Forward(), this);
    }
    internal void AsociarHUD(CarHUD EstadoInicial) => this.DisplayEstado = EstadoInicial;

    internal override void Update(float dTime, KeyboardState keyboard) {

        //  ESTADO INICIAL
        //
        if(keyboard.GetPressedKeyCount()>0) this.Awake();
        
        TimerInmune+=dTime;
        //Turbo = Math.Max(Turbo - 0.5f*dTime, 0);
        this.DisplayEstado?.Update(this.World, Vida, Turbo); // la vida y el turbo están en coeficientes entre 0 y 1
        
        if(TimerVolcado>2) this.Awake();
        if(!this.Body().Awake) return;

        if(keyboard.MachineGunTrigger() && DisplayEstado?.BulletAmmo.Ammo > 0){
            DisplayEstado?.BulletAmmo.PullingTrigger(dTime);
            shootingState = true;
            if(retardo == 10){
                // Activar la visibilidad del Draw de la bala
                // var bala = new Bala(this.Position() + this.Rotation().Forward() * 100, this.Rotation().Forward());
                // PistonDerby.ElementosDinamicos.Add(bala);
                // retardo = 0;
            }                
        }
        else{
            DisplayEstado?.BulletAmmo.ReleasingTrigger();
            shootingState = false;
        }
        retardo = Math.Min(10, retardo+1);

        if(MisilCargado && keyboard.MissileTrigger()){
            var misil = new Misil(this.Position() + this.Rotation().Forward() * 120, this.Rotation());
            PistonDerby.ElementosDinamicos.Add(misil);
            MisilCargado = false;
        }
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
        //      WheelRotation :      Las ruedas tienen que girar sobre el vector "Up" del auto
        //                           una vez que son colocadas en su posición.
        //                           Se determina según su velocidad angular y un factor mágico de ajuste.
        //
        //      WheelFloorRotation : Rotación acumulada que depende de la velocidad del auto.
        //                           Se usa para rotar las ruedas sobre el piso.
        //                           Disminuye cuando la velocidad baja.
        //
        WheelRotation += this.AngularVelocity().Y * WHEEL_ROTATION_FACTOR;
        float accelerationFactor = keyboard.AccelerationSense();
        // Transformación lineal para que accelerationFactor tenga valores -2 (reversa), -1 (sin acción) y 2 (acelerando al máximo)
        accelerationFactor = accelerationFactor<1 ? accelerationFactor-1 : 2;
        WheelFloorRotation += (this.LinearVelocity().Length()) * WHEEL_FLOOR_ROTATION_FACTOR * accelerationFactor * dTime;

        
        //  VELOCIDAD LINEAL
        //
        //      linear_speed :      La velocidad lineal se aplica sobre el Vector "Forward" del auto.
        //                          Toma potencia y dirección según el eje de aceleración.
        //                          Visualmente es como si fuera un cohete.

        float linear_speed;
        if(PuedeSaltar && keyboard.Turbo() && Turbo > 0){
            linear_speed = LINEAR_SPEED * 2f;
            Turbo -= 0.01f;
        }
        else{
            linear_speed = LINEAR_SPEED;
        }       
        
        //  CONTROLES
        //
        //      AngularImpulse :    El giro se aplica sobre el Vector "Up" del auto.
        //                          Toma potencia y dirección según WheelTurning.
        //                          Visualmente es como si fuera un trompo.
        //
        Vector3 angularImpulse = this.Rotation().Up() * (ANGULAR_SPEED * 2) * WheelTurning; /* (* Math.Min(porcentajeVelocidad * 4, 1)) */ // Si le ponemos eso, no se despega de la pared
        this.ApplyAngularImpulse(angularImpulse);
        // MachineGun.ApplyAngularImpulse(angularImpulse);
               
        //      HorizontalImpulse : El impulso se aplica desde la "trompa" del auto (offsetAmount).
        //                          Si está en el piso, la dirección será la de la rotación actual.
        //                          Si no está en el piso, va a ser con la que venía. 
        //                          El sentido lo determinan las teclas W y S con AccelerationSense().
        //
        Vector3 horizontalImpulse = (PuedeSaltar)? this.Rotation().Forward() : Vector3.Zero /* velocidadActual.XZFoward() */;
        horizontalImpulse *= keyboard.AccelerationSense() * linear_speed;
        float offsetAmount = 2f; //habría que generalizarlo para ubicar exáctamente en donde están las ruedas o un poquito más adelante
        this.ApplyLinearImpulse(horizontalImpulse, offsetAmount);
        // MachineGun.ApplyLinearImpulse(horizontalImpulse, offsetAmount);
        //
        //      VerticalImpulse :   Salto. Se aplica sobre el Vector "Up" del auto.
        //                          Toma potencia según JUMP_POWER.
        //                          Se aplica si se presiona la tecla de salto y si está en el piso.
        //
        if(PuedeSaltar && keyboard.Jumped())
        {
            TimerVolcado = 0;
            Vector3 verticalImpulse = this.Rotation().Up() * JUMP_POWER ;
            this.ApplyLinearImpulse(verticalImpulse);
            // MachineGun.ApplyLinearImpulse(verticalImpulse);

            PuedeSaltar = false;
        }
        //      Volcado :           Si está dado vuelta, se le aplica un impulso correctivo.
        //                          Se aplica si no está en el piso y si pasaron 2 segundos desde que se volcó.
        //                          El impulso correctivo es una fuerza hacia arriba y hacia el lado contrario al que está volcado.
        //
        else if(!PuedeSaltar){
            TimerVolcado += dTime;
            if(TimerVolcado > 2){
                TimerVolcado = 0;
                if(this.Body().Pose.Orientation.Up().Y>0){ // Sobre uno de sus costados
                    Vector3 offsetDirection;
                    Vector3 correctiveImpulse = -Body().Pose.Orientation.Up() * JUMP_POWER * 0.4f;
                    if(this.Body().Pose.Orientation.Left().Y < 0){
                        offsetDirection = -Body().Pose.Orientation.Left();
                    }else{
                        offsetDirection = Body().Pose.Orientation.Left();
                    }
                    this.ApplyImpulse(correctiveImpulse, offsetDirection * this.Scale());
                    // MachineGun.ApplyImpulse(correctiveImpulse, offsetDirection * this.Scale());
                }else{ // Totalmente dado vuelta
                    Vector3 correctiveImpulse = -Body().Pose.Orientation.Up() * JUMP_POWER * 0.4f;
                    this.ApplyImpulse(correctiveImpulse, Body().Pose.Orientation.Left() * this.Scale());
                    // MachineGun.ApplyImpulse(correctiveImpulse, Body().Pose.Orientation.Left() * this.Scale());
                }

                if(TimerVolcado>5){
                    Vector3 correctiveImpulse = Body().Pose.Orientation.Up() * JUMP_POWER * 1.5f;
                    this.ApplyImpulse(correctiveImpulse, (Body().Pose.Orientation.Left()+Body().Pose.Orientation.Forward()) * this.Scale());
                    // MachineGun.ApplyImpulse(correctiveImpulse, (Body().Pose.Orientation.Left()+Body().Pose.Orientation.Forward()) * this.Scale());
                }
            }
        }
        MachineGun.Update(Body().Pose.Position, Body().Pose.Orientation);
    }

    //  COLISIONES
    // 
    //      OnCollision :       Si colisiona con un PowerUp, se le aplica el efecto del PowerUp.
    //                          Si colisiona con el piso, se le permite saltar.
    //                          Si colisiona con un enemigo, se le aplica el efecto del enemigo.
    internal override bool OnCollision(Elemento other)
    {
        if(other is Piso _){
            PuedeSaltar = true;
        }
        if(other is ElementoEstatico _){
            // Console.WriteLine("Toqué un elemento estático");
        }
        if(other is MachineGun _){
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
    public bool Inmune() => TimerInmune < 3;

    internal void PickPowerUp()
    {
        if(!Inmune()){
            TimerInmune = 0;
            Turbo = 1;
            DisplayEstado?.BulletAmmo.Recargar();
            MisilCargado = true;
        }
    }

    internal void HitByMachineGun(Vector3 enemyForward, float depth)
    {
            Vector3 bulletDirection = enemyForward;
            bulletDirection.Normalize();
            this.ApplyLinearImpulse(enemyForward * 250);

            var hitDamage = 0.05f;
            Vida = (Vida>=hitDamage)? Vida-hitDamage : 0;
    }

    internal override void Draw()
    {
        if(DisplayEstado != null)
            if(DisplayEstado.HasAmmo())
                MachineGun.Draw();
        base.Draw();
    }        
}
