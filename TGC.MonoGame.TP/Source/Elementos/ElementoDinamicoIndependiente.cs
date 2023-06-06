using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PistonDerby.Drawers;
using PistonDerby.Utils;

namespace PistonDerby.Elementos;
public class ElementoDinamicoIndependiente : ElementoDinamico {
    internal override Model Model => SettedModel;
    internal override IDrawer Drawer => this.SavedDrawer;
    internal readonly Model SettedModel;
    internal readonly float SettedScale;
    internal readonly float SettedMass;
    private IDrawer SavedDrawer;
    private Matrix WorldMatrix;

    internal ElementoDinamicoIndependiente(Model model, IDrawer drawer, Vector3 position, Vector3 rotation, float scale = 1, Collisions.ShapeType shapeType = Collisions.ShapeType.BOX) { 
        this.SettedModel = model;
        this.SavedDrawer = drawer;
        this.SettedScale = scale;
        this.SettedMass = scale/20;
        Matrix rotacion =  Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z); 
        WorldMatrix  =  rotacion * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);

        if(Model is null) return; // Fixea Elementos Estaticos Hechos con Geometria, hay que adaptar eso.

        Shape = PistonDerby.Simulation.LoadShape(Collisions.ShapeType.BOX, Model, scale);
        this.AddToSimulation(position, Quaternion.CreateFromRotationMatrix(rotacion));
    }
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

    internal override float Mass() => this.SettedMass;
    internal override float Scale() => SettedScale;
}
