using Microsoft.Xna.Framework;

namespace PistonDerby.Drawers;
internal interface IDrawer
{
    void Draw(Matrix World);
}