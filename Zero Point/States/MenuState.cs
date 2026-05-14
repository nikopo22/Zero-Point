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
    private Color playColor;
    private Color exitColor;
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
        playColor = normalColor;
        exitColor = normalColor;
        previousMouseState = Mouse.GetState();
    }

    public void Update(out bool playClicked, out bool exitClicked)
    {
        MouseState current = Mouse.GetState();
        int mx = current.X, my = current.Y;

        playClicked = false;
        exitClicked = false;

        if (playButton.Contains(mx, my))
        {
            playColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                playClicked = true;
        }
        else
        {
            playColor = normalColor;
        }

        if (exitButton.Contains(mx, my))
        {
            exitColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                exitClicked = true;
        }
        else
        {
            exitColor = normalColor;
        }

        previousMouseState = current;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, 1280, 720), new Color(25, 25, 40));

        string title = "ZERO POINT";
        Vector2 titleSize = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2(1280 / 2 - titleSize.X / 2, 100), new Color(220, 180, 255));

        DrawButton(spriteBatch, playButton, playColor, "ИГРАТЬ");
        DrawButton(spriteBatch, exitButton, exitColor, "ВЫХОД");
    }

    private void DrawButton(SpriteBatch spriteBatch, Rectangle button, Color fillColor, string text)
    {
        spriteBatch.Draw(pixelTexture, button, fillColor);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X, button.Y, button.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X, button.Y + button.Height - 2, button.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X, button.Y, 2, button.Height), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X + button.Width - 2, button.Y, 2, button.Height), Color.White);

        Vector2 textSize = font.MeasureString(text);
        spriteBatch.DrawString(font, text, new Vector2(button.X + (button.Width - textSize.X) / 2, button.Y + (button.Height - textSize.Y) / 2), Color.White);
    }
}
