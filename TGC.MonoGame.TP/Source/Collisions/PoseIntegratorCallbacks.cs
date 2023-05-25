using System;
using System.Numerics;
using BepuPhysics;
using BepuUtilities;

namespace PistonDerby.Collisions;

public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
{
    // Gravedad a aplicar a los Bodies de la Simulación
    public Vector3 Gravity;

    // "Damping" o "Amortiguamiento" :
    //      Fracción de velocidad (Linear o Angular) a remover por unidad de tiempo. 
    //      Valores entre 0 y 1. Siendo 0 sin fricción y 1 dejándolo casi inmóvil.
    public float LinearDamping;
    public float AngularDamping;
    private Vector3Wide GravityWideDt;
    private Vector<float> LinearDampingDt;
    private Vector<float> AngularDampingDt;

    // Determina si se debería conservar o no el "momentum angular" cuando el Pose cambia
    //      (investigar modos)
    public readonly AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

    // Determina si el PoseIntegrator debería usar substepping para cuerpos sin restricciones, usando un substepping solver.
    //      Si se setea en true, los Bodies se integraran con el mismo número de substeps que los Bodies sin restricciones.
    //      Se se setea en false, los Bodies sin restricciones van a usar un solo step de longitud igual al dt que se seteó en la Simulación.
    public readonly bool AllowSubstepsForUnconstrainedBodies => false;

    // Determina si el callback debería ser llamado para los cuerpos Kinematicos también.
    //      Si se setea en true, IntegrateVelocity va a ser llamada para los paquetes que incluyan Bodies Kinematicos
    //      Si se setea en false, los cuerpos Kinemáticos van a continuar con la velocidad que se les seteó
    //              (en la mayoría de los casos esto se setea en falso)
    public readonly bool IntegrateVelocityForKinematics => false;

    public PoseIntegratorCallbacks(Vector3 gravity, float linearDamping = .03f, float angularDamping = .03f) : this()
    {
        Gravity = gravity;
        LinearDamping = linearDamping;
        AngularDamping = angularDamping;
    }

    public void Initialize(Simulation simulation) { }

    public void PrepareForIntegration(float dt)
    {
        // No hay motivo para recalcular "gravity * dt" por cada Body; para eso calculamos el valor por adelantado.
        // Como estos callbacks no usan Damping por body, se puede precaluclar todo
        LinearDampingDt = new Vector<float>(MathF.Pow(MathHelper.Clamp(1 - LinearDamping, 0, 1), dt));
        AngularDampingDt = new Vector<float>(MathF.Pow(MathHelper.Clamp(1 - AngularDamping, 0, 1), dt));
        GravityWideDt = Vector3Wide.Broadcast(Gravity * dt);
    }

    // Callback de un conjunto de Bodies siendo integrados.
    public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation,
        BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt,
        ref BodyVelocityWide velocity)
    {
        /* 
            Este es un spot útil para implementar cosas como Gravedad dependiente de la posición o amortiguamiento por cuerpo.
            Esta implementación utiliza un único valor de amortiguamiento para todos los cuerpos que permite que sea precalculada. 
            
                No tenemos que chequear por kinematicos porque IntegrateVelocityForKinematics es false
                Así que nunca los vamos a devolver en este callback

            Nota :
                Algunos tipos están ordenados en "array-of-structures-of-arrays" (AOSOA) como Vector3Wide o QuaternionWide
                por un criterio de performance.

        */
        velocity.Linear = (velocity.Linear + GravityWideDt) * LinearDampingDt;
        velocity.Angular *= AngularDampingDt;
    }
}