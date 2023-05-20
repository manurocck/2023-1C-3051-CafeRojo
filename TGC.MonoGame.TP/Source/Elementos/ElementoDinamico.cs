using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Elementos
{
    public abstract class ElementoDinamico : Elemento {
        private BodyHandle BodyHandle { get; set; }
        internal IConvexShape Shape { get; set; }
        internal abstract float Mass();
        internal abstract float Scale();
        internal abstract void Update(float dTime, KeyboardState keyboard);

        private const float SIMULATION_SLEEP_THRESHOLD = 0.01f;
        private const float SIMULATION_MAXIMUN_SPECULATIVE_MARGIN = 0.1f;

        internal BodyReference Body() => TGCGame.Simulation.Bodies.GetBodyReference(BodyHandle);

        internal Quaternion Rotation() => Body().Pose.Orientation.ToQuaternion();
        internal Vector3 Position() => Body().Pose.Position;
        internal override Matrix World() => Matrix.CreateScale(Scale()) * 
                                            Matrix.CreateFromQuaternion(Rotation()) * 
                                            Matrix.CreateTranslation(Position());

        internal void ApplyAngularImpulse(Vector3 impulse) => Body().ApplyAngularImpulse(impulse.ToBepu());
        internal void ApplyLinearImpulse(Vector3 impulse) => Body().ApplyLinearImpulse(impulse.ToBepu());
        internal Vector3 AngularVelocity() => Body().Velocity.Angular;
        internal Vector3 LinearVelocity() => Body().Velocity.Linear;

        internal void AddToSimulation<TConvexShape>(Vector3 InitialPosition, Quaternion Rotation) where TConvexShape : unmanaged, IConvexShape { 
            BodyInertia Inertia = Shape.ComputeInertia(Mass());
            
            TypedIndex Index = TGCGame.Simulation.Shapes.Add((TConvexShape)Shape);

            BodyHandle = TGCGame.Simulation.Bodies.Add(BodyDescription.CreateDynamic(
                new RigidPose(InitialPosition.ToBepu(), Rotation.ToBepu() ),
                Inertia,
                new CollidableDescription(Index, SIMULATION_MAXIMUN_SPECULATIVE_MARGIN),
                new BodyActivityDescription(SIMULATION_SLEEP_THRESHOLD)));
        }
    }
}