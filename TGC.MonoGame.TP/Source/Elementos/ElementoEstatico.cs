
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Drawers;
using PistonDerby.Utils;

namespace PistonDerby.Elementos;
public class ElementoEstatico : Elemento {
    internal override Model Model => SettedModel;
    internal readonly Model SettedModel;
    protected StaticHandle StaticHandle { get; private set; }
    internal TypedIndex Shape { get; set; }
    private IDrawer SavedDrawer;
    private Matrix WorldMatrix;

    internal override IDrawer Drawer => SavedDrawer;
    internal override Matrix World => WorldMatrix;

    internal ElementoEstatico(Model Model, IDrawer Drawer, Vector3 Position, Vector3 Rotation, float Scale = 1, Collisions.ShapeType shapeType = Collisions.ShapeType.BOX) {
        this.SettedModel = Model;
        this.SavedDrawer = Drawer;
        Matrix rotacion =  Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z); 
        WorldMatrix  =  rotacion * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

        if(Model is null) return; // Fixea Elementos Estaticos Hechos con Geometria, hay que adaptar eso.

        Shape = PistonDerby.Simulation.LoadShape(Collisions.ShapeType.BOX, Model, Scale);
        this.AddToSimulation(Position, Quaternion.CreateFromRotationMatrix(rotacion));
    }

    private StaticReference Body() => PistonDerby.Simulation.GetStaticReference(StaticHandle);

    protected override void DebugGizmos()
    {
        BoundingBox aabb = this.Body().BoundingBox.ToBoundingBox(); 
        PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Gold);
        

        BoundingBox sombraAcual = new BoundingBox(this.Body().BoundingBox.Min, this.Body().BoundingBox.Max);
        float alturaBoxSombra = sombraAcual.Max.Y - sombraAcual.Min.Y;
        
        sombraAcual.Min.Y = -alturaBoxSombra*0.5f;
        sombraAcual.Max.Y = -alturaBoxSombra*0.5f;
        
        aabb = sombraAcual; 
        PistonDerby.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Magenta);
    }

    internal void AddToSimulation(Vector3 initialPosition, Quaternion initialRotation) { 
        StaticHandle = PistonDerby.Simulation.CreateStatic(initialPosition.ToBepu(), initialRotation.ToBepu(), Shape);
        PistonDerby.Simulation.Colliders.RegisterCollider(StaticHandle, this);
    }
}