using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.UI;

namespace ZeroPoint.States;

public class VictoryState
{
    private const int ScreenWidth = 1280;
    private const int ScreenHeight = 720;
    private const int ButtonWidth = 260;
    private const int ButtonHeight = 60;
    private const int ButtonSpacing = 80;
    private const int MenuWidth = 720;
    private const int MenuTop = 120;
    private const int BorderThickness = 4;

    private readonly Button[] _buttons;
    private readonly Texture2D _pixelTexture;
    private readonly SpriteFont _font;
    private readonly Color _overlayColor;
    private readonly Rectangle _menuRectangle;
    private MouseState _previousMouseState;
    private int _activeCompletedIndex = -1;

    public VictoryState(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        _font = font;
        _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });

        int centerX = ScreenWidth / 2 - ButtonWidth / 2;
        int startY = 320;

        _buttons = new[]
        {
            new Button(new Rectangle(centerX, startY, ButtonWidth, ButtonHeight), "ПРОДОЛЖИТЬ", font),
            new Button(new Rectangle(centerX, startY + ButtonSpacing, ButtonWidth, ButtonHeight), "ПОВТОРИТЬ", font),
            new Button(new Rectangle(centerX, startY + ButtonSpacing * 2, ButtonWidth, ButtonHeight), "ГЛАВНОЕ МЕНЮ", font)
        };

        int menuHeight = _buttons[^1].Bounds.Bottom + ButtonSpacing - MenuTop;
        int menuX = ScreenWidth / 2 - MenuWidth / 2;
        _menuRectangle = new Rectangle(menuX, MenuTop, MenuWidth, menuHeight);

        _overlayColor = new Color(0, 0, 0, 190);
        _previousMouseState = Mouse.GetState();
    }

    public void Update(int completedLevelIndex, out bool continueClicked, out bool retryClicked, out bool menuClicked)
    {
        MouseState current = Mouse.GetState();

        // Recreate/relayout buttons when the completed level index changes
        if (completedLevelIndex != _activeCompletedIndex)
        {
            _activeCompletedIndex = completedLevelIndex;
            int centerX = ScreenWidth / 2 - ButtonWidth / 2;
            int startY = 320;

            if (completedLevelIndex == 3)
            {
                // Last level: show two buttons (start over and main menu)
                _buttons[0] = new Button(new Rectangle(centerX, startY, ButtonWidth, ButtonHeight), "НАЧАТЬ СНАЧАЛА", _font);
                _buttons[1] = new Button(new Rectangle(centerX, startY + ButtonSpacing, ButtonWidth, ButtonHeight), "ГЛАВНОЕ МЕНЮ", _font);
                // hide/unused slot
                _buttons[2] = new Button(new Rectangle(-500, -500, 0, 0), "", _font);
            }
            else
            {
                // Default layout with three options
                _buttons[0] = new Button(new Rectangle(centerX, startY, ButtonWidth, ButtonHeight), "ПРОДОЛЖИТЬ", _font);
                _buttons[1] = new Button(new Rectangle(centerX, startY + ButtonSpacing, ButtonWidth, ButtonHeight), "ПОВТОРИТЬ", _font);
                _buttons[2] = new Button(new Rectangle(centerX, startY + ButtonSpacing * 2, ButtonWidth, ButtonHeight), "ГЛАВНОЕ МЕНЮ", _font);
            }
        }

        foreach (Button button in _buttons)
        {
            if (button != null)
                button.Update(current);
        }

        if (completedLevelIndex == 3)
        {
            // For last level: continueClicked not used, retryClicked -> start over, menuClicked -> menu
            continueClicked = false;
            retryClicked = _buttons[0].WasReleased(current, _previousMouseState);
            menuClicked = _buttons[1].WasReleased(current, _previousMouseState);
        }
        else
        {
            continueClicked = _buttons[0].WasReleased(current, _previousMouseState);
            retryClicked = _buttons[1].WasReleased(current, _previousMouseState);
            menuClicked = _buttons[2].WasReleased(current, _previousMouseState);
        }

        _previousMouseState = current;
    }

    public void Draw(SpriteBatch spriteBatch, int completedLevelIndex)
    {
        spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, ScreenWidth, ScreenHeight), _overlayColor);
        DrawPanel(spriteBatch);
        DrawText(spriteBatch, completedLevelIndex);

        foreach (Button button in _buttons)
        {
            button.Draw(spriteBatch, _pixelTexture);
        }
    }

    private void DrawPanel(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_pixelTexture, _menuRectangle, new Color(30, 30, 50, 240));
        spriteBatch.Draw(_pixelTexture, new Rectangle(_menuRectangle.X, _menuRectangle.Y, _menuRectangle.Width, BorderThickness), Color.White);
        spriteBatch.Draw(_pixelTexture, new Rectangle(_menuRectangle.X, _menuRectangle.Bottom - BorderThickness, _menuRectangle.Width, BorderThickness), Color.White);
        spriteBatch.Draw(_pixelTexture, new Rectangle(_menuRectangle.X, _menuRectangle.Y, BorderThickness, _menuRectangle.Height), Color.White);
        spriteBatch.Draw(_pixelTexture, new Rectangle(_menuRectangle.Right - BorderThickness, _menuRectangle.Y, BorderThickness, _menuRectangle.Height), Color.White);
    }

    private void DrawText(SpriteBatch spriteBatch, int completedLevelIndex)
    {
        string title;
        if (completedLevelIndex == 3)
            title = "Победа!";
        else
            title = $"Уровень {completedLevelIndex} пройден!";
        Vector2 titleSize = _font.MeasureString(title);
        Vector2 titlePosition = new Vector2(ScreenWidth / 2 - titleSize.X / 2, _menuRectangle.Y + 40);
        spriteBatch.DrawString(_font, title, titlePosition, Color.LightGreen);
        if (completedLevelIndex != 3)
        {
            string subtitle = "Вы можете продолжить или вернуться в меню";
            Vector2 subtitleSize = _font.MeasureString(subtitle);
            Vector2 subtitlePosition = new Vector2(ScreenWidth / 2 - subtitleSize.X / 2, _menuRectangle.Y + 110);
            spriteBatch.DrawString(_font, subtitle, subtitlePosition, Color.White);
        }
    }
}
