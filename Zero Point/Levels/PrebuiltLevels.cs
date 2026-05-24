using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ZeroPoint.Entities;

namespace ZeroPoint.Levels;

public static class PrebuiltLevels
{
    // Применяет простой уровень (из запроса) к переданному экземпляру Level1
    public static void ApplyEasyLevel(Level1 level)
    {
        var platforms = new List<Platform>()
        {
            // Ground
            new Platform(0, 680, 1400, 40),

            // Platforms
            new Platform(250, 580, 180, 25),
            new Platform(520, 500, 180, 25),
            new Platform(800, 420, 180, 25),
            new Platform(1080, 340, 180, 25),
        };

        var spikes = new List<Spike>()
        {
            new Spike(430, 655),
            new Spike(470, 655),

            new Spike(720, 655),
            new Spike(760, 655),
            new Spike(800, 655),

            new Spike(980, 655),
            new Spike(1020, 655),
        };

        // Нет металлических или скрытых платформ в этом уровне
        var metals = new List<MetalSurface>();
        var hidden = new List<HiddenPlatform>();
        var walls = new List<InvisibleWall>()
        {
            new InvisibleWall(0, 0, 50, 800),
            new InvisibleWall(1350, 0, 50, 800),
        };

        // Выход (Finish) — используем прямоугольник рядом с указанной позицией
        var exit = new Rectangle(1220, 260, 40, 50);

        // Старт игрока — оставим стандартный
        var playerStart = level.PlayerStartPosition;

        level.SetLevelData(platforms, spikes, metals, hidden, exit, playerStart, walls);
    }

    public static void ApplyMediumLevel(Level1 level)
    {
        var platforms = new List<Platform>()
        {
            // земля
            new Platform(0, 680, 1400, 40),

            // Start 
            new Platform(100, 600, 180, 25),

        };

        var hidden = new List<HiddenPlatform>()
        {
            new HiddenPlatform(380, 540, 140, 20),

            new HiddenPlatform(900, 400, 140, 20),
        };

        var metals = new List<MetalSurface>()
        {
            new MetalSurface(560, 350, 40, 180),
            new MetalSurface(1040, 250, 40, 200),
        };

        var spikes = new List<Spike>()
        {
            new Spike(300, 655),
            new Spike(340, 655),
            new Spike(380, 655),
            new Spike(420, 655),

            new Spike(760, 655),
            new Spike(800, 655),
            new Spike(840, 655),


        };

        var walls = new List<InvisibleWall>()
        {
            new InvisibleWall(0, 0, 50, 800),
            new InvisibleWall(1350, 0, 50, 800),
        };

        var exit = new Rectangle(1080, 110, 40, 50);
        var playerStart = level.PlayerStartPosition;

        level.SetLevelData(platforms, spikes, metals, hidden, exit, playerStart, walls);
    }

    public static void ApplyHardLevel(Level1 level)
    {
        var platforms = new List<Platform>()
        {
            // земля
            new Platform(0, 680, 1400, 40),

            // Start
            new Platform(80, 620, 120, 20),

            // Tiny platforms
            // new Platform(650, 500, 90, 20),
            // new Platform(800, 430, 90, 20),

            // Final 
            new Platform(1320, 120, 140, 20),
        };

        var hidden = new List<HiddenPlatform>()
        {
            new HiddenPlatform(260, 570, 120, 20),
            new HiddenPlatform(430, 520, 120, 20),
            new HiddenPlatform(950, 360, 90, 20),
            new HiddenPlatform(1180, 220, 100, 20),
            new HiddenPlatform(1320, 170, 100, 20),
        };

        var metals = new List<MetalSurface>()
        {
            new MetalSurface(560, 420, 40, 220),
            new MetalSurface(1100, 250, 40, 260),
        };

        var spikes = new List<Spike>()
        {
            new Spike(220, 655),
            new Spike(260, 655),
            new Spike(300, 655),
            new Spike(340, 655),

            new Spike(700, 475),
            new Spike(740, 475),

            new Spike(850, 405),
            new Spike(890, 405),

            new Spike(1180, 655),
            new Spike(1220, 655),
            new Spike(1260, 655),
            new Spike(1300, 655),
            new Spike(1340, 655),
        };

        var walls = new List<InvisibleWall>()
        {
            new InvisibleWall(0, 0, 50, 800),
            new InvisibleWall(1350, 0, 50, 800),
        };

        var exit = new Rectangle(1380, 50, 40, 50);
        var playerStart = level.PlayerStartPosition;

        level.SetLevelData(platforms, spikes, metals, hidden, exit, playerStart, walls);
    }
}
