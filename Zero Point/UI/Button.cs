using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZeroPoint.UI;

/// <summary>
/// Интерактивная кнопка для меню
/// </summary>
public class Button
{
    // === ОСНОВНЫЕ СВОЙСТВА ===
    public Rectangle Bounds { get; private set; }      // Прямоугольник кнопки
    public string Text { get; private set; }           // Текст на кнопке
    public bool IsHovered { get; private set; }        // Наведена ли мышь
    public bool IsClicked { get; private set; }        // Нажата ли кнопка

    // === ЦВЕТА ===
    private Color normalColor;      // Обычное состояние
    private Color hoverColor;       // При наведении мыши
    private Color clickColor;       // При нажатии
    private Color currentColor;     // Текущий цвет

    // === ШРИФТ ===
    private SpriteFont font;

    // === КОНСТРУКТОР ===
    public Button(Rectangle bounds, string text, SpriteFont font)
    {
        Bounds = bounds;
        Text = text;
        this.font = font;

        // Настройка цветов
        normalColor = new Color(100, 100, 150);      // Тёмно-синий
        hoverColor = new Color(150, 150, 200);       // Светло-синий
        clickColor = new Color(80, 80, 120);         // Очень тёмный
        currentColor = normalColor;

        IsHovered = false;
        IsClicked = false;
    }

    /// <summary>
    /// Обновление состояния кнопки (проверка наведения и нажатия)
    /// </summary>
    public void Update(MouseState mouseState)
    {
        // Проверяем, находится ли курсор мыши внутри кнопки
        if (Bounds.Contains(mouseState.X, mouseState.Y))
        {
            IsHovered = true;
            currentColor = hoverColor;

            // Проверяем нажатие левой кнопки мыши
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                IsClicked = true;
                currentColor = clickColor;
            }
            else
            {
                IsClicked = false;
            }
        }
        else
        {
            IsHovered = false;
            IsClicked = false;
            currentColor = normalColor;
        }
    }

    /// <summary>
    /// Проверяет, была ли кнопка отпущена после нажатия
    /// </summary>
    public bool WasReleased(MouseState currentMouse, MouseState previousMouse)
    {
        return Bounds.Contains(currentMouse.X, currentMouse.Y) &&
               currentMouse.LeftButton == ButtonState.Released &&
               previousMouse.LeftButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Отрисовка кнопки
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        // Рисуем фон кнопки
        spriteBatch.Draw(pixelTexture, Bounds, currentColor);

        // Рисуем обводку
        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, 2), Color.White);  // Верхняя
        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - 2, Bounds.Width, 2), Color.White);  // Нижняя
        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X, Bounds.Y, 2, Bounds.Height), Color.White);  // Левая
        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X + Bounds.Width - 2, Bounds.Y, 2, Bounds.Height), Color.White);  // Правая

        // Рисуем текст (если есть шрифт)
        if (font != null)
        {
            // Центрируем текст по кнопке
            Vector2 textSize = font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                Bounds.X + (Bounds.Width - textSize.X) / 2,
                Bounds.Y + (Bounds.Height - textSize.Y) / 2
            );

            spriteBatch.DrawString(font, Text, textPosition, Color.White);
        }
    }
}