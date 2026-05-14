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
    private Color normalColor;
    private Color hoverColor;
    private Color overlayColor;
    private SpriteFont font;

    public PauseMenuState(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        this.font = font;

        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        int buttonWidth = 220;
        int buttonHeight = 50;
        int centerX = 1280 / 2 - buttonWidth / 2;
        int startY = 300;
        int buttonSpacing = 70;

        resumeButton = new Rectangle(centerX, startY, buttonWidth, buttonHeight);
        menuButton = new Rectangle(centerX, startY + buttonSpacing, buttonWidth, buttonHeight);
        exitButton = new Rectangle(centerX, startY + buttonSpacing * 2, buttonWidth, buttonHeight);

        normalColor = new Color(70, 70, 130, 220);
        hoverColor = new Color(120, 120, 200, 220);
        overlayColor = new Color(0, 0, 0, 180);

        previousMouseState = Mouse.GetState();
    }

    public void Update(out bool resumeClicked, out bool menuClicked, out bool exitClicked)
    {
        MouseState currentMouseState = Mouse.GetState();
        int mouseX = currentMouseState.X;
        int mouseY = currentMouseState.Y;

        resumeClicked = false;
        menuClicked = false;
        exitClicked = false;

        if (resumeButton.Contains(mouseX, mouseY) &&
            currentMouseState.LeftButton == ButtonState.Released &&
            previousMouseState.LeftButton == ButtonState.Pressed)
        {
            resumeClicked = true;
        }

        if (menuButton.Contains(mouseX, mouseY) &&
            currentMouseState.LeftButton == ButtonState.Released &&
            previousMouseState.LeftButton == ButtonState.Pressed)
        {
            menuClicked = true;
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
        // Затемнение
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, 1280, 720), overlayColor);

        // Рамка меню паузы
        int menuWidth = 400;
        int menuHeight = 300;
        int menuX = 1280 / 2 - menuWidth / 2;
        int menuY = 720 / 2 - menuHeight / 2;

        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, menuWidth, menuHeight), new Color(30, 30, 50, 230));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, menuWidth, 3), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY + menuHeight - 3, menuWidth, 3), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, 3, menuHeight), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX + menuWidth - 3, menuY, 3, menuHeight), new Color(150, 100, 200));

        // Заголовок ПАУЗА
        string title = "ПАУЗА";
        Vector2 titleSize = font.MeasureString(title);
        spriteBatch.DrawString(font, title, new Vector2(1280 / 2 - titleSize.X / 2, menuY + 30), new Color(220, 180, 255));

        // Разделитель
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX + 20, menuY + 80, menuWidth - 40, 2), new Color(100, 100, 150));

        bool isHoverResume = resumeButton.Contains(Mouse.GetState().X, Mouse.GetState().Y);
        bool isHoverMenu = menuButton.Contains(Mouse.GetState().X, Mouse.GetState().Y);
        bool isHoverExit = exitButton.Contains(Mouse.GetState().X, Mouse.GetState().Y);

        // Кнопка ПРОДОЛЖИТЬ
        DrawButton(spriteBatch, resumeButton, isHoverResume, "ПРОДОЛЖИТЬ");

        // Кнопка ГЛАВНОЕ МЕНЮ
        DrawButton(spriteBatch, menuButton, isHoverMenu, "ГЛАВНОЕ МЕНЮ");

        // Кнопка ВЫХОД
        DrawButton(spriteBatch, exitButton, isHoverExit, "ВЫХОД");
    }

    private void DrawButton(SpriteBatch spriteBatch, Rectangle button, bool isHovered, string text)
    {
        Color buttonColor = isHovered ? hoverColor : normalColor;

        spriteBatch.Draw(pixelTexture, button, buttonColor);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X, button.Y, button.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X, button.Y + button.Height - 2, button.Width, 2), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X, button.Y, 2, button.Height), Color.White);
        spriteBatch.Draw(pixelTexture, new Rectangle(button.X + button.Width - 2, button.Y, 2, button.Height), Color.White);

        Vector2 textSize = font.MeasureString(text);
        spriteBatch.DrawString(font, text, new Vector2(button.X + (button.Width - textSize.X) / 2, button.Y + (button.Height - textSize.Y) / 2), Color.White);
    }
}
