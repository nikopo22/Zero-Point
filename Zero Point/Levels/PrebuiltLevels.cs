using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ZeroPoint.Entities;

namespace ZeroPoint.Levels;

public static class PrebuiltLevels
{
    public static void ApplyEasyLevel(Level1 level)
    {
        var platforms = new List<Platform>()
        {
            new Platform(0, 680, 1400, 40),
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

        var metals = new List<MetalSurface>();
        var hidden = new List<HiddenPlatform>();
        var walls = new List<InvisibleWall>()
        {
            new InvisibleWall(0, 0, 50, 800),
            new InvisibleWall(1350, 0, 50, 800),
        };

        var exit = new Rectangle(1220, 260, 40, 50);
        var playerStart = level.PlayerStartPosition;

        level.SetLevelData(platforms, spikes, metals, hidden, exit, playerStart, walls);
    }

    public static void ApplyMediumLevel(Level1 level)
    {
        var platforms = new List<Platform>()
        {
            new Platform(0, 680, 1400, 40),
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
            new Platform(0, 680, 4200, 40),
            new Platform(150, 580, 120, 25),

            new Platform(1450, 400, 80, 25),

            new Platform(1150, 380, 90, 25),
            new Platform(1300, 320, 100, 25),

            new Platform(1450, 400, 80, 25),
            new Platform(1550, 350, 80, 25),

            new Platform(1900, 220, 100, 25),
            new Platform(2100, 260, 150, 25),

            new Platform(2350, 580, 100, 25),
            new Platform(2500, 500, 90, 25),
            
            new Platform(2900, 350, 80, 25),
            new Platform(3050, 280, 80, 25),
            new Platform(3200, 350, 100, 25),
            
            new Platform(3450, 550, 80, 25),
            new Platform(3600, 480, 80, 25),
            new Platform(3750, 400, 90, 25),
            new Platform(3900, 320, 100, 25),
            new Platform(4050, 250, 120, 25),
            
            new Platform(4200, 200, 150, 25),

        };

        var hidden = new List<HiddenPlatform>()
        {
            new HiddenPlatform(350, 500, 100, 25),
            new HiddenPlatform(680, 480, 80, 25),
                    new HiddenPlatform(1620, 320, 100, 20),
        
            new HiddenPlatform(2800, 450, 100, 20),
            new HiddenPlatform(2950, 500, 80, 20),
            
            new HiddenPlatform(3700, 300, 100, 20),
            
            new HiddenPlatform(4150, 350, 120, 20),
 
        };

        var metals = new List<MetalSurface>()
        {
            new MetalSurface(1000, 450, 40, 230),
            new MetalSurface(1400, 450, 40, 230),
            new MetalSurface(1800, 200, 50, 480),
            new MetalSurface(2700, 300, 35, 380),
            new MetalSurface(3300, 250, 40, 430),
            new MetalSurface(4100, 300, 45, 380),
        };

        var spikes = new List<Spike>()
        {
            new Spike(400, 655),
            new Spike(440, 655),
            new Spike(480, 655),
            new Spike(650, 655),
            new Spike(690, 655),
            new Spike(730, 655),
            new Spike(770, 655),
            new Spike(1100, 655),
            new Spike(1140, 655),
            new Spike(1180, 655),
            new Spike(1220, 655),
            new Spike(1260, 655),
            new Spike(1650, 655),
            new Spike(1690, 655),
            new Spike(1730, 655),
            new Spike(2250, 655),
            new Spike(2290, 655),
            new Spike(2330, 655),
            new Spike(2370, 655),
            new Spike(2410, 655),
            new Spike(2850, 655),
            new Spike(2890, 655),
            new Spike(2930, 655),
            new Spike(2970, 655),
            new Spike(3450, 655),
            new Spike(3490, 655),
            new Spike(3530, 655),
            new Spike(3570, 655),
            new Spike(3610, 655),
            new Spike(3650, 655),
            new Spike(3690, 655),
            new Spike(3730, 655),
            new Spike(3770, 655),
            new Spike(3810, 655),
            new Spike(3850, 655),
            new Spike(3890, 655),
            new Spike(3930, 655),
            new Spike(3970, 655),
            new Spike(4010, 655),
            new Spike(4050, 655),
            new Spike(4090, 655),
            new Spike(4120, 655),
            new Spike(4160, 655),
            new Spike(1300, 295),
            new Spike(1340, 295),
            

        };

        var walls = new List<InvisibleWall>()
        {
            new InvisibleWall(0, 0, 50, 800),         
            new InvisibleWall(4350, 0, 50, 800),       
        };

        var exit = new Rectangle(4300, 150, 50, 60);
        var playerStart = level.PlayerStartPosition;

        level.SetLevelData(platforms, spikes, metals, hidden, exit, playerStart, walls);
    }
}
