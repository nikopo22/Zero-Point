using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeroPoint.Entities;

public class Spike
{
    public Rectangle Bounds { get; private set; }
    private Color color;

    public Spike(int x, int y)
    {
        Bounds = new Rectangle(x, y, 32, 32);
        color = Color.Red; 
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        spriteBatch.Draw(pixelTexture, Bounds, color);
    }
}
