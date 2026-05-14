using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZeroPoint.States;

public class PauseMenuState
{
    private Rectangle resumeButton;
    private Rectangle menuButton;
    private Rectangle exitButton;
    private Texture2D pixelTexture;
    private MouseState previousMouseState;
    private Color resumeColor;
    private Color menuColor;
    private Color exitColor;
    private Color normalColor;
    private Color hoverColor;
    private Color overlayColor;
    private SpriteFont font;

    public PauseMenuState(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        this.font = font;

        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        int btnW = 220, btnH = 50;
        int centerX = 1280 / 2 - btnW / 2;
        int startY = 320;
        int spacing = 70;

        resumeButton = new Rectangle(centerX, startY, btnW, btnH);
        menuButton = new Rectangle(centerX, startY + spacing, btnW, btnH);
        exitButton = new Rectangle(centerX, startY + spacing * 2, btnW, btnH);

        normalColor = new Color(70, 70, 130);
        hoverColor = new Color(120, 120, 200);
        overlayColor = new Color(0, 0, 0, 180);

        resumeColor = normalColor;
        menuColor = normalColor;
        exitColor = normalColor;
        previousMouseState = Mouse.GetState();
    }

    public void Update(out bool resumeClicked, out bool menuClicked, out bool exitClicked)
    {
        MouseState current = Mouse.GetState();
        int mx = current.X, my = current.Y;

        resumeClicked = false;
        menuClicked = false;
        exitClicked = false;

        if (resumeButton.Contains(mx, my))
        {
            resumeColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                resumeClicked = true;
        }
        else
        {
            resumeColor = normalColor;
        }

        if (menuButton.Contains(mx, my))
        {
            menuColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                menuClicked = true;
        }
        else
        {
            menuColor = normalColor;
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
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, 1280, 720), overlayColor);

        int menuWidth = 400;
        int menuHeight = 320;
        int menuX = 1280 / 2 - menuWidth / 2;
        int menuY = 720 / 2 - menuHeight / 2;

        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, menuWidth, menuHeight), new Color(30, 30, 50, 230));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, menuWidth, 3), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY + menuHeight - 3, menuWidth, 3), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, 3, menuHeight), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX + menuWidth - 3, menuY, 3, menuHeight), new Color(150, 100, 200));

        string title = "ПАУЗА";
        Vector2 titleSize = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2(1280 / 2 - titleSize.X / 2, menuY + 30), new Color(220, 180, 255));

        spriteBatch.Draw(pixelTexture, new Rectangle(menuX + 20, menuY + 90, menuWidth - 40, 2), new Color(100, 100, 150));

        DrawButton(spriteBatch, resumeButton, resumeColor, "ПРОДОЛЖИТЬ");
        DrawButton(spriteBatch, menuButton, menuColor, "ГЛАВНОЕ МЕНЮ");
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
