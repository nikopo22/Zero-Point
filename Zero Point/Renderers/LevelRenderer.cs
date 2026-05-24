using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Levels;
using ZeroPoint.Entities;
using ZeroPoint.Core;
using ZeroPoint.Utils;
using System;

namespace ZeroPoint.Renderers
{
    public static class LevelRenderer
    {
        public static void Draw(Level1 level, SpriteBatch spriteBatch, Texture2D pixelTexture, Texture2D blockTexture, Texture2D spikeTexture, SpriteSheet portalSpriteSheet, int portalFrame)
        {
            if (level == null || spriteBatch == null)
                return;

            foreach (var platform in level.Platforms)
                DrawPlatform(spriteBatch, platform.Bounds, blockTexture);

            foreach (var metal in level.MetalSurfaces)
                DrawPlatform(spriteBatch, metal.Bounds, blockTexture);

            foreach (var hidden in level.HiddenPlatforms)
                DrawHiddenPlatform(spriteBatch, hidden, blockTexture);

            foreach (var spike in level.Spikes)
                DrawSpike(spriteBatch, spike, spikeTexture);

            if (portalSpriteSheet != null)
            {
                int portalWidth = portalSpriteSheet.FrameWidth / 5;
                int portalHeight = portalSpriteSheet.FrameHeight / 5;
                var portalDrawRect = new Rectangle(
                    level.ExitDoor.Center.X - portalWidth / 2,
                    level.ExitDoor.Bottom - portalHeight,
                    portalWidth,
                    portalHeight);

                portalSpriteSheet.Draw(spriteBatch, portalFrame, portalDrawRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(pixelTexture, level.ExitDoor, Color.Purple);
            }
        }

        private static void DrawPlatform(SpriteBatch spriteBatch, Rectangle Bounds, Texture2D platformTexture)
        {
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

        private static void DrawHiddenPlatform(SpriteBatch spriteBatch, HiddenPlatform hidden, Texture2D platformTexture)
        {
            if (hidden == null || !hidden.IsRevealed)
                return;
            DrawPlatform(spriteBatch, hidden.Bounds, platformTexture);
        }

        private static void DrawSpike(SpriteBatch spriteBatch, Spike spike, Texture2D spikeTexture)
        {
            if (spike == null || spikeTexture == null)
                return;

            spriteBatch.Draw(spikeTexture, spike.Bounds, Color.White);
        }
    }
}
