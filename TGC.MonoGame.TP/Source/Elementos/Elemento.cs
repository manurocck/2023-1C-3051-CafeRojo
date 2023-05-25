
using Microsoft.Xna.Framework;
using PistonDerby.Drawers;

namespace PistonDerby.Elementos;
public abstract class Elemento {

    internal abstract IDrawer Drawer();
    internal abstract Matrix World();

    internal virtual void Draw() => Drawer().Draw(World());
    internal virtual bool OnCollision(Elemento other) => true;
}