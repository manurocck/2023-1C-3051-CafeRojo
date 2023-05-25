using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP.Drawers;

internal class GeometryTextureDrawer : IDrawer{
    private Effect Efecto = TGCGame.GameContent.E_TextureTiles;
    private Texture2D Texture;
    private IGeometryPrimitive Geometry;
    internal GeometryTextureDrawer(IGeometryPrimitive geometry, Texture2D texture){
        this.Texture = texture;
        this.Geometry = geometry;
    }

    void IDrawer.Draw(Matrix World)
    {
        Efecto.Parameters["World"].SetValue(World);
        Geometry.Draw(Efecto);
    }
}