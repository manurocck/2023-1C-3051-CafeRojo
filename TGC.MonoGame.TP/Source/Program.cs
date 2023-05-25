using System;

namespace PistonDerby;
public static class Program
{
    [STAThread]
    static void Main()
    {
        using (var game = new PistonDerby())
            game.Run();
    }
}
