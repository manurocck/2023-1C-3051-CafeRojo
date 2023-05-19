using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;
using TGC.MonoGame.TP.Collisions;
using BepuVector3    = System.Numerics.Vector3;
using BepuPhysics;
using BepuPhysics.Collidables;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP
{
    public class Auto : IElementoDinamico
    { 
        private const float WHEEL_TURNING_LIMIT = 0.5f;
        private const float ANGULAR_SPEED = 900f;
        private const float LINEAR_SPEED = 80f;
        private const float AUTO_SCALE = 0.08f * TGCGame.S_METRO;
        private const float SIMU_BOX_SCALE = 0.010f*AUTO_SCALE;
        private const float WHEEL_ROTATION_FACTOR = 0.000008f; // Factor de ajuste para la rotación
        private const float JUMP_POWER = 1000f; // Factor de ajuste para la rotación
        private bool puedeSaltar = true;
        private BodyHandle Handle;
        private Vector3 Position;
        private float WheelRotation; // Tal vez pueda usarse también como rotación del auto
        private float WheelTurning = 0f;
        private float Turbo = 1000;

        private float Vel_Turbo = 1f;

        public Auto(Vector3 posicionInicial, float escala = AUTO_SCALE) 
        : base(TGCGame.GameContent.M_Auto, Vector3.Zero, Vector3.Zero, escala)
        {
            Position = posicionInicial + new Vector3(0,TGCGame.S_METRO,0);
            Escala = escala;
            this.SetEffect(TGCGame.GameContent.E_SpiralShader);
                
            var boxSize = Model.Size()*SIMU_BOX_SCALE;
            var boxShape = new Box(boxSize.X,boxSize.Y,boxSize.Z); // a chequear
            var boxInertia = boxShape.ComputeInertia(3);
            var boxIndex = TGCGame.Simulation.Shapes.Add(boxShape);

            Handle = TGCGame.Simulation.Bodies.Add(BodyDescription.CreateDynamic(
                            new BepuVector3(Position.X,Position.Y,Position.Z),
                            boxInertia,
                            new CollidableDescription(boxIndex, 0.1f),
                            new BodyActivityDescription(0.01f)));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {   
            // Seteo inicial
            var linearImpulse = Vector3.Zero;
            var angularImpulse = Vector3.Zero;
            var simuWorld = TGCGame.Simulation.Bodies.GetBodyReference(Handle);
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            Vel_Turbo = 1f;
            var pressedKeys = keyboardState.GetPressedKeys();
            if(pressedKeys.Length>0) simuWorld.Awake = true;

            foreach(var key in pressedKeys){
                switch(key){
                    case Keys.A:
                        WheelTurning = (WheelTurning<WHEEL_TURNING_LIMIT)? // Qué no gire de más
                                                                    WheelTurning+WHEEL_TURNING_LIMIT*dTime*4f
                                                                  : WHEEL_TURNING_LIMIT; 

                    break;
                    case Keys.D:
                        
                        WheelTurning = (WheelTurning>(-1)*WHEEL_TURNING_LIMIT)? // Qué no gire de más
                                                                   WheelTurning-WHEEL_TURNING_LIMIT*dTime*4f
                                                                 : (-1)*WHEEL_TURNING_LIMIT;
                    break;
                    case Keys.S:
                        linearImpulse = simuWorld.Pose.Orientation.Forward() * (-LINEAR_SPEED);
                    break;
                    case Keys.W:
                        linearImpulse = simuWorld.Pose.Orientation.Forward() * (LINEAR_SPEED);
                    break;
                    case Keys.Space:
                         if(puedeSaltar){
                             linearImpulse = simuWorld.Pose.Orientation.Up() * JUMP_POWER;
                             puedeSaltar = false;
                         }
                    break;
                    case Keys.LeftShift:
                        if(Turbo > 0){
                            Vel_Turbo = 1.5f;
                            Turbo--;
                        }
                    break;
                }
            }
            
            var velocidadActual = simuWorld.Velocity.Linear.ToVector3();
            var coeficienteVelocidad = (Math.Abs(velocidadActual.X) + Math.Abs(velocidadActual.Y) + Math.Abs(velocidadActual.Z)) / 2000;
            Console.WriteLine("Velocidad alcanzada :    . . . . . . {0:F}%", (coeficienteVelocidad * 100f));

            // Giro
            angularImpulse = new Vector3(0,ANGULAR_SPEED,0)*WheelTurning*Math.Min(coeficienteVelocidad*4, 1);

            // Rotación de ruedas
            WheelTurning = WheelTurning > 0 ? WheelTurning - WHEEL_TURNING_LIMIT*dTime : WheelTurning + WHEEL_TURNING_LIMIT*dTime;
            WheelRotation += simuWorld.Velocity.Angular.Y * WHEEL_ROTATION_FACTOR;

            // Aplicar impulsos
            simuWorld.ApplyImpulse(linearImpulse.ToBepu() * Vel_Turbo, (simuWorld.Pose.Orientation.Forward() * 2).ToBepu());
            simuWorld.ApplyAngularImpulse(angularImpulse.ToBepu());


            // Drawable World
            Position = simuWorld.Pose.Position;
            var quaternion = simuWorld.Pose.Orientation;
            
            World =
                Matrix.CreateScale(Escala) *
                Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)) * 
                Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));
        }   

        public override void Draw(){
            var simuWorld = TGCGame.Simulation.Bodies.GetBodyReference(Handle);
            
            var aabb = simuWorld.BoundingBox;
            
            TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);
            
            var quaternion = simuWorld.Pose.Orientation;
            var worldAux = Matrix.Identity;

            // acá se están dibujando las ruedas una vez. sacarlas del dibujado.
            foreach(var bone in Model.Bones){
                switch(bone.Name){
                    case "Car":
                        foreach(var mesh in bone.Meshes){  
                            foreach(var meshPart in mesh.MeshParts){
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            mesh.Draw();
                        }
                    break;
                    case "WheelA": // Adelante derecha
                    case "WheelB": // Adelante izquierda
                        worldAux = 
                                    Matrix.CreateScale(1.5f)
                                    * Matrix.CreateRotationY(WheelTurning)                        // giro del volante
                                    * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                    * Matrix.CreateRotationY(WheelRotation)                     // giro con el auto
                                    * World
                                    ;
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Rotación extra -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                meshPart.Effect.Parameters["World"]?.SetValue(worldAux);
                            }
                            mesh.Draw();
                        }
                    break;
                    case "WheelC": // Atras izquierda
                    case "WheelD": // Atras derecha
                        worldAux = 
                                    Matrix.CreateScale(2f)
                                    * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                    * Matrix.CreateRotationY(WheelRotation)                   // giro con el auto
                                    * World 
                                    ;
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                meshPart.Effect.Parameters["World"]?.SetValue(worldAux);
                            }
                            mesh.Draw();
                        }
                    break;
                }
            }
        }
    }

}
    