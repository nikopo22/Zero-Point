using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZeroPoint.States;

public class MenuState
{
    private Rectangle playButton;
    private Rectangle exitButton;
    private Texture2D pixelTexture;
    private MouseState previousMouse;

    private Color playColor;
    private Color exitColor;
    private Color normalColor;
    private Color hoverColor;

    public MenuState(GraphicsDevice graphicsDevice)
    {
        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        int btnW = 200, btnH = 50;
        int centerX = 1280 / 2 - btnW / 2;

        playButton = new Rectangle(centerX, 300, btnW, btnH);
        exitButton = new Rectangle(centerX, 380, btnW, btnH);

        normalColor = new Color(70, 70, 130);
        hoverColor = new Color(120, 120, 200);

        playColor = normalColor;
        exitColor = normalColor;

        previousMouse = Mouse.GetState();
    }

    public void Update(out bool playClicked, out bool exitClicked)
    {
        MouseState current = Mouse.GetState();
        int mx = current.X, my = current.Y;

        playClicked = false;
        exitClicked = false;

        //проверка ИГРАТЬ
        if (playButton.Contains(mx, my))
        {
            playColor = hoverColor;
            if (current.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                playClicked = true;
        }
        else
        {
            playColor = normalColor;
        }

        //проверка ВЫХОД
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
        // фон
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, 1280, 720), new Color(25, 25, 40));

        // титл
        int titleW = 400, titleH = 60;
        int titleX = 1280 / 2 - titleW / 2;
        spriteBatch.Draw(pixelTexture, new Rectangle(titleX, 100, titleW, titleH), new Color(150, 100, 200));
        spriteBatch.Draw(pixelTexture, new Rectangle(titleX + 5, 105, titleW - 10, titleH - 10), new Color(35, 35, 55));

        //играть
        spriteBatch.Draw(pixelTexture, playButton, playColor);
        DrawBorder(spriteBatch, playButton);

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