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
    private MouseState previousMouse;

    private Color resumeColor;
    private Color menuColor;
    private Color exitColor;
    private Color normalColor;
    private Color hoverColor;
    private Color overlayColor;

    public PauseMenuState(GraphicsDevice graphicsDevice)
    {
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

        previousMouse = Mouse.GetState();
    }

    public void Update(out bool resumeClicked, out bool menuClicked, out bool exitClicked)
    {
        MouseState current = Mouse.GetState();
        int mx = current.X, my = current.Y;

        resumeClicked = false;
        menuClicked = false;
        exitClicked = false;

        //продолжить
        if (resumeButton.Contains(mx, my))
        {
            resumeColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                resumeClicked = true;
        }
        else
        {
            resumeColor = normalColor;
        }

        //глав меню
        if (menuButton.Contains(mx, my))
        {
            menuColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                menuClicked = true;
        }
        else
        {
            menuColor = normalColor;
        }

        //выход
        if (exitButton.Contains(mx, my))
        {
            exitColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                exitClicked = true;
        }
        else
        {
            exitColor = normalColor;
        }

        previousMouse = current;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, 1280, 720), overlayColor);

        int menuW = 400, menuH = 320;
        int menuX = 1280 / 2 - menuW / 2;
        int menuY = 720 / 2 - menuH / 2;

        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, menuW, menuH), new Color(30, 30, 50, 230));

        //рамка
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, menuW, 3), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY + menuH - 3, menuW, 3), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX, menuY, 3, menuH), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(menuX + menuW - 3, menuY, 3, menuH), new Color(150, 100, 200));

        //заголовок
        int titleW = 120, titleH = 40;
        int titleX = 1280 / 2 - titleW / 2;
        int titleY = menuY + 30;
        spriteBatch.Draw(pixelTexture, new Rectangle(titleX, titleY, titleW, titleH), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(titleX + 5, titleY + 5, titleW - 10, titleH - 10), new Color(50, 50, 80));

        spriteBatch.Draw(pixelTexture, new Rectangle(menuX + 20, menuY + 90, menuW - 40, 2), new Color(100, 100, 150));

        //продолжить
        spriteBatch.Draw(pixelTexture, resumeButton, resumeColor);
        DrawBorder(spriteBatch, resumeButton);

        //глав меню
        spriteBatch.Draw(pixelTexture, menuButton, menuColor);
        DrawBorder(spriteBatch, menuButton);

        //выход
        spriteBatch.Draw(pixelTexture, exitButton, exitColor);
        DrawBorder(spriteBatch, exitButton);
    }

    private void DrawBorder(SpriteBatch sb, Rectangle rect)
    {
        sb.Draw(pixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, 2), Color.White);
        sb.Draw(pixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - 2, rect.Width, 2), Color.White);
        sb.Draw(pixelTexture, new Rectangle(rect.X, rect.Y, 2, rect.Height), Color.White);
        sb.Draw(pixelTexture, new Rectangle(rect.X + rect.Width - 2, rect.Y, 2, rect.Height), Color.White);
    }
}
