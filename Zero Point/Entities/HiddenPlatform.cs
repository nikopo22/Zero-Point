using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Utils;

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

    public void Draw(SpriteBatch spriteBatch, Texture2D platformTexture)
    {
        if (!IsRevealed)
            return;
        if (platformTexture == null)
            return;

        if (Bounds.Height > Bounds.Width)
        {
            int tileHeight = Constants.PLATFORM_HEIGHT;
            for (int offsetY = 0; offsetY < Bounds.Height; offsetY += tileHeight)
            {
                int destH = Math.Min(tileHeight, Bounds.Height - offsetY);
                var destRect = new Rectangle(Bounds.X, Bounds.Y + offsetY, Bounds.Width, destH);
                spriteBatch.Draw(platformTexture, destRect, Color.White);
            }
            return;
        }

        float scale = (float)Bounds.Height / platformTexture.Height;
        int scaledWidth = Math.Max(1, (int)Math.Round(platformTexture.Width * scale));

        for (int offsetX = 0; offsetX < Bounds.Width; offsetX += scaledWidth)
        {
            int destW = Math.Min(scaledWidth, Bounds.Width - offsetX);
            var destRect = new Rectangle(Bounds.X + offsetX, Bounds.Y, destW, Bounds.Height);

            if (destW == scaledWidth)
            {
                spriteBatch.Draw(platformTexture, destRect, Color.White);
            }
            else
            {
                int srcW = Math.Max(1, (int)Math.Round(destW / scale));
                var srcRect = new Rectangle(0, 0, srcW, platformTexture.Height);
                spriteBatch.Draw(platformTexture, destRect, srcRect, Color.White);
            }
        }
    }
}
