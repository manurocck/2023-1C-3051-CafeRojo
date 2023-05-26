using System;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Geometries;

public abstract class IGeometryPrimitive{
    internal VertexBuffer Vertices { get; set; }
    internal IndexBuffer Indices { get; set; }

    internal IGeometryPrimitive(GraphicsDevice graphicsDevice){   
        CreateVertexBuffer(graphicsDevice);
        CreateIndexBuffer(graphicsDevice);
    }
    internal abstract void CreateVertexBuffer(GraphicsDevice graphicsDevice);
    internal abstract void CreateIndexBuffer(GraphicsDevice graphicsDevice);
    public void Draw(Effect effect)
    {
        var graphicsDevice = effect.GraphicsDevice;

        graphicsDevice.SetVertexBuffer(Vertices);
        graphicsDevice.Indices = Indices;
        
        foreach (var effectPass in effect.CurrentTechnique.Passes)
        {
            effectPass.Apply();
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.IndexCount / 3);
        }
    }
}