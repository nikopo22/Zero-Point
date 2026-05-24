using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Entities;

namespace ZeroPoint.Renderers
{
    public static class PlayerRenderer
    {
        // Observer Pattern: Track player health to change render color
        private static int _lastKnownHealth = 1;
        private static Color _healthAwareColor = Color.White;

        public static void Draw(SpriteBatch spriteBatch, Player player, Core.SpriteSheet spriteSheet)
        {
            if (player == null || spriteSheet == null)
                return;

            // Observer Pattern: React to health changes (Health == 0 means dead)
            if (player.Health != _lastKnownHealth)
            {
                _lastKnownHealth = player.Health;
                
                // Change render color based on health (dead = red)
                _healthAwareColor = player.Health <= 0 
                    ? new Color(255, 0, 0, 200)      // Dead: red
                    : Color.White;                    // Alive: normal
            }

            SpriteEffects effect = player.FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            
            // Observer Pattern: Use health-aware color
            Color drawColor = player.MagnetAbility.IsActive 
                ? new Color(100, 150, 255)  // Magnet: blue
                : _healthAwareColor;         // Otherwise: health-based color

            int cropTop = 6;

            Rectangle src = spriteSheet.GetSourceRectangle(player.CurrentFrame);
            src.Y += cropTop;
            src.Height = System.Math.Max(1, src.Height - cropTop);

            float destW = src.Width * player.DrawScale;
            float destH = src.Height * player.DrawScale;

            float drawX = player.Position.X - (destW - Utils.Constants.PLAYER_WIDTH) / 2f;
            float drawY = player.Position.Y - (destH - Utils.Constants.PLAYER_HEIGHT);

            var destRect = new Rectangle((int)System.Math.Round(drawX), (int)System.Math.Round(drawY), (int)System.Math.Round(destW), (int)System.Math.Round(destH));

            spriteBatch.Draw(spriteSheet.Texture, destRect, src, drawColor, 0f, Vector2.Zero, effect, 0f);
        }
    }
}
