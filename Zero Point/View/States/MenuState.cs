using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.UI;

namespace ZeroPoint.States;

public class MenuState
{
    private readonly Button playButton;
    private readonly Button exitButton;
    private readonly Texture2D pixelTexture;
    private readonly Texture2D menuTexture;
    private MouseState previousMouseState;
    private readonly SpriteFont font;

    public MenuState(GraphicsDevice graphicsDevice, SpriteFont font, Texture2D menuTexture)
    {
        this.font = font;
        this.menuTexture = menuTexture;

        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        int buttonWidth = 220;
        int buttonHeight = 60;
        int centerX = 1280 / 2 - buttonWidth / 2;

        playButton = new Button(new Rectangle(centerX, 320, buttonWidth, buttonHeight), "ИГРАТЬ", font);
        exitButton = new Button(new Rectangle(centerX, 420, buttonWidth, buttonHeight), "ВЫХОД", font);

        previousMouseState = Mouse.GetState();
    }

    public void Update(out bool playClicked, out bool exitClicked)
    {
        MouseState current = Mouse.GetState();

        playButton.Update(current);
        exitButton.Update(current);

        playClicked = playButton.WasReleased(current, previousMouseState);
        exitClicked = exitButton.WasReleased(current, previousMouseState);

        previousMouseState = current;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (menuTexture != null)
        {
            spriteBatch.Draw(menuTexture, new Rectangle(0, 0, 1280, 720), Color.White);
        }
        else
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, 1280, 720), new Color(25, 25, 40));
        }

        string title = "ZERO POINT";
        float titleScale = 2.0f;
        Vector2 titleSize = font.MeasureString(title) * titleScale;
        spriteBatch.DrawString(font, title, new Vector2(1280 / 2 - titleSize.X / 2, 100), Color.Black, 0f, Vector2.Zero, titleScale, SpriteEffects.None, 0f);

        playButton.Draw(spriteBatch, pixelTexture);
        exitButton.Draw(spriteBatch, pixelTexture);
    }
}
