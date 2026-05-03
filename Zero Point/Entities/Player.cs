using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using ZeroPoint.Abilities;
using ZeroPoint.Utils;

namespace ZeroPoint.Entities;

public class Player
{
    //св-ва
    public Vector2 Position { get; set; }      //координаты игрока
    public Vector2 Velocity { get; set; }      //скорость
    public bool IsGrounded { get; set; }       
    public bool IsOnMetal { get; set; }        

    //способности
    public MagnetAbility MagnetAbility { get; private set; }
    public ScanAbility ScanAbility { get; private set; }

    //прямоугольник
    public Rectangle Bounds => new Rectangle(
        (int)Position.X,
        (int)Position.Y,
        Constants.PLAYER_WIDTH,
        Constants.PLAYER_HEIGHT
    );

    public Rectangle PreviousBounds { get; private set; }

    private Color normalColor;     
    private Color magnetColor;    

    private KeyboardState previousKeyboardState;

    // конструктор
    public Player(Vector2 startPosition)
    {
        Position = startPosition;
        Velocity = Vector2.Zero;
        IsGrounded = false;
        IsOnMetal = false;

        normalColor = Color.Blue;
        magnetColor = new Color(100, 150, 255);

        MagnetAbility = new MagnetAbility();
        ScanAbility = new ScanAbility();
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState,
                       List<MetalSurface> metalSurfaces,
                       List<HiddenPlatform> hiddenPlatforms)
    {
        //сохраняем позицию прошлого кадра
        PreviousBounds = Bounds;

        //прошедшее время 
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        MagnetAbility.Update(gameTime);
        ScanAbility.Update(gameTime);

        //магнит:зажат Left Shift
        if (keyboardState.IsKeyDown(Keys.LeftShift))
        {
            MagnetAbility.Activate();
        }

        //сканер:нажатие E
        if (keyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E))
        {
            ScanAbility.Activate();
        }

        // подсветка скрытых платформ
        if (ScanAbility.IsActive)
        {
            foreach (var hidden in hiddenPlatforms)
            {
                //расстояние от игрока до платформы
                float distance = Vector2.Distance(Position,
                    new Vector2(hidden.Bounds.X + hidden.Bounds.Width / 2,
                                hidden.Bounds.Y + hidden.Bounds.Height / 2));

                if (distance <= Constants.SCAN_RADIUS)
                {
                    hidden.IsRevealed = true;
                }
            }
        }
        else
        {
            //скрываем все платформы
            foreach (var hidden in hiddenPlatforms)
            {
                hidden.IsRevealed = false;
            }
        }

        //движение
        float moveDirection = 0;
        if (keyboardState.IsKeyDown(Keys.A))
            moveDirection = -1;  // В=влево
        if (keyboardState.IsKeyDown(Keys.D))
            moveDirection = 1;   // вправо

        Velocity = new Vector2(moveDirection * Constants.PLAYER_SPEED, Velocity.Y);

        //прыжок
        if (keyboardState.IsKeyDown(Keys.W) &&
            previousKeyboardState.IsKeyUp(Keys.W) &&
            (IsGrounded || IsOnMetal))  
        {
            Velocity = new Vector2(Velocity.X, Constants.PLAYER_JUMP_FORCE);
            IsGrounded = false;
            IsOnMetal = false;
        }

        //гравитация
        if (!(IsOnMetal && MagnetAbility.IsActive))
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + Constants.GRAVITY * deltaTime);
        }

        Position += Velocity * deltaTime;

        //прилипание
        bool wasOnMetal = IsOnMetal;
        IsOnMetal = false;

        foreach (var metal in metalSurfaces)
        {
            if (Bounds.Intersects(metal.Bounds) && MagnetAbility.IsActive)
            {

                if (Velocity.Y >= 0 && PreviousBounds.Bottom <= metal.Bounds.Top + 10)
                {
                    Position = new Vector2(Position.X, metal.Bounds.Top - Bounds.Height);
                    Velocity = new Vector2(Velocity.X, 0);
                    IsOnMetal = true;
                    IsGrounded = true;
                }

                else if (Velocity.Y <= 0 && PreviousBounds.Top >= metal.Bounds.Bottom - 10)
                {
                    Position = new Vector2(Position.X, metal.Bounds.Bottom);
                    Velocity = new Vector2(Velocity.X, 0);
                    IsOnMetal = true;
                }

                else if (Velocity.X >= 0 && PreviousBounds.Right <= metal.Bounds.Left + 10)
                {
                    Position = new Vector2(metal.Bounds.Left - Bounds.Width, Position.Y);
                    Velocity = new Vector2(0, Velocity.Y);
                    IsOnMetal = true;
                }

                else if (Velocity.X <= 0 && PreviousBounds.Left >= metal.Bounds.Right - 10)
                {
                    Position = new Vector2(metal.Bounds.Right, Position.Y);
                    Velocity = new Vector2(0, Velocity.Y);
                    IsOnMetal = true;
                }
            }
        }

        previousKeyboardState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        //магнит активен - рисуем голубым, иначе синим
        Color drawColor = MagnetAbility.IsActive ? magnetColor : normalColor;
        spriteBatch.Draw(pixelTexture, Bounds, drawColor);
    }

    public void Reset(Vector2 respawnPosition)
    {
        Position = respawnPosition;
        Velocity = Vector2.Zero;
        IsGrounded = false;
        IsOnMetal = false;

        MagnetAbility.Deactivate();
        ScanAbility.Deactivate();
    }
}
