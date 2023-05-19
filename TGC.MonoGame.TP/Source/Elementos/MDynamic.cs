using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Elementos
{
    internal interface MDynamic {
        internal BodyHandle BodyHandle { get; set; }
        internal IConvexShape Shape { set; get; }
        internal float Mass();
        internal float Scale();

        internal void Update(float dTime, KeyboardState keyboard);
    }

    internal static class MDynamicExtension {
        private const float SIMULATION_SPEEL_THRESHOLD = 0.01f;
        private const float SIMULATION_MAXIMUN_SPECULATIVE_MARGIN = 0.1f;

        internal static BodyReference Body(this MDynamic colliable) => TGCGame.Simulation.Bodies.GetBodyReference(colliable.BodyHandle);

        internal static Quaternion Rotation(this MDynamic colliable) => colliable.Body().Pose.Orientation.ToQuaternion();
        internal static Vector3 Position(this MDynamic colliable) => colliable.Body().Pose.Position;
        public static Matrix World(this MDynamic colliable)  => Matrix.CreateScale(colliable.Scale()) * 
                                                                Matrix.CreateFromQuaternion(colliable.Rotation()) * 
                                                                Matrix.CreateTranslation(colliable.Position());

        internal static void ApplyAngularImpulse(this MDynamic colliable, Vector3 impulse) => colliable.Body().ApplyAngularImpulse(impulse.ToBepu());
        internal static void ApplyLinearImpulse(this MDynamic colliable, Vector3 impulse) => colliable.Body().ApplyLinearImpulse(impulse.ToBepu());
        internal static Vector3 AngularVelocity(this MDynamic colliable) => colliable.Body().Velocity.Angular;
        internal static Vector3 LinearVelocity(this MDynamic colliable) => colliable.Body().Velocity.Linear;

        internal static void AddToSimulation<TConvexShape>(this MDynamic colliable, Vector3 InitialPosition) where TConvexShape : unmanaged, IConvexShape { 
            BodyInertia Inertia = colliable.Shape.ComputeInertia(colliable.Mass());
            TypedIndex Index = TGCGame.Simulation.Shapes.Add((TConvexShape)colliable.Shape);

            colliable.BodyHandle =  TGCGame.Simulation.Bodies.Add(BodyDescription.CreateDynamic(
                                    InitialPosition.ToBepu(),
                                    Inertia,
                                    new CollidableDescription(Index, SIMULATION_MAXIMUN_SPECULATIVE_MARGIN),
                                    new BodyActivityDescription(SIMULATION_SPEEL_THRESHOLD)));
        }
    }
}