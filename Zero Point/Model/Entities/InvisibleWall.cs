using Microsoft.Xna.Framework;

namespace ZeroPoint.Entities;

public class InvisibleWall
{
    public Rectangle Bounds { get; private set; }

    public InvisibleWall(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
    }
}
