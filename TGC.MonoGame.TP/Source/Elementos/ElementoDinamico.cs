using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Elementos
{
    public abstract class ElementoDinamico : Elemento {
        protected BodyHandle BodyHandle { get; set; }
        internal TypedIndex Shape { get; set; }
        internal abstract float Mass();
        internal abstract float Scale();
        internal virtual void Update(float dTime, KeyboardState keyboard) { }

        private const float SIMULATION_SLEEP_THRESHOLD = 0.01f;
        private const float SIMULATION_MAXIMUN_SPECULATIVE_MARGIN = 0.1f;

        internal BodyReference Body() => TGCGame.Simulation.GetBodyReference(BodyHandle);

        internal Quaternion Rotation() => Body().Pose.Orientation.ToQuaternion();
        internal Vector3 Position() => Body().Pose.Position;
        internal override Matrix World() => Matrix.CreateScale(Scale()) * 
                                            Matrix.CreateFromQuaternion(Rotation()) * 
                                            Matrix.CreateTranslation(Position());

        internal void ApplyAngularImpulse(Vector3 impulse) => Body().ApplyAngularImpulse(impulse.ToBepu());
        internal void ApplyLinearImpulse(Vector3 impulse) => Body().ApplyLinearImpulse(impulse.ToBepu());
        internal void ApplyLinearImpulse(Vector3 impulse, float offset = 0) => 
            Body().ApplyImpulse(impulse.ToBepu(), QuaternionExtensions.Forward((this.Body().Pose.Orientation.ToQuaternion())*offset).ToBepu());
        internal void Awake() => TGCGame.Simulation.Awake(BodyHandle);
        internal Vector3 AngularVelocity() => Body().Velocity.Angular;
        internal Vector3 LinearVelocity() => Body().Velocity.Linear;

        internal void AddToSimulation(Vector3 initialPosition, Quaternion initialRotation) { 
            BodyHandle = TGCGame.Simulation.CreateDynamic(initialPosition, initialRotation, Shape, Mass());
            TGCGame.Simulation.Colliders.RegisterCollider(BodyHandle, this);
        }
    }
}