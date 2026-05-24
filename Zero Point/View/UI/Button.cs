using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZeroPoint.UI;

public class Button
{
    public Rectangle Bounds { get; private set; }      
    public string Text { get; private set; }          
    public bool IsHovered { get; private set; }      
    public bool IsClicked { get; private set; }      

    private Color normalColor;     
    private Color hoverColor;       
    private Color clickColor;      
    private Color currentColor;     

    private SpriteFont font;

    public Button(Rectangle bounds, string text, SpriteFont font)
    {
        Bounds = bounds;
        Text = text;
        this.font = font;

        normalColor = new Color(100, 100, 150);     
        hoverColor = new Color(150, 150, 200);     
        clickColor = new Color(80, 80, 120);        
        currentColor = normalColor;

        IsHovered = false;
        IsClicked = false;
    }

    public void Update(MouseState mouseState)
    {
        if (Bounds.Contains(mouseState.X, mouseState.Y))
        {
            IsHovered = true;
            currentColor = hoverColor;

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

    public bool WasReleased(MouseState currentMouse, MouseState previousMouse)
    {
        return Bounds.Contains(currentMouse.X, currentMouse.Y) &&
               currentMouse.LeftButton == ButtonState.Released &&
               previousMouse.LeftButton == ButtonState.Pressed;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        spriteBatch.Draw(pixelTexture, Bounds, currentColor);

        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, 2), Color.White); 
        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - 2, Bounds.Width, 2), Color.White); 
        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X, Bounds.Y, 2, Bounds.Height), Color.White);  
        spriteBatch.Draw(pixelTexture,
            new Rectangle(Bounds.X + Bounds.Width - 2, Bounds.Y, 2, Bounds.Height), Color.White);  

        if (font != null)
        {
            Vector2 textSize = font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                Bounds.X + (Bounds.Width - textSize.X) / 2,
                Bounds.Y + (Bounds.Height - textSize.Y) / 2
            );

            spriteBatch.DrawString(font, Text, textPosition, Color.White);
        }
    }
}