using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;

namespace TGC.MonoGame.TP.Collisions;

// Muestra un ejemplo de uso de BepuPhysics completamente aislado de otros archivos 

public static class SimpleSelfContainedDemo
{
    
    /*                        * * *
        < < <  Ejemplo de inicialización de Simulación > > > 
     */
    public static void Run()
    {
        // El buffer pool es una fuente de grandes espacios vacíos de memoria para que use el motor.
        var bufferPool = new BufferPool();
        
        // Lo siguiente setea la simulacion con los callbacks definidos más abajo
        var simulation = Simulation.Create(
                            bufferPool, 
                            new NarrowPhaseCallbacks(),
                            new PoseIntegratorCallbacks(
                                new Vector3(0, -10, 0)), 
                                // Le dice que use 8 iteraciones de velocidad 'per substep' y solo una solucion 'per step'.
                                new SolveDescription(8, 1) 
                            );

        // Agrega una esfera grande que se va a mover y la agrega a la simulación.
        var sphere = new Sphere(1);
        var sphereInertia = sphere.ComputeInertia(1);
        simulation.Bodies.Add(
            BodyDescription.CreateDynamic(
                new Vector3(0, 5, 0), 
                sphereInertia,
                simulation.Shapes.Add(sphere), 
                0.01f // Después de cuánto se "duerme"
                )
            );
        
        // Agrega una caja que va a contener la esfera
        simulation.Statics.Add(
            new StaticDescription(
                new Vector3(0, 0, 0),
                simulation.Shapes.Add(
                    new Box(500, 1, 500))
                )
            );

        // Para procesarla con varios hilos (si no queremos hilos múltiples se puede evitar este paso)
        var threadDispatcher = new ThreadDispatcher(Environment.ProcessorCount);

        // Ahora realiza 100 saltos en el tiempo !
        for (var i = 0; i < 100; ++i)
        {
            // Note that each timestep is 0.01 units in duration, so all 100 time steps will last 1 unit of time.
            // (Usually, units of time are defined to be seconds, but the engine has no preconceived notions about units. All it sees are the numbers.)
            simulation.Timestep(0.01f, threadDispatcher);
        }

        // Si se planea reutilizar la BufferPool, hacer un dispose de la simulación es una buena idea (limpia todas las BufferPool para ser reutilizadas).
        // Si se realiza mal el dispose, puede resultar en memory leaks (memoria perdida en un proceso fantasma).
        simulation.Dispose();
        threadDispatcher.Dispose();
        bufferPool.Clear();
    }

    /*                             * * *
        < < <  Cómo se resuelven los Callbacks en la NarrowPhase > > > 
     */
    public struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
    {
        // Resumen
        //      Realiza cualquier inicialización requerida después de que la instancia de Simulation fue construída 
        //
        // Parámetro
        //      simulation : La Simulation que 'posee' este Callback.
        //
        public void Initialize(Simulation simulation)
        {
            // A veces, los Callbacks son creados después de que la instancia de Simulación es creada
            // así que la simulación llamaría a esta función cuando esté lista.

            // Cualquier lógica que dependa de que la simulación exista primero puede ser puesta acá.
        }

        // Resumen 
        //      Elige si permitir o no la resolución del contacto proviniente de dos collisionables que se intersectan
        //
        // Parámetros
        //      workerIndex         : Índice del hilo que identificó la intersección del par.
        //      a                   : Referencia al primer Collidable en el par.
        //      b                   : Referencia al segundo Collidable en el par.
        //      speculativeMargin   : Referencia del margen especulativo usado por el par. 
        //                              Por default se inicializa por la NarrowPhase examinando los márgenes de cada par (pero puede modificarse)
        //
        // Return 
        //      True si la detección de la colisión debería resolverse, False si no debería.
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b,
            ref float speculativeMargin)
        {   
            /* 
                Puede ser utilizada para implementar filtros en las colisiones (por ejemplo con los PowerUps)
                Acá solo nos aseguramos que al menos uno de los dos Collidables sea dinámico (un body)

                El motor nunca va a crear un par Static-Static, pero si va a generar pares Kinematic-Kinematic
                Puede ser útil si queremos hacer algún tipo de sensor/disparador
                pero como los pares Kinematic-Kinematic no pueden generar resticciones (tienen masa infinita)
                simulaciones simples pueden ignorar estos pares.
                
                La mayor parte de las veces se puede ignorar el parámetro speculativeMargin
             */
            return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
        }

        // Resumen 
        //      Elige si permitir o no la resolución del contacto proviniente de dos collisionables que se intersectan
        //
        // Parámetros
        //      workerIndex : Índice del hilo que detectó la colisión del par.
        //      pair        : Par de los dos Collidables en cuestión.
        //      childIndexA : Índice del Collidable A en el par. 
        //      childIndexB : Índice del Collidable B en el par.
        // 
        // Return 
        //      True si la detección de la colisión debería resolverse, False si no debería.
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
        {
            /* 
                Esta función solo se llama en pares que involucren al menos 
                una Shape que contenga múltples hijos (Como un Compound de Shapes)
             */
            return true;
        }

        // Resumen
        //     Provee una notificación de que un Manifold fue creado para un par
        //     Da la oportunidad para modificar los detalles del Manifold
        // 
        // Parámetros
        //     workerIndex  : Índice del hilo que creó este Manifold.
        //     pair         : Set de contactos detectados entre los Collidables.
        //     pairMaterial : Propiedades del los materiales del Manifold.
        // 
        // Return
        //     True si debería aplicarse las restricciones para este Manifold
        //     False si no debrería
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold,
            out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
        {
            /* 
                IContactManifold incluye funciones para acceder a los datos de contactos sin importar qué tipo de Manifold es.
                    Si querés tener acceso directo al Tipo de manifold, podes usar la propiedad manifold.Convex y
                    un cast como Unsafe.As<TManifold, ConvexContactManifold or NonconvexContactManifold>(ref manifold).
                
                El motor no define un material como atributo de los Bodies. 
                Así que toda referencia a los materiales son manejadas por los callbacks.

                Para los propósitos de la demo, se van a usar las mismas configuraciones para todos los pares
                
                Nota:   
                        No hay ninguna propiedad de "Rebote" o de "Coeficiente de restitucion")
                        El rebote es manejado a través de SpringSettings. 
                        Para mas detalles del SpringSetting: https://github.com/bepu/bepuphysics2/issues/3 (Chequear BouncinessDemo para ver algunas opciones)
             */
            pairMaterial.FrictionCoefficient = 1f;
            pairMaterial.MaximumRecoveryVelocity = 2f;
            pairMaterial.SpringSettings = new SpringSettings(30, 1);

            // Para los propósitos de la demo, los contactos siempre se generan.
            return true;
        }

        // Resumen
        //      Provee una notificación de que un Manifold fue creado entre dos hijos de dos Collidables en un 
        //      par que está incluido en un compound
        //      Ofrece la oportunidad de alterar los detalles del Manifold
        //
        // Parámetros
        //      workerIndex  : Índice del hilo que creó este Manifold
        //      pair         : Set de contactos detectados entre los Collidables.
        //      childIndexA  : Índice del Collidable A en el par (siempre =0 si no es un Compound)
        //      childIndexB  : Índice del Collidable B en el par (siempre =0 si no es un Compound)
        //      manifold     : Set de contactos detectados entre los Collidables
        //
        // Return
        //     True si debería aplicarse las restricciones para este Manifold
        //     False si no debrería
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
        {
            return true;
        }

        // Resumen
        //     Se deshace de cualquier recurso retenido por los Callbacks
        //     Llamada por la NarrowPhase cuando realiza su Dispose
        public void Dispose()
        {
        }
    }

    //Note that the engine does not require any particular form of gravity- it, like all the contact callbacks, is managed by a callback.
    public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        public Vector3 Gravity;

        //Note that velocity integration uses "wide" types. These are array-of-struct-of-arrays types that use SIMD accelerated types underneath.
        //Rather than handling a single body at a time, the callback handles up to Vector<float>.Count bodies simultaneously.
        private Vector3Wide gravityWideDt;

        /// <summary>
        ///     Gets how the pose integrator should handle angular velocity integration.
        /// </summary>
        public AngularIntegrationMode AngularIntegrationMode =>
            AngularIntegrationMode.Nonconserving; //Don't care about fidelity in this demo!

        /// <summary>
        ///     Gets whether the integrator should use substepping for unconstrained bodies when using a substepping solver.
        ///     If true, unconstrained bodies will be integrated with the same number of substeps as the constrained bodies in the
        ///     solver.
        ///     If false, unconstrained bodies use a single step of length equal to the dt provided to Simulation.Timestep.
        /// </summary>
        public readonly bool AllowSubstepsForUnconstrainedBodies => false;

        /// <summary>
        ///     Gets whether the velocity integration callback should be called for kinematic bodies.
        ///     If true, IntegrateVelocity will be called for bundles including kinematic bodies.
        ///     If false, kinematic bodies will just continue using whatever velocity they have set.
        ///     Most use cases should set this to false.
        /// </summary>
        public readonly bool IntegrateVelocityForKinematics => false;

        public PoseIntegratorCallbacks(Vector3 gravity) : this()
        {
            Gravity = gravity;
        }

        /// <summary>
        ///     Performs any required initialization logic after the Simulation instance has been constructed.
        /// </summary>
        /// <param name="simulation">Simulation that owns these callbacks.</param>
        public void Initialize(Simulation simulation)
        {
            //In this demo, we don't need to initialize anything.
            //If you had a simulation with per body gravity stored in a CollidableProperty<T> or something similar, having the simulation provided in a callback can be helpful.
        }

        /// <summary>
        ///     Callback invoked ahead of dispatches that may call into <see cref="IntegrateVelocity" />.
        ///     It may be called more than once with different values over a frame. For example, when performing bounding box
        ///     prediction, velocity is integrated with a full frame time step duration.
        ///     During substepped solves, integration is split into substepCount steps, each with fullFrameDuration / substepCount
        ///     duration.
        ///     The final integration pass for unconstrained bodies may be either fullFrameDuration or fullFrameDuration /
        ///     substepCount, depending on the value of AllowSubstepsForUnconstrainedBodies.
        /// </summary>
        /// <param name="dt">Current integration time step duration.</param>
        /// <remarks>This is typically used for precomputing anything expensive that will be used across velocity integration.</remarks>
        public void PrepareForIntegration(float dt)
        {
            //No reason to recalculate gravity * dt for every body; just cache it ahead of time.
            gravityWideDt = Vector3Wide.Broadcast(Gravity * dt);
        }

        /// <summary>
        ///     Callback for a bundle of bodies being integrated.
        /// </summary> <param name="bodyIndices">Indices of the bodies being integrated in this bundle.</param> <param name="position">Current body positions.</param> <param name="orientation">Current body orientations.</param> <param name="localInertia">Body's current local inertia.</param> <param name="integrationMask"> Mask indicating which lanes are active in the bundle. Active lanes will contain 0xFFFFFFFF, inactive lanes will contain 0. </param> <param name="workerIndex">Index of the worker thread processing this bundle.</param> <param name="dt">Durations to integrate the velocity over. Can vary over lanes.</param> <param name="velocity"> Velocity of bodies in the bundle. Any changes to lanes which are not active by the integrationMask will be discarded.</param>
        public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation,
            BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt,
            ref BodyVelocityWide velocity)
        {
            //This also is a handy spot to implement things like position dependent gravity or per-body damping.
            //We don't have to check for kinematics; IntegrateVelocityForKinematics returns false in this type, so we'll never see them in this callback.
            //Note that these are SIMD operations and "Wide" types. There are Vector<float>.Count lanes of execution being evaluated simultaneously.
            //The types are laid out in array-of-structures-of-arrays (AOSOA) format. That's because this function is frequently called from vectorized contexts within the solver.
            //Transforming to "array of structures" (AOS) format for the callback and then back to AOSOA would involve a lot of overhead, so instead the callback works on the AOSOA representation directly.
            velocity.Linear += gravityWideDt;
        }
    }
}