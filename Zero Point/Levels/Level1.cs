using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Entities;
using ZeroPoint.Utils;
using System.Collections.Generic;
using System;
using System.IO;
using TiledSharp;

namespace ZeroPoint.Levels;

public class Level1
{
    // объекты
    public List<Platform> Platforms { get; private set; }
    public List<Spike> Spikes { get; private set; }
    public List<MetalSurface> MetalSurfaces { get; private set; }
    public List<HiddenPlatform> HiddenPlatforms { get; private set; }

    public Vector2 PlayerStartPosition { get; private set; }
    public Rectangle ExitDoor { get; private set; }

    // Текстуры
    private Texture2D tileset;
    private Texture2D midBackground;
    private Texture2D farBackground;

    // TMX данные
    private TmxMap tmx;

    private const int TILE_SIZE = 32;

    public Level1(ContentManager contentManager = null)
    {
        Platforms = new List<Platform>();
        Spikes = new List<Spike>();
        MetalSurfaces = new List<MetalSurface>();
        HiddenPlatforms = new List<HiddenPlatform>();

        PlayerStartPosition = new Vector2(100, 500);
        LoadDefaultLevel();
    }

    public void LoadTextures(Texture2D tilesetTexture, Texture2D midBg, Texture2D farBg)
    {
        tileset = tilesetTexture;
        midBackground = midBg;
        farBackground = farBg;
    }

    public void LoadFromTmx(ContentManager contentManager)
    {
        try
        {
            var basePath = AppContext.BaseDirectory;
            var tmxPath = Path.Combine(basePath, "Content", "Levels", "level.tmx");

            if (!File.Exists(tmxPath))
                return;

            tmx = new TmxMap(tmxPath);
        }
        catch
        {
            // on failure, keep default level
            tmx = null;
        }
    }

    // Позволяет программно установить данные уровня (платформы, шипы и т.д.)
    public void SetLevelData(List<Platform> platforms, List<Spike> spikes, List<MetalSurface> metalSurfaces, List<HiddenPlatform> hiddenPlatforms, Rectangle exitDoor, Vector2 playerStart)
    {
        Platforms = platforms ?? new List<Platform>();
        Spikes = spikes ?? new List<Spike>();
        MetalSurfaces = metalSurfaces ?? new List<MetalSurface>();
        HiddenPlatforms = hiddenPlatforms ?? new List<HiddenPlatform>();
        ExitDoor = exitDoor;
        PlayerStartPosition = playerStart;
    }

    private void LoadDefaultLevel()
    {

        // пол
        Platforms.Add(new Platform(0, 700, 2000, 20));
        // стартовая платформа
        Platforms.Add(new Platform(100, 550, 100, 20));
        // платформа для прыжка
        Platforms.Add(new Platform(300, 500, 80, 20));
        // платформа перед выходом
        Platforms.Add(new Platform(750, 550, 100, 20));

        //метал поверх
        // стена слева
        MetalSurfaces.Add(new MetalSurface(50, 400, 40, 150));
        // участок пола
        MetalSurfaces.Add(new MetalSurface(300, 630, 80, 20));
        // стена для прыжка с магнитом
        MetalSurfaces.Add(new MetalSurface(700, 450, 40, 100));
        // потолок для магнита
        MetalSurfaces.Add(new MetalSurface(500, 100, 100, 20));

        // скрытые платформы
        HiddenPlatforms.Add(new HiddenPlatform(600, 400, 80, 20));
        HiddenPlatforms.Add(new HiddenPlatform(800, 520, 60, 20));

        //шипи
        Spikes.Add(new Spike(480, 550));
        Spikes.Add(new Spike(512, 550));

        //выход
        ExitDoor = new Rectangle(850, 600, 40, 50);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture, Texture2D blockTexture, Texture2D spikeTexture)
    {
        if (tmx != null && tileset != null)
        {
            DrawTmxLayers(spriteBatch);
        }
        else
        {
            // Рисуем платформы с текстурами
            if (tileset != null)
            {
                foreach (var platform in Platforms)
                    DrawTiledPlatform(spriteBatch, platform.Bounds, 4);

                foreach (var metal in MetalSurfaces)
                    DrawTiledPlatform(spriteBatch, metal.Bounds, 3);
            }
            else
            {
                foreach (var platform in Platforms)
                    platform.Draw(spriteBatch, blockTexture);

                foreach (var metal in MetalSurfaces)
                    metal.Draw(spriteBatch, blockTexture);
            }

            // Рисуем скрытые платформы и шипы
            foreach (var hidden in HiddenPlatforms)
                hidden.Draw(spriteBatch, blockTexture);

            foreach (var spike in Spikes)
                spike.Draw(spriteBatch, spikeTexture);

            // Рисуем выход
            spriteBatch.Draw(pixelTexture, ExitDoor, Color.Purple);
        }
    }

    private void DrawTiledPlatform(SpriteBatch spriteBatch, Rectangle bounds, int tileIndex)
    {
        if (tileset == null) return;

        int tilesetsPerRow = tileset.Width / TILE_SIZE;
        int tileX = (tileIndex % tilesetsPerRow) * TILE_SIZE;
        int tileY = (tileIndex / tilesetsPerRow) * TILE_SIZE;
        var sourceRect = new Rectangle(tileX, tileY, TILE_SIZE, TILE_SIZE);

        // Заполняем платформу повторяющимися тайлами
        for (int x = bounds.X; x < bounds.Right; x += TILE_SIZE)
        {
            for (int y = bounds.Y; y < bounds.Bottom; y += TILE_SIZE)
            {
                int width = System.Math.Min(TILE_SIZE, bounds.Right - x);
                int height = System.Math.Min(TILE_SIZE, bounds.Bottom - y);
                
                var destRect = new Rectangle(x, y, width, height);
                spriteBatch.Draw(tileset, destRect, sourceRect, Color.White);
            }
        }
    }

    private void DrawTmxLayers(SpriteBatch spriteBatch)
    {
        if (tmx == null || tileset == null) return;

        int tilesPerRow = tileset.Width / tmx.TileWidth;

        foreach (var layer in tmx.Layers)
        {
            for (int y = 0; y < tmx.Height; y++)
            {
                for (int x = 0; x < tmx.Width; x++)
                {
                    var tile = layer.Tiles[x + y * tmx.Width];
                    if (tile.Gid == 0) continue;

                    int tileId = tile.Gid - tmx.Tilesets[0].FirstGid;
                    int sx = (tileId % tilesPerRow) * tmx.TileWidth;
                    int sy = (tileId / tilesPerRow) * tmx.TileHeight;

                    var src = new Rectangle(sx, sy, tmx.TileWidth, tmx.TileHeight);
                    var dest = new Rectangle(x * tmx.TileWidth, y * tmx.TileHeight, tmx.TileWidth, tmx.TileHeight);

                    spriteBatch.Draw(tileset, dest, src, Color.White);
                }
            }
        }
    }
}