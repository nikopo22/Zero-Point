using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeroPoint.Entities;

public class MetalSurface
{
    public Rectangle Bounds { get; private set; }
    private Color color;

    public MetalSurface(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
        color = new Color(150, 140, 130);  
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        spriteBatch.Draw(pixelTexture, Bounds, color);
    }
}
