using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Geometries
{
    internal class QuadPrimitive : IGeometryPrimitive
    {
        internal QuadPrimitive(GraphicsDevice graphicsDevice) : base (graphicsDevice) { }
        internal override void CreateVertexBuffer(GraphicsDevice graphicsDevice)
        {
            Vector2 textureCoordinateLowerLeft = Vector2.Zero;
            Vector2 textureCoordinateLowerRight = Vector2.UnitX;
            Vector2 textureCoordinateUpperLeft = Vector2.UnitY;
            Vector2 textureCoordinateUpperRight = Vector2.One;

            var vertices = new[]
            {
                // (0,0,0)
                new VertexPositionNormalTexture(Vector3.Zero, Vector3.Up, textureCoordinateLowerLeft),
                // (0,0,1)
                new VertexPositionNormalTexture(Vector3.UnitZ, Vector3.Up, textureCoordinateUpperLeft),
                // (1,0,1)
                new VertexPositionNormalTexture(Vector3.UnitZ + Vector3.UnitX, Vector3.Up, textureCoordinateUpperRight),
                // (1,0,0)
                new VertexPositionNormalTexture(Vector3.UnitX, Vector3.Up, textureCoordinateLowerRight)
            };

            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(vertices);
        }
        internal override void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            var indices = new ushort[]
            {
                0, 2, 1, 
                0, 3, 2,
            };

            Indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length,
                BufferUsage.WriteOnly);
            Indices.SetData(indices);
        }
    }
}