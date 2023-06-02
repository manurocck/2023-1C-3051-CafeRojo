using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Drawers;
internal class ColorDrawer : IDrawer
{
    private static Effect Effect => PistonDerby.GameContent.E_BasicShader;
    protected readonly Color Color;

    internal ColorDrawer(Color Color)
    {
        this.Color = Color;
    }

    void IDrawer.Draw(Model Model, Matrix World)
    {
        ModelMeshCollection meshes = Model.Meshes;
        Effect.Parameters["DiffuseColor"].SetValue(Color.ToVector3());

        foreach (ModelMesh mesh in meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                meshPart.Effect = Effect;

            Effect.Parameters["Intensidad"].SetValue(1);
            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }
}