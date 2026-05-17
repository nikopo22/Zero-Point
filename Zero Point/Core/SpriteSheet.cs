using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeroPoint.Core;

public class SpriteSheet
{
    private Texture2D _texture;
    private int _frameWidth;
    private int _frameHeight;
    private int _columns;
    
    public SpriteSheet(Texture2D texture, int frameWidth, int frameHeight)
    {
        _texture = texture;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _columns = texture.Width / frameWidth;
    }
    
    public Rectangle GetSourceRectangle(int frameIndex)
    {
        int x = (frameIndex % _columns) * _frameWidth;
        int y = (frameIndex / _columns) * _frameHeight;
        return new Rectangle(x, y, _frameWidth, _frameHeight);
    }
    
    public void Draw(SpriteBatch spriteBatch, int frameIndex, Vector2 position, Color color)
    {
        Rectangle sourceRect = GetSourceRectangle(frameIndex);
        spriteBatch.Draw(_texture, position, sourceRect, color);
    }
    
    public void Draw(SpriteBatch spriteBatch, int frameIndex, Rectangle destinationRect, Color color)
    {
        Rectangle sourceRect = GetSourceRectangle(frameIndex);
        spriteBatch.Draw(_texture, destinationRect, sourceRect, color);
    }
    
    public void Draw(SpriteBatch spriteBatch, int frameIndex, Vector2 position, Color color, SpriteEffects effects)
    {
        Rectangle sourceRect = GetSourceRectangle(frameIndex);
        spriteBatch.Draw(_texture, position, sourceRect, color, 0f, Vector2.Zero, 1f, effects, 0f);
    }
    
    public int FrameWidth => _frameWidth;
    public int FrameHeight => _frameHeight;
}
