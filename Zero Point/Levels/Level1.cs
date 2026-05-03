using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Entities;
using ZeroPoint.Utils;
using System.Collections.Generic;

namespace ZeroPoint.Levels;

public class Level1
{
    public List<Platform> Platforms { get; private set; }
    public List<Spike> Spikes { get; private set; }
    public Vector2 PlayerStartPosition { get; private set; }
    public Rectangle ExitDoor { get; private set; }
    public bool LevelCompleted { get; set; }

    public Level1()
    {
        Platforms = new List<Platform>();
        Spikes = new List<Spike>();
        LevelCompleted = false;

        // стартовая позиция
        PlayerStartPosition = new Vector2(100, 500);

        // платформы
        // пол
        Platforms.Add(new Platform(0, 650, 2000, 20));

        // 1 платформа
        Platforms.Add(new Platform(100, 550, 100, 20));

        // 2 платформа
        Platforms.Add(new Platform(300, 500, 80, 20));

        // 3 платформа
        Platforms.Add(new Platform(500, 450, 100, 20));

        // 4 платформа
        Platforms.Add(new Platform(750, 550, 100, 20));

        // шипы
        Spikes.Add(new Spike(480, 550)); 
        Spikes.Add(new Spike(512, 550));

        // выход 
        ExitDoor = new Rectangle(850, 600, 40, 50);
    }

    public void Update()
    {
        // Здесь можно добавить анимации платформ и т.д.
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        // Рисуем платформы
        foreach (var platform in Platforms)
        {
            platform.Draw(spriteBatch, pixelTexture);
        }

        // Рисуем шипы
        foreach (var spike in Spikes)
        {
            spike.Draw(spriteBatch, pixelTexture);
        }

        // Рисуем выход (фиолетовый прямоугольник)
        spriteBatch.Draw(pixelTexture, ExitDoor, Color.Purple);
    }
}
