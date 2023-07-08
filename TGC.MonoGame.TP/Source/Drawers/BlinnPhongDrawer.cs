using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Drawers;
internal class BlinnPhongDrawer : IDrawer
{
    protected Effect Effect;
    protected readonly Texture2D Texture;

    internal BlinnPhongDrawer(Texture2D Texture, bool isTiled = false)
    {
        this.Texture = Texture;
        Effect = isTiled ? PistonDerby.GameContent.E_BlinnPhongTiles : PistonDerby.GameContent.E_BlinnPhong;
    }

    void IDrawer.Draw(Model Model, Matrix World)
    {
        ModelMeshCollection meshes = Model.Meshes;;

        Effect.Parameters["Texture"].SetValue(Texture);
        foreach(var mesh in Model.Meshes) {
            // El EnemyCar, si usa esta matrix, las ruedas se le colocan piola
            // pero algunos muebles se "desarman"
            // Matrix meshBoneTranslation = Matrix.CreateTranslation(mesh.ParentBone.ModelTransform.Translation);
            Matrix meshBoneTranslation = Matrix.Identity; 
            foreach(var meshPart in mesh.MeshParts) {
                meshPart.Effect = Effect;
                meshPart.Effect.Parameters["World"]?.SetValue(meshBoneTranslation*World);
                meshPart.Effect.Parameters["matInverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(World)));
                
            }   
            mesh.Draw();
        }
    }
}