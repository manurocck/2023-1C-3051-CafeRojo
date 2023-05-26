using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BepuPhysics.Collidables;
using PistonDerby.Utils;
using PistonDerby.Elementos;
using PistonDerby.Drawers;

namespace PistonDerby.Autos;
internal class Auto : ElementoDinamico { 
    private const float WHEEL_TURNING_LIMIT = 0.5f;
    private const float ANGULAR_SPEED = 1.8f * PistonDerby.S_METRO;
    private const float LINEAR_SPEED = 0.12f * PistonDerby.S_METRO;
    private const float JUMP_POWER = 1f * PistonDerby.S_METRO;
    private const float WHEEL_ROTATION_FACTOR = 0.000008f; // Factor de ajuste para la rotación (setead para S_METRO = 250f) puede fallar (o no) si se cambia
    internal override IDrawer Drawer() => new CarDrawer(this);
    internal override float Scale() => 0.08f * PistonDerby.S_METRO;
    internal override float Mass() => 1f;
    private bool PuedeSaltar = true;
    internal float WheelTurning = 0f; 
    internal float WheelRotation = 0f;
    internal BoundingBox shadowBaseBox;

    internal Auto(Vector3 posicionInicial) {
        var boxSize = PistonDerby.GameContent.M_Auto.Dimensiones() * 0.010f * this.Scale(); //SIMU_BOX_SCALE Que va a ir a Content
        Shape = PistonDerby.Simulation.LoadShape<Box>(new Box(boxSize.X,boxSize.Y,boxSize.Z));
        this.AddToSimulation(posicionInicial + new Vector3(0,PistonDerby.S_METRO,0), Quaternion.Identity);

        shadowBaseBox = new BoundingBox(this.Body().BoundingBox.Min,this.Body().BoundingBox.Max);
    }

    internal override void Update(float dTime, KeyboardState keyboard) {

        if(keyboard.GetPressedKeyCount()>0) this.Awake();

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
        Vector3 horizontalImpulse = (PuedeSaltar)? this.Rotation().Forward() : velocidadActual.XZFoward();
        horizontalImpulse *= keyboard.AccelerationSense() * LINEAR_SPEED ;
        float offsetAmount = 2f; //habría que generalizarlo para ubicar exáctamente en donde están las ruedas o un poquito más adelante
        this.ApplyLinearImpulse(horizontalImpulse, offsetAmount);
        //
        //      VerticalImpulse :   A desarrollar.
        //
        if(PuedeSaltar && keyboard.Jumped())
        {
            Vector3 verticalImpulse = Vector3.UnitY * JUMP_POWER ;
            this.ApplyLinearImpulse(verticalImpulse);
            PuedeSaltar = false;
        } 
    }
    internal override bool OnCollision(Elemento other)
    {
        if(other is AutoEnemigo _){
            Console.WriteLine("Toqué un enemy car");
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
            
        // // DEBUG
        // if(sombraAcual.Intersects(this.Body().BoundingBox.ToBoundingBox()))
        //     Console.WriteLine("Toqué el piso");
                    
        return sombraAcual.Intersects(this.Body().BoundingBox.ToBoundingBox());
    }
    public void DebugGizmos()
    {
        BoundingBox aabb = this.Body().BoundingBox.ToBoundingBox(); 
        PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Gold);
        

        BoundingBox sombraAcual = new BoundingBox(this.Body().BoundingBox.Min, this.Body().BoundingBox.Max);
        float alturaBoxSombra = sombraAcual.Max.Y - sombraAcual.Min.Y;
        
        sombraAcual.Min.Y = -alturaBoxSombra*0.5f;
        sombraAcual.Max.Y = -alturaBoxSombra*0.5f;
        
        aabb = sombraAcual; 
        PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Magenta);
    }
}
