using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Utils;

namespace ZeroPoint.Entities;

public class Platform
{
    public Rectangle Bounds { get; private set; }  
    private Color color;                           

    public Platform(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
        color = new Color(100, 100, 100);  
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D platformTexture)
    {
        if (platformTexture == null)
            return;

        // If platform is taller than wide, treat as vertical: use fixed tile height
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