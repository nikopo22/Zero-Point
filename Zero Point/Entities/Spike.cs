using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeroPoint.Entities;

public class Spike
{
    public Rectangle Bounds { get; private set; }

    public Spike(int x, int y)
    {
        Bounds = new Rectangle(x, y, 32, 32);
    }

    // Rendering moved to LevelRenderer
}
