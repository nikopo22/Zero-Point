using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZeroPoint.States;

public class MenuState
{
    private Rectangle playButton;
    private Rectangle exitButton;
    private Texture2D pixelTexture;
    private MouseState previousMouseState;
    private Color normalColor;
    private Color hoverColor;
    private SpriteFont font;

    public MenuState(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        this.font = font;

        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        int buttonWidth = 220;
        int buttonHeight = 60;
        int centerX = 1280 / 2 - buttonWidth / 2;

        playButton = new Rectangle(centerX, 320, buttonWidth, buttonHeight);
        exitButton = new Rectangle(centerX, 420, buttonWidth, buttonHeight);

        normalColor = new Color(70, 70, 130);
        hoverColor = new Color(120, 120, 200);

        previousMouseState = Mouse.GetState();
    }

    public void Update(out bool playClicked, out bool exitClicked)
    {
        MouseState currentMouseState = Mouse.GetState();
        int mouseX = currentMouseState.X;
        int mouseY = currentMouseState.Y;

        playClicked = false;
        exitClicked = false;

        if (playButton.Contains(mouseX, mouseY) &&
            currentMouseState.LeftButton == ButtonState.Released &&
            previousMouseState.LeftButton == ButtonState.Pressed)
        {
            playClicked = true;
        }

        if (exitButton.Contains(mouseX, mouseY) &&
            currentMouseState.LeftButton == ButtonState.Released &&
            previousMouseState.LeftButton == ButtonState.Pressed)
        {
            exitClicked = true;
        }

        previousMouseState = currentMouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Фон
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, 1280, 720), new Color(25, 25, 40));

        // Декоративные линии
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 250, 1280, 2), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 260, 1280, 1), new Color(100, 100, 150));

        // Название ZERO POINT
        string title = "ZERO POINT";
        Vector2 titleSize = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2(1280 / 2 - titleSize.X / 2, 100), new Color(220, 180, 255));

        bool isHoverPlay = playButton.Contains(Mouse.GetState().X, Mouse.GetState().Y);
        bool isHoverExit = exitButton.Contains(Mouse.GetState().X, Mouse.GetState().Y);

        // Кнопка ИГРАТЬ
        Color playColor = isHoverPlay ? hoverColor : normalColor;
        spriteBatch.Draw(pixelTexture, playButton, playColor);
        spriteBatch.Draw(pixelTexture, new Rectangle(playButton.X, playButton.Y, playButton.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(playButton.X, playButton.Y + playButton.Height - 2, playButton.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(playButton.X, playButton.Y, 2, playButton.Height), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(playButton.X + playButton.Width - 2, playButton.Y, 2, playButton.Height), Color.White);

        string playText = "ИГРАТЬ";
        Vector2 playSize = font.MeasureString(playText);
        spriteBatch.DrawString(font, playText, new Vector2(playButton.X + (playButton.Width - playSize.X) / 2, playButton.Y + (playButton.Height - playSize.Y) / 2), Color.White);

        // Кнопка ВЫХОД
        Color exitColor = isHoverExit ? hoverColor : normalColor;
        spriteBatch.Draw(pixelTexture, exitButton, exitColor);
        spriteBatch.Draw(pixelTexture, new Rectangle(exitButton.X, exitButton.Y, exitButton.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(exitButton.X, exitButton.Y + exitButton.Height - 2, exitButton.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(exitButton.X, exitButton.Y, 2, exitButton.Height), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(exitButton.X + exitButton.Width - 2, exitButton.Y, 2, exitButton.Height), Color.White);

        string exitText = "ВЫХОД";
        Vector2 exitSize = font.MeasureString(exitText);
        spriteBatch.DrawString(font, exitText, new Vector2(exitButton.X + (exitButton.Width - exitSize.X) / 2, exitButton.Y + (exitButton.Height - exitSize.Y) / 2), Color.White);
    }
}