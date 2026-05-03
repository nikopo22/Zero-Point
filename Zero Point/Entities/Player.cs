using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.Utils;

namespace ZeroPoint.Entities;

public class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; } 
    public bool IsGrounded { get; set; }

    public Rectangle Bounds => new Rectangle(
        (int)Position.X,
        (int)Position.Y,
        Constants.PLAYER_WIDTH,
        Constants.PLAYER_HEIGHT
    );

    public Rectangle PreviousBounds { get; private set; }

    private Color color;
    private KeyboardState previousKeyboardState;

    public Player(Vector2 startPosition)
    {
        Position = startPosition;
        Velocity = Vector2.Zero;
        IsGrounded = false;
        color = Color.Blue;
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        PreviousBounds = Bounds;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // движение влево-вправо
        float moveDirection = 0;
        if (keyboardState.IsKeyDown(Keys.A))
            moveDirection = -1;
        if (keyboardState.IsKeyDown(Keys.D))
            moveDirection = 1;

        Velocity = new Vector2(moveDirection * Constants.PLAYER_SPEED, Velocity.Y);

        // прыг 
        if (keyboardState.IsKeyDown(Keys.W) &&
            previousKeyboardState.IsKeyUp(Keys.W) &&
            IsGrounded)
        {

            Velocity = new Vector2(Velocity.X, Constants.PLAYER_JUMP_FORCE);
            IsGrounded = false;
        }

        // гравитация
        Velocity = new Vector2(Velocity.X, Velocity.Y + Constants.GRAVITY * deltaTime);

        Position += Velocity * deltaTime;

        previousKeyboardState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        spriteBatch.Draw(pixelTexture, Bounds, color);
    }

    public void Reset(Vector2 respawnPosition)
    {
        Position = respawnPosition;
        Velocity = Vector2.Zero;
        IsGrounded = false;
    }
}
