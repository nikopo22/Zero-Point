using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Entities;
using ZeroPoint.Utils;
using System.Collections.Generic;

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
    public bool LevelCompleted { get; set; }

    public Level1()
    {
        Platforms = new List<Platform>();
        Spikes = new List<Spike>();
        MetalSurfaces = new List<MetalSurface>();
        HiddenPlatforms = new List<HiddenPlatform>();
        LevelCompleted = false;

        PlayerStartPosition = new Vector2(100, 500);

        //платформы
        // пол
        Platforms.Add(new Platform(0, 650, 2000, 20));
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

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        //обычные платформы
        foreach (var platform in Platforms)
        {
            platform.Draw(spriteBatch, pixelTexture);
        }

        //металлические поверхности
        foreach (var metal in MetalSurfaces)
        {
            metal.Draw(spriteBatch, pixelTexture);
        }

        //скрытые платформы
        foreach (var hidden in HiddenPlatforms)
        {
            hidden.Draw(spriteBatch, pixelTexture);
        }

        //шипы
        foreach (var spike in Spikes)
        {
            spike.Draw(spriteBatch, pixelTexture);
        }

        //выход
        spriteBatch.Draw(pixelTexture, ExitDoor, Color.Purple);
    }
}