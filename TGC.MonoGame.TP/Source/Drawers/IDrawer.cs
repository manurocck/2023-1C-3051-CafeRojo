using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PistonDerby.Drawers;
internal interface IDrawer
{
    void Draw(Model model, Matrix world);
}