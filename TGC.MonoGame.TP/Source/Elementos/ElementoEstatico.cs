
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Elementos;
public class ElementoEstatico : Elemento {
    internal IConvexShape Shape { get; set; }
    private IDrawer SavedDrawer;
    private Matrix WorldMatrix;

    internal override IDrawer Drawer() => SavedDrawer;
    internal override Matrix World() => WorldMatrix;

    internal ElementoEstatico(IDrawer Drawer, Vector3 Position, Vector3 Rotation, float Scale = 1) {
        this.SavedDrawer = Drawer;
        Matrix rotacion =  Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z); 
        WorldMatrix  =  rotacion * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
    }
}