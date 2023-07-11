
using System.Globalization;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Drawers;
using PistonDerby.Utils;

namespace PistonDerby.Elementos;
public class ElementoEstatico : Elemento {
    protected List<StaticHandle> StaticHandlers { get; private set; }
    internal List<TypedIndex> Shapes { get; set; }
    internal override Model Model => SettedModel;
    internal readonly Model SettedModel;
    private IDrawer SavedDrawer;
    private Matrix WorldMatrix;

    internal override IDrawer Drawer => SavedDrawer;
    internal override Matrix World => WorldMatrix;

    internal ElementoEstatico(Vector3 CorrimientoCaja, Box Caja, Model Model, IDrawer Drawer, Vector3 Position, Vector3 Rotation, float Scale = 1, Collisions.ShapeType shapeType = Collisions.ShapeType.BOX) {
        this.SettedModel = Model;
        this.SavedDrawer = Drawer;
        Matrix rotacion =  Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z); 
        WorldMatrix  =  rotacion * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

        if(Model is null) return; // Fixea Elementos Estaticos Hechos con Geometria, hay que adaptar eso.

        // Shape = PistonDerby.Simulation.LoadShape(Collisions.ShapeType.BOX, Model, Scale);
        // CajaElemento = Caja;

        StaticHandlers = new List<StaticHandle>();
        Shapes = new List<TypedIndex>();
        Shapes.Add(PistonDerby.Simulation.LoadShape<Box>(Caja));

        this.AddToSimulation(Position+CorrimientoCaja, Quaternion.CreateFromRotationMatrix(rotacion));
    }
    internal ElementoEstatico(List<Vector3> CorrimientoCajas, List<Box> Cajas, Model Model, IDrawer Drawer, Vector3 Position, Vector3 Rotation, float Scale = 1, Collisions.ShapeType shapeType = Collisions.ShapeType.BOX) {
        this.SettedModel = Model;
        this.SavedDrawer = Drawer;
        Matrix rotacion =  Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z); 
        WorldMatrix  =  rotacion * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

        if(Model is null) return; // Fixea Elementos Estaticos Hechos con Geometria, hay que adaptar eso.
        
        StaticHandlers = new List<StaticHandle>();
        Shapes = new List<TypedIndex>();
        int i = 0;
        foreach (Box caja in Cajas)
        {
            Shapes.Add(PistonDerby.Simulation.LoadShape<Box>(caja));
            this.AddToSimulation(Position+CorrimientoCajas[i], Quaternion.CreateFromRotationMatrix(rotacion));
            i++;
        }
    }

    internal List<StaticReference> Static() {
        List<StaticReference> statics = new List<StaticReference>();
        foreach(StaticHandle StaticHandle in StaticHandlers) 
            statics.Add(PistonDerby.Simulation.GetStaticReference(StaticHandle));
        return statics;
    }

    protected override void DebugGizmos()
    {
        List<StaticReference> statics = Static();
        foreach(StaticReference st in statics){
            BoundingBox aabb = st.BoundingBox.ToBoundingBox(); 
            PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Gold);
            
            BoundingBox sombraAcual = new BoundingBox(st.BoundingBox.Min, st.BoundingBox.Max);    
        }

    }

    internal void DrawBlack() => PistonDerby.GameContent.BlackDrawer.Draw(Model, WorldMatrix);
    internal void AddToSimulation(Vector3 initialPosition, Quaternion initialRotation) { 
        StaticHandlers = new List<StaticHandle>();
        foreach(var shape in Shapes) {
            StaticHandle st = PistonDerby.Simulation.CreateStatic(initialPosition.ToBepu(), initialRotation.ToBepu(), shape);
            StaticHandlers.Add(st);
            PistonDerby.Simulation.Colliders.RegisterCollider(st, this);
        }
    }
}