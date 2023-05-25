using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Drawers;
internal class MirrorDrawer : IDrawer
{
    protected Effect Effect = PistonDerby.GameContent.E_TextureMirror;
    protected readonly Model Model;
    protected readonly Texture2D Texture;

    internal MirrorDrawer(Model Model, Texture2D Texture)
    {
        this.Model = Model;
        this.Texture = Texture;
    }

    void IDrawer.Draw(Matrix World)
    {
        ModelMeshCollection meshes = Model.Meshes;
        Effect.Parameters["Texture"].SetValue(Texture);
        foreach (ModelMesh mesh in meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                meshPart.Effect = Effect;

            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }
}