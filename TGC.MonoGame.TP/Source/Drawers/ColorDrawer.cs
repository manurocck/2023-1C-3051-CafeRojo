using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Drawers
{
    internal class ColorDrawer : IDrawer
    {
        private static Effect Effect => TGCGame.GameContent.E_BasicShader;
        protected readonly Model Model;
        protected readonly Color Color;

        internal ColorDrawer(Model Model, Color Color)
        {
            this.Model = Model;
            this.Color = Color;
        }

        void IDrawer.Draw(Matrix World)
        {
            ModelMeshCollection meshes = Model.Meshes;

            foreach (ModelMesh mesh in meshes)
            {
                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
                Effect.Parameters["DiffuseColor"].SetValue(Color.ToVector3());
                mesh.Draw();
            }
        }
    }
}