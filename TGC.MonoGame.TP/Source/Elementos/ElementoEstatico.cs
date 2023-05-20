
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Elementos
{
    public class ElementoEstatico : Elemento {
        internal IConvexShape Shape { get; set; }
        private IDrawer SavedDrawer;
        private Matrix WorldMatrix;

        internal override IDrawer Drawer() => SavedDrawer;
        internal override Matrix World() => WorldMatrix;

        internal ElementoEstatico(IDrawer Drawer, Vector3 Position, Quaternion Rotation, float Scale = 1) {
            this.SavedDrawer = Drawer;
            WorldMatrix  =  Matrix.CreateScale(Scale) *
                            Matrix.CreateFromQuaternion(Rotation) *
                            Matrix.CreateTranslation(Position);
        }

        internal void AddToSimulation<TConvexShape>(Vector3 Position, Quaternion Rotation) where TConvexShape : unmanaged, IConvexShape
        { 
            TGCGame.Simulation.Statics.Add(new StaticDescription(
                Position.ToBepu(), Rotation.ToBepu(),
                TGCGame.Simulation.Shapes.Add((TConvexShape)Shape)));
        }
   }
}