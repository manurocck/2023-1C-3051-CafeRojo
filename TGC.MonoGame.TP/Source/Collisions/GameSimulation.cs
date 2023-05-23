using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Collisions;

internal class GameSimulation
{
    private const float SIMULATION_SLEEP_THRESHOLD = 0.01f;
    private const float SIMULATION_MAXIMUN_SPECULATIVE_MARGIN = 0.1f;
    private const float TIME_STEP = 1 / 60f;
    private readonly Simulation Simulation;
    private readonly BufferPool BufferPool = new BufferPool();
    private readonly SimpleThreadDispatcher ThreadDispatcher;
    private readonly Vector3 Gravity = new Vector3(0, -150f, 0);
    internal readonly Colliders Colliders = new Colliders();

    internal GameSimulation()
    {
        this.ThreadDispatcher = new SimpleThreadDispatcher(ThreadCount());
        this.Simulation = CreateSimulation();
    }

    private int ThreadCount() => Math.Max(1, Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
    private Simulation CreateSimulation() => Simulation.Create(
        BufferPool, new NarrowPhaseCallbacks(new SpringSettings(30, 1)),
        new PoseIntegratorCallbacks(Gravity.ToBepu(), 0f, 0f), new SolveDescription(8, 1)
    );

    internal void Update() => Simulation.Timestep(TIME_STEP, ThreadDispatcher);
     
    internal TypedIndex LoadShape<S>(S shape) where S : unmanaged, IShape => Simulation.Shapes.Add(shape);

    internal BodyReference GetBodyReference(BodyHandle handle) => Simulation.Bodies.GetBodyReference(handle);
    internal void Awake(BodyHandle handle) => Simulation.Awakener.AwakenBody(handle);

    internal StaticHandle CreateStatic(Vector3 position, Quaternion rotation, TypedIndex shape) =>
        Simulation.Statics.Add(new StaticDescription(position.ToBepu(), rotation.ToBepu(), shape));

    internal BodyHandle CreateDynamic(Vector3 position, Quaternion rotation, TypedIndex shape, float mass)
    {
        BodyInertia inertia = this.Simulation.Shapes.GetShape<Box>(shape.Index).ComputeInertia(mass);

        BodyDescription bodyDescription = BodyDescription.CreateDynamic(
            new RigidPose(position.ToBepu(),  rotation.ToBepu()),
            new BodyVelocity(Vector3.Zero.ToBepu()),
            inertia,
            new CollidableDescription(shape, SIMULATION_MAXIMUN_SPECULATIVE_MARGIN),
            new BodyActivityDescription(SIMULATION_SLEEP_THRESHOLD));
        return Simulation.Bodies.Add(bodyDescription);
    }

    internal void DestroyStatic(StaticHandle handle) => Simulation.Statics.Remove(handle);
    internal void DestroyBody(BodyHandle handle) => Simulation.Bodies.Remove(handle);

    internal void Dispose()
    {
        Simulation.Dispose();
        BufferPool.Clear();
        ThreadDispatcher.Dispose();
    }
}
