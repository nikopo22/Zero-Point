using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using ZeroPoint.Abilities;
using ZeroPoint.Utils;

namespace ZeroPoint.Entities;

public class Player
{
    private Texture2D _texture;

    private int _currentFrame = 0;
    private float _animationTimer = 0f;

    private const float FRAME_TIME = 0.1f;

    private int _frameWidth = 30;
    private int _frameHeight = 64;

    private int _frameCount = 6;
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
    
    // Оптимизация: кэшируем квадрат радиуса сканирования
    private static readonly float SCAN_RADIUS_SQUARED = Constants.SCAN_RADIUS * Constants.SCAN_RADIUS;
    private static readonly float METAL_ADHESION_MARGIN = 10f;

    // конструктор
    public Player(Vector2 startPosition, Texture2D texture)
    {
        _texture = texture;
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

        // подсветка скрытых платформ (оптимизировано: используем DistanceSquared)
        UpdateHiddenPlatforms(hiddenPlatforms);

        // движение (оптимизировано: более чистый код)
        UpdateMovement(keyboardState);
        Animate(gameTime);
        //гравитация
        if (!(IsOnMetal && MagnetAbility.IsActive))
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + Constants.GRAVITY * deltaTime);
        }

        Position += Velocity * deltaTime;

        //прилипание к металлическим поверхностям
        UpdateMetalAdherence(metalSurfaces);

        previousKeyboardState = keyboardState;
    }

    /// <summary>
    /// Обновляет видимость скрытых платформ на основе активности сканера
    /// </summary>
    private void UpdateHiddenPlatforms(List<HiddenPlatform> hiddenPlatforms)
    {
        if (ScanAbility.IsActive)
        {
            foreach (var hidden in hiddenPlatforms)
            {
                // Оптимизация: используем DistanceSquared вместо Distance - избегаем sqrt
                Vector2 hiddenCenter = new Vector2(
                    hidden.Bounds.X + hidden.Bounds.Width / 2f,
                    hidden.Bounds.Y + hidden.Bounds.Height / 2f
                );
                
                float distanceSquared = Vector2.DistanceSquared(Position, hiddenCenter);
                hidden.IsRevealed = distanceSquared <= SCAN_RADIUS_SQUARED;
            }
        }
        else
        {
            // Скрываем все платформы (оптимизировано: избегаем foreach для очистки)
            foreach (var hidden in hiddenPlatforms)
            {
                hidden.IsRevealed = false;
            }
        }
    }

    /// <summary>
    /// Обновляет движение игрока на основе ввода
    /// </summary>
    private void UpdateMovement(KeyboardState keyboardState)
    {
        // Оптимизация: вычисляем направление более чистым способом
        float moveDirection = 0;
        if (keyboardState.IsKeyDown(Keys.A))
            moveDirection -= 1;
        if (keyboardState.IsKeyDown(Keys.D))
            moveDirection += 1;

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
    }

    /// <summary>
    /// Обновляет прилипание к металлическим поверхностям
    /// </summary>
    private void UpdateMetalAdherence(List<MetalSurface> metalSurfaces)
    {
        IsOnMetal = false;

        if (!MagnetAbility.IsActive)
            return;

        foreach (var metal in metalSurfaces)
        {
            if (!Bounds.Intersects(metal.Bounds))
                continue;

            // Проверяем направление столкновения и применяем эффект магнита
            if (Velocity.Y >= 0 && PreviousBounds.Bottom <= metal.Bounds.Top + METAL_ADHESION_MARGIN)
            {
                Position = new Vector2(Position.X, metal.Bounds.Top - Bounds.Height);
                Velocity = new Vector2(Velocity.X, 0);
                IsOnMetal = true;
                IsGrounded = true;
            }
            else if (Velocity.Y <= 0 && PreviousBounds.Top >= metal.Bounds.Bottom - METAL_ADHESION_MARGIN)
            {
                Position = new Vector2(Position.X, metal.Bounds.Bottom);
                Velocity = new Vector2(Velocity.X, 0);
                IsOnMetal = true;
            }
            else if (Velocity.X >= 0 && PreviousBounds.Right <= metal.Bounds.Left + METAL_ADHESION_MARGIN)
            {
                Position = new Vector2(metal.Bounds.Left - Bounds.Width, Position.Y);
                Velocity = new Vector2(0, Velocity.Y);
                IsOnMetal = true;
            }
            else if (Velocity.X <= 0 && PreviousBounds.Left >= metal.Bounds.Right - METAL_ADHESION_MARGIN)
            {
                Position = new Vector2(metal.Bounds.Right, Position.Y);
                Velocity = new Vector2(0, Velocity.Y);
                IsOnMetal = true;
            }
        }
    }


    public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
    {
        Rectangle sourceRect = new Rectangle(
            _currentFrame * _frameWidth,
            0,
            _frameWidth,
            _frameHeight
        );

        spriteBatch.Draw(
            _texture,
            Position,
            sourceRect,
            Color.White
        );
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

    private void Animate(GameTime gameTime)
    {
        _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_animationTimer >= FRAME_TIME)
        {
            _currentFrame++;
            _animationTimer = 0f;

            if (_currentFrame >= _frameCount)
                _currentFrame = 0;
        }
    }
}
