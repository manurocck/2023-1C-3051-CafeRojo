
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Drawers;

namespace TGC.MonoGame.TP.Elementos
{
    public abstract class Elemento {

        internal abstract IDrawer Drawer();
        internal abstract Matrix World();

        internal virtual void Draw() => Drawer().Draw(World());
        internal virtual bool OnCollision(Elemento other) => true;
    }
}