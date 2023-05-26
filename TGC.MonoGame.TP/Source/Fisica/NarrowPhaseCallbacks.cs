
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using PistonDerby.Elementos;

namespace PistonDerby.Collisions;

public struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
{
    private SpringSettings ContactSpringiness { get; set; }
    private float MaximumRecoveryVelocity { get; set; }
    private float FrictionCoefficient { get; set; }

    public NarrowPhaseCallbacks(SpringSettings contactSpringiness) : this(contactSpringiness, 2f, 1f){}
    public NarrowPhaseCallbacks(SpringSettings contactSpringiness, float maximumRecoveryVelocity,
        float frictionCoefficient)
    {
        ContactSpringiness = contactSpringiness;
        MaximumRecoveryVelocity = maximumRecoveryVelocity;
        FrictionCoefficient = frictionCoefficient;
    }
    public void Initialize(Simulation simulation)
    {
        // Define por default el SpringSettings si no se definió
        if (ContactSpringiness.AngularFrequency == 0 && ContactSpringiness.TwiceDampingRatio == 0)
        {
            ContactSpringiness = new SpringSettings(30, 1);
            MaximumRecoveryVelocity = 2f;
            FrictionCoefficient = 1f;
        }
    }

    // Resumen 
    //      Permite o no la resolución del contacto proviniente de dos collisionables que se intersectan
    //
    // Parámetros
    //      workerIndex         : Índice del hilo que identificó la intersección del par.
    //      a                   : Referencia al primer Collidable en el par.
    //      b                   : Referencia al segundo Collidable en el par.
    //      speculativeMargin   : Referencia del margen especulativo usado por el par. 
    //                              Por default se inicializa por la NarrowPhase examinando los márgenes de cada par (pero puede modificarse)
    //
    // Return 
    //      True si la detección de la colisión debería resolverse, False si no debería (se atraviesan)
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
                pero como los pares Kinematic-Kinematic no pueden generar resticciones (tienen masa infinita),
                simulaciones simples pueden ignorar estos pares.
            
            La mayor parte de las veces se puede ignorar el parámetro speculativeMargin
        */
        return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
    }

    // Resumen 
    //      Permite o no la resolución del contacto proviniente de dos collisionables que se intersectan
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

    // Diccionario
    //      "Manifold" :    El Manifold tiene la información de los puntos de contacto entre los pares, 
    //                      la normal de la colisión y la fuerza o impulso necesario para corregir la colisión
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

        pairMaterial.FrictionCoefficient = FrictionCoefficient;
        pairMaterial.MaximumRecoveryVelocity = MaximumRecoveryVelocity;
        pairMaterial.SpringSettings = ContactSpringiness;

        Elemento elementoA = GetCollitionHandler(pair.A);
        Elemento elementoB = GetCollitionHandler(pair.B);
         
        bool handler = elementoA.OnCollision(elementoB);
        elementoB?.OnCollision(elementoA);

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

        return handler;
    }

    private Elemento GetCollitionHandler(CollidableReference collider) =>
            collider.Mobility == CollidableMobility.Static ?
                PistonDerby.Simulation.Colliders.GetHandler(collider.StaticHandle) :
                PistonDerby.Simulation.Colliders.GetHandler(collider.BodyHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB,
        ref ConvexContactManifold manifold)
    {
        return true;
    }

    public void Dispose()
    {
        //Something to be dispose.
    }
}