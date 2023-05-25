using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Utils;

namespace PistonDerby.Elementos;
public abstract class ElementoDinamico : Elemento {
    protected BodyHandle BodyHandle { get; set; }
    internal TypedIndex Shape { get; set; }
    internal abstract float Mass();
    internal abstract float Scale();
    internal virtual void Update(float dTime, KeyboardState keyboard) { }
    internal BodyReference Body() => PistonDerby.Simulation.GetBodyReference(BodyHandle);

    internal Quaternion Rotation() => Body().Pose.Orientation.ToQuaternion();
    internal Vector3 Position() => Body().Pose.Position;
    internal override Matrix World() => Matrix.CreateScale(Scale()) * 
                                        Matrix.CreateFromQuaternion(Rotation()) * 
                                        Matrix.CreateTranslation(Position());

    internal void ApplyAngularImpulse(Vector3 impulse) => Body().ApplyAngularImpulse(impulse.ToBepu());
    internal void ApplyLinearImpulse(Vector3 impulse, float offset = 0) => 
        Body().ApplyImpulse(impulse.ToBepu(), QuaternionExtensions.Forward((this.Body().Pose.Orientation.ToQuaternion())*offset).ToBepu());
    internal void Awake() => PistonDerby.Simulation.Awake(BodyHandle);
    internal Vector3 AngularVelocity() => Body().Velocity.Angular;
    internal Vector3 LinearVelocity() => Body().Velocity.Linear;

    internal void AddToSimulation(Vector3 initialPosition, Quaternion initialRotation) { 
        BodyHandle = PistonDerby.Simulation.CreateDynamic(initialPosition, initialRotation, Shape, Mass());
        PistonDerby.Simulation.Colliders.RegisterCollider(BodyHandle, this);
    }
}