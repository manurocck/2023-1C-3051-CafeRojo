using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Geometries;

namespace PistonDerby.Drawers;

internal class GeometryTextureDrawer : IDrawer{
    private Effect Efecto = PistonDerby.GameContent.E_TextureTiles;
    private Texture2D Texture;
    private IGeometryPrimitive Geometry;
    internal GeometryTextureDrawer(IGeometryPrimitive geometry, Texture2D texture){
        this.Texture = texture;
        this.Geometry = geometry;
    }

    void IDrawer.Draw(Model Model, Matrix World)
    {
        Efecto.Parameters["World"].SetValue(World);
        Geometry.Draw(Efecto);
    }
}