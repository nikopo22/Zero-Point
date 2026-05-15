using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeroPoint.Entities;

public class HiddenPlatform
{
    public Rectangle Bounds { get; private set; }
    public bool IsRevealed { get; set; } 

    private Color hiddenColor;   // когда скрыта (почти прозрачный)
    private Color revealedColor; // когда раскрыта (стальной)

    public HiddenPlatform(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
        hiddenColor = new Color(80, 80, 80, 50);     // полупрозрачный
        revealedColor = new Color(150, 180, 200);   
        IsRevealed = false;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        if (!IsRevealed)
            return;

        spriteBatch.Draw(pixelTexture, Bounds, revealedColor);
    }
}
