using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Core;
using ZeroPoint.Entities;
using ZeroPoint.Utils;
using System.Collections.Generic;
using System;

namespace ZeroPoint.Levels;

public class Level1
{
    public List<Platform> Platforms { get; private set; }
    public List<Spike> Spikes { get; private set; }
    public List<MetalSurface> MetalSurfaces { get; private set; }
    public List<HiddenPlatform> HiddenPlatforms { get; private set; }
    public List<InvisibleWall> InvisibleWalls { get; private set; }

    public Vector2 PlayerStartPosition { get; private set; }
    public Rectangle ExitDoor { get; private set; }

    private const int TILE_SIZE = 32;

    public Level1(ContentManager contentManager = null)
    {
        Platforms = new List<Platform>();
        Spikes = new List<Spike>();
        MetalSurfaces = new List<MetalSurface>();
        HiddenPlatforms = new List<HiddenPlatform>();
        InvisibleWalls = new List<InvisibleWall>();

        PlayerStartPosition = new Vector2(100, 500);
        LoadDefaultLevel();
    }




    public void SetLevelData(List<Platform> platforms, List<Spike> spikes, List<MetalSurface> metalSurfaces, List<HiddenPlatform> hiddenPlatforms, Rectangle exitDoor, Vector2 playerStart, List<InvisibleWall> invisibleWalls = null)
    {
        Platforms = platforms ?? new List<Platform>();
        Spikes = spikes ?? new List<Spike>();
        MetalSurfaces = metalSurfaces ?? new List<MetalSurface>();
        HiddenPlatforms = hiddenPlatforms ?? new List<HiddenPlatform>();
        InvisibleWalls = invisibleWalls ?? new List<InvisibleWall>();
        ExitDoor = exitDoor;
        PlayerStartPosition = playerStart;
    }

    private void LoadDefaultLevel()
    {
        InvisibleWalls.Add(new InvisibleWall(0, 0, 50, 800));
        InvisibleWalls.Add(new InvisibleWall(1950, 0, 50, 800));

        Platforms.Add(new Platform(0, 700, 2000, 20));
        Platforms.Add(new Platform(100, 550, 100, 20));
        Platforms.Add(new Platform(300, 500, 80, 20));
        Platforms.Add(new Platform(750, 550, 100, 20));

        MetalSurfaces.Add(new MetalSurface(50, 400, 40, 150));
        MetalSurfaces.Add(new MetalSurface(300, 630, 80, 20));
        MetalSurfaces.Add(new MetalSurface(700, 450, 40, 100));
        MetalSurfaces.Add(new MetalSurface(500, 100, 100, 20));

        HiddenPlatforms.Add(new HiddenPlatform(600, 400, 80, 20));
        HiddenPlatforms.Add(new HiddenPlatform(800, 520, 60, 20));

        Spikes.Add(new Spike(480, 550));
        Spikes.Add(new Spike(512, 550));

        ExitDoor = new Rectangle(850, 600, 40, 50);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture, Texture2D blockTexture, Texture2D spikeTexture, SpriteSheet portalSpriteSheet, int portalFrame)
    {
        ZeroPoint.Renderers.LevelRenderer.Draw(this, spriteBatch, pixelTexture, blockTexture, spikeTexture, portalSpriteSheet, portalFrame);
    }

}
