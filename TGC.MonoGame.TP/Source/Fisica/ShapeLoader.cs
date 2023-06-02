using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PistonDerby.Utils;

namespace PistonDerby.Collisions;

enum ShapeType {
    BOX = 0
}

internal class ShapeLoader
{
    private readonly ConcurrentDictionary<ShapeType, Func<Model, float, TypedIndex>> ShapesLoaders = new ConcurrentDictionary<ShapeType, Func<Model, float, TypedIndex>>();
    private readonly Shapes Shapes;

    internal ShapeLoader(Shapes Shapes)
    {
        this.Shapes = Shapes;
        ShapesLoaders.TryAdd(ShapeType.BOX, LoadBox);
    }

    internal TypedIndex LoadShape(ShapeType shapeType, Model model, float scale = 1f) => ShapesLoaders.GetValueOrDefault(shapeType)(model, scale);

    //SHAPE LOADERS
    private TypedIndex LoadBox(Model model, float scale) => Shapes.Add(GeneraterBox(model, scale));

    //SHAPE GENERATORS
    private Box GeneraterBox(Model model, float scale)
    {
        Vector3 minPoint = Vector3.One * float.MaxValue;
        Vector3 maxPoint = Vector3.One * float.MinValue;

        Matrix[] transforms = new Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(transforms);

        var meshes = model.Meshes;
        for (int index = 0; index < meshes.Count; index++)
        {
            var meshParts = meshes[index].MeshParts;
            for (int subIndex = 0; subIndex < meshParts.Count; subIndex++)
            {
                VertexBuffer vertexBuffer = meshParts[subIndex].VertexBuffer;
                VertexDeclaration declaration = vertexBuffer.VertexDeclaration;
                int vertexSize = declaration.VertexStride / sizeof(float);

                float[] rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                vertexBuffer.GetData(rawVertexBuffer);

                for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                {
                    Matrix transform = transforms[meshes[index].ParentBone.Index];
                    Vector3 vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                    vertex = Vector3.Transform(vertex, transform.DeEscalateTransform());
                    minPoint = Vector3.Min(minPoint, vertex * scale);
                    maxPoint = Vector3.Max(maxPoint, vertex * scale);
                }
            }
        }
        return new Box(
            Math.Abs(minPoint.X - maxPoint.X),
            Math.Abs(minPoint.Y - maxPoint.Y),
            Math.Abs(minPoint.Z - maxPoint.Z)
            );
    }

}
