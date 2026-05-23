using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.UI;

namespace ZeroPoint.States;

public class VictoryState
{
    private readonly Button _continueButton;
    private readonly Button _menuButton;
    private readonly Texture2D _pixelTexture;
    private MouseState _previousMouseState;
    private readonly SpriteFont _font;
    private readonly Color _overlayColor;

    public VictoryState(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        _font = font;

        _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });

        int buttonWidth = 260;
        int buttonHeight = 60;
        int centerX = 1280 / 2 - buttonWidth / 2;
        int startY = 360;
        int spacing = 90;

        _continueButton = new Button(new Rectangle(centerX, startY, buttonWidth, buttonHeight), "ПРОДОЛЖИТЬ", font);
        _menuButton = new Button(new Rectangle(centerX, startY + spacing, buttonWidth, buttonHeight), "ГЛАВНОЕ МЕНЮ", font);

        _overlayColor = new Color(0, 0, 0, 190);
        _previousMouseState = Mouse.GetState();
    }

    public void Update(out bool continueClicked, out bool menuClicked)
    {
        MouseState current = Mouse.GetState();

        _continueButton.Update(current);
        _menuButton.Update(current);

        continueClicked = _continueButton.WasReleased(current, _previousMouseState);
        menuClicked = _menuButton.WasReleased(current, _previousMouseState);

        _previousMouseState = current;
    }

    public void Draw(SpriteBatch spriteBatch, int completedLevelIndex)
    {
        spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, 1280, 720), _overlayColor);

        int menuWidth = 720;
        int menuHeight = 420;
        int menuX = 1280 / 2 - menuWidth / 2;
        int menuY = 120;

        spriteBatch.Draw(_pixelTexture, new Rectangle(menuX, menuY, menuWidth, menuHeight), new Color(30, 30, 50, 240));
        spriteBatch.Draw(_pixelTexture, new Rectangle(menuX, menuY, menuWidth, 4), Color.White);
        spriteBatch.Draw(_pixelTexture, new Rectangle(menuX, menuY + menuHeight - 4, menuWidth, 4), Color.White);
        spriteBatch.Draw(_pixelTexture, new Rectangle(menuX, menuY, 4, menuHeight), Color.White);
        spriteBatch.Draw(_pixelTexture, new Rectangle(menuX + menuWidth - 4, menuY, 4, menuHeight), Color.White);

        string title = $"Уровень {completedLevelIndex} пройден!";
        Vector2 titleSize = _font.MeasureString(title);
        spriteBatch.DrawString(_font, title, new Vector2(1280 / 2 - titleSize.X / 2, menuY + 40), Color.LightGreen);

        string subtitle = "Вы можете продолжить или вернуться в меню";
        Vector2 subtitleSize = _font.MeasureString(subtitle);
        spriteBatch.DrawString(_font, subtitle, new Vector2(1280 / 2 - subtitleSize.X / 2, menuY + 110), Color.White);

        _continueButton.Draw(spriteBatch, _pixelTexture);
        _menuButton.Draw(spriteBatch, _pixelTexture);
    }
}
