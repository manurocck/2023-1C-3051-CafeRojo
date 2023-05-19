using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Colliders
{
    internal interface MCollidable {
        internal float Scale { get; set; } 
        internal BodyHandle BodyHandle { get; set; }
        internal IShape Shape { get; set; }
        //internal void Update();
    }

    internal static class MCollidableExtension {
        internal static BodyReference Body(this MCollidable colliable) => TGCGame.Simulation.Bodies.GetBodyReference(colliable.BodyHandle);

        internal static Quaternion Rotation(this MCollidable colliable) => colliable.Body().Pose.Orientation.ToQuaternion();
        internal static Vector3 Position(this MCollidable colliable) => colliable.Body().Pose.Position;

        internal static Matrix World(this MCollidable colliable)  =>    Matrix.CreateScale(colliable.Scale) * 
                                                                        Matrix.CreateFromQuaternion(colliable.Rotation()) * 
                                                                        Matrix.CreateTranslation(colliable.Position());
        internal static void Rotate(this MCollidable colliable, Quaternion rotation) {
            BodyReference Body = colliable.Body();
            Body.Pose.Orientation = Body.Pose.Orientation * rotation.ToBepu();
        }
        
        internal static void AddVelocity(this MCollidable colliable, Vector3 dVelocity) {
            BodyReference Body = colliable.Body();
            Body.Velocity.Linear += dVelocity.ToBepu();
        }
        internal static void SetVelocity(this MCollidable colliable, Vector3 newVelocity) {
            BodyReference Body = colliable.Body();
            Body.Velocity.Linear += newVelocity.ToBepu();
        }

        /*internal static void AddToSimulation(this MCollidable colliable) {
            //var boxSize = TGCGame.GameContent.M_Auto.Size() / AUTO_SCALE * 1.5f;
            //var boxShape = new Box(boxSize.X,boxSize.Y,boxSize.Z); // a chequear
            
            var boxInertia = colliable.Shape.ComputeInertia(1);
            var boxIndex = TGCGame.Simulation.Shapes.Add(boxShape);

            colliable.BodyHandle =  TGCGame.Simulation.Bodies.Add(BodyDescription.CreateDynamic(
                                    new BepuVector3(Posicion.X,Posicion.Y,Posicion.Z),
                                    boxInertia,
                                    new CollidableDescription(boxIndex, 0.1f),
                                    new BodyActivityDescription(0.01f)));
        }*/
    }
}