using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Drawers;
internal class NotDrawer : IDrawer
{
    void IDrawer.Draw(Model Model, Matrix World) { }
}