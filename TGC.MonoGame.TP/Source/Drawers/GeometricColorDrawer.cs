using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Geometries;

namespace PistonDerby.Drawers;

internal class GeometricColorDrawer : IDrawer{
    private Effect Efecto = PistonDerby.GameContent.E_TextureTiles;
    private Color Color;
    private GeometricPrimitive Geometry;
    internal GeometricColorDrawer(GeometricPrimitive geometry, Color color){
        this.Color = color;
        this.Geometry = geometry;
    }

    void IDrawer.Draw(Model Model, Matrix World)
    {
        Efecto.Parameters["World"].SetValue(World);
        Efecto.Parameters["DiffuseColor"].SetValue(Color.ToVector3());
        Geometry.Draw(Efecto);
    }
}