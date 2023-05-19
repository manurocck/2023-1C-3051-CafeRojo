using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Drawers
{
    internal interface MDrawable {
        internal IDrawer Drawer { get; set;} 
        internal Matrix World();
    }

    internal static class DrawableExtension {
        internal static void Draw(this MDrawable drawable) => drawable.Drawer.Draw(drawable.World());
    }
}