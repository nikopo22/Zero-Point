using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.UI;

namespace ZeroPoint.States;

public class PauseMenuState
{
    private readonly Button resumeButton;
    private readonly Button menuButton;
    private readonly Button exitButton;
    private readonly Texture2D pixelTexture;
    private MouseState previousMouseState;
    private readonly SpriteFont font;
    private readonly Color overlayColor;

    public PauseMenuState(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        this.font = font;

        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        int btnW = 220, btnH = 50;
        int centerX = 1280 / 2 - btnW / 2;
        int startY = 320;
        int spacing = 70;

        resumeButton = new Button(new Rectangle(centerX, startY, btnW, btnH), "ПРОДОЛЖИТЬ", font);
        menuButton = new Button(new Rectangle(centerX, startY + spacing, btnW, btnH), "ГЛАВНОЕ МЕНЮ", font);
        exitButton = new Button(new Rectangle(centerX, startY + spacing * 2, btnW, btnH), "ВЫХОД", font);

        overlayColor = new Color(0, 0, 0, 180);
        previousMouseState = Mouse.GetState();
    }

    public void Update(out bool resumeClicked, out bool menuClicked, out bool exitClicked)
    {
        MouseState current = Mouse.GetState();

        resumeButton.Update(current);
        menuButton.Update(current);
        exitButton.Update(current);

        resumeClicked = resumeButton.WasReleased(current, previousMouseState);
        menuClicked = menuButton.WasReleased(current, previousMouseState);
        exitClicked = exitButton.WasReleased(current, previousMouseState);

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

        resumeButton.Draw(spriteBatch, pixelTexture);
        menuButton.Draw(spriteBatch, pixelTexture);
        exitButton.Draw(spriteBatch, pixelTexture);
    }
}
