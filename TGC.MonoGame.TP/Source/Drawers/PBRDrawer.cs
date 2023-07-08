using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Drawers;
internal class PBRDrawer : IDrawer
{
    private static Effect Effect => PistonDerby.GameContent.E_PBRpackedShader;
    protected readonly Texture2D PBRpackedTexture;
    protected readonly Texture2D BaseColor;
    protected readonly Texture2D NormalMap;

    internal PBRDrawer(Texture2D RoughnessMetallicOpacity, Texture2D BaseColor, Texture2D NormalMap)
    {
        this.PBRpackedTexture = RoughnessMetallicOpacity;
        this.BaseColor = BaseColor;
        this.NormalMap = NormalMap;
    }
    

    void IDrawer.Draw(Model Model, Matrix World)
    {
        ModelMeshCollection meshes = Model.Meshes;
        SetearTexturas();

        foreach (ModelMesh mesh in meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                meshPart.Effect = Effect;

            Effect.Parameters["World"]?.SetValue(World);
            Effect.Parameters["matInverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(World)));
            Effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0.5f, 0.5f));
            mesh.Draw();
        }
    }

    private void SetearTexturas()
    {
        Effect.Parameters["baseColorTexture"]?.SetValue(BaseColor);
        Effect.Parameters["normalTexture"]?.SetValue(NormalMap);
        Effect.Parameters["roughnessMetallicOpacityTexture"]?.SetValue(PBRpackedTexture);
    }
}