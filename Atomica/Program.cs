﻿using Microsoft.Xna.Framework;

namespace Quickie003;

public static class Program
{
    [STAThread]
    private static void Main()
    {
        using var game = new Game1();
        game.Run();
    }
}