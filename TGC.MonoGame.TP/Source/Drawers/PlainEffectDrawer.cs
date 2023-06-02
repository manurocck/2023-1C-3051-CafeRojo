using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Drawers;
internal class PlainEffectDrawer : IDrawer // Generalizar como EffectDrawer ?? NoParametersDrawer ??
{
    protected Effect Effect;
    
    internal PlainEffectDrawer(Effect Effect)
    {
        this.Effect = Effect;
    }

    void IDrawer.Draw(Model Model, Matrix World)
    {
        ModelMeshCollection meshes = Model.Meshes;
        foreach (ModelMesh mesh in meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                meshPart.Effect = Effect;

            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }
}
