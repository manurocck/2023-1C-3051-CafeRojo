using System;
using System.Linq;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Utils;
internal static class ModelExtensions {
    /// <summarize> Devuelve el tamaño de la caja que envuelve tu corazon </summarize> 
    internal static Box GeneraterBox(this Microsoft.Xna.Framework.Graphics.Model model, float scale = 1)
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

    /*internal static Box GeneraterBox2(this Microsoft.Xna.Framework.Graphics.Model model, float scale = 1f)
    {
        Vector3 minPoint = Vector3.One * float.MaxValue;
        Vector3 maxPoint = Vector3.One * float.MinValue;

        Matrix[] transforms = new Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(transforms);

        foreach (ModelMesh mesh in model.Meshes) 
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                Vector3[] vertices = GetVertexs(meshPart);
                foreach (Vector3 vertex in vertices)
                {
                    Matrix transform = transforms[mesh.ParentBone.Index];
                    Vector3 transformedVertex = Vector3.Transform(vertex, transform.DeEscalateTransform());
                    minPoint = Vector3.Min(minPoint, vertex * scale);
                    maxPoint = Vector3.Max(maxPoint, vertex * scale);
                }
            }
        
        return new Box(
            Math.Abs(minPoint.X - maxPoint.X),
            Math.Abs(minPoint.Y - maxPoint.Y),
            Math.Abs(minPoint.Z - maxPoint.Z)
        );
    }


    private static Vector3[] GetVertexs(ModelMeshPart meshPart)
    {
        VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
        VertexElement[] elements = vd.GetVertexElements();

        bool elementPredicate(VertexElement ve) => ve.VertexElementUsage == VertexElementUsage.Position && ve.VertexElementFormat == VertexElementFormat.Vector3;
        if (!elements.Any(elementPredicate))
            return null;

        VertexElement element = elements.First(elementPredicate);
        
        Vector3[] vertexData = new Vector3[meshPart.NumVertices];
        meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
            vertexData, 0, vertexData.Length, vd.VertexStride);

        return vertexData;
    }*/
}
