using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.Abilities;
using ZeroPoint.Core;
using ZeroPoint.Utils;
using System.Collections.Generic;

namespace ZeroPoint.Entities;

public class Player
{
    // === ОСНОВНЫЕ СВОЙСТВА ===
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsOnMetal { get; set; }
    
    // === СПОСОБНОСТИ ===
    public MagnetAbility MagnetAbility { get; private set; }
    public ScanAbility ScanAbility { get; private set; }
    
    // === СПРАЙТ-ЛИСТ И АНИМАЦИЯ ===
    private SpriteSheet _spriteSheet;
    private bool _facingRight = true;
    
    // Индексы кадров (согласно твоей нумерации)
    private int[] _idleFrames = { 0, 1, 2, 3, 4 };
    private int[] _walkFrames = { 5, 6, 7, 8, 9, 10 };
    private int[] _jumpFrames = { 11, 12, 13, 14, 15 };
    private int[] _landFrames = { 16, 17, 18 };
    
    private int _currentFrame;
    private int _currentAnimationIndex;
    private double _animationTimer;
    private double _animationSpeed = 0.08;
    
    // Состояние анимации
    private enum AnimationState { Idle, Walking, Jumping, Landing }
    private AnimationState _animationState;
    private bool _wasGrounded;
    private bool _justLanded;
    
    // === КОЛЛИЗИИ ===
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Constants.PLAYER_WIDTH, Constants.PLAYER_HEIGHT);
    public Rectangle PreviousBounds { get; private set; }
    
    // === ЦВЕТА ДЛЯ СПОСОБНОСТЕЙ ===
    private Color normalColor;
    private Color magnetColor;
    private KeyboardState _previousKeyboardState;
    
    // === КОНСТРУКТОР ===
    public Player(Vector2 startPosition, SpriteSheet spriteSheet)
    {
        Position = startPosition;
        Velocity = Vector2.Zero;
        IsGrounded = false;
        IsOnMetal = false;
        _spriteSheet = spriteSheet;
        
        normalColor = Color.White;
        magnetColor = new Color(100, 150, 255);
        
        MagnetAbility = new MagnetAbility();
        ScanAbility = new ScanAbility();
        
        _animationState = AnimationState.Idle;
        _currentFrame = _idleFrames[0];
        _currentAnimationIndex = 0;
        _wasGrounded = false;
        _justLanded = false;
    }
    
    // === ОБНОВЛЕНИЕ ===
    public void Update(GameTime gameTime, KeyboardState keyboardState, 
                       List<MetalSurface> metalSurfaces,
                       List<HiddenPlatform> hiddenPlatforms)
    {
        PreviousBounds = Bounds;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Обновление способностей
        MagnetAbility.Update(gameTime);
        ScanAbility.Update(gameTime);
        
        // Активация способностей по клавишам
        if (keyboardState.IsKeyDown(Keys.LeftShift))
            MagnetAbility.Activate();
        
        if (keyboardState.IsKeyDown(Keys.E) && _previousKeyboardState.IsKeyUp(Keys.E))
            ScanAbility.Activate();
        
        // Сканирование скрытых платформ
        if (ScanAbility.IsActive)
        {
            foreach (var hidden in hiddenPlatforms)
            {
                float distance = Vector2.Distance(Position, 
                    new Vector2(hidden.Bounds.X + hidden.Bounds.Width / 2,
                                hidden.Bounds.Y + hidden.Bounds.Height / 2));
                
                if (distance <= Constants.SCAN_RADIUS)
                    hidden.IsRevealed = true;
            }
        }
        else
        {
            foreach (var hidden in hiddenPlatforms)
                hidden.IsRevealed = false;
        }
        
        // Движение
        float moveDirection = 0;
        if (keyboardState.IsKeyDown(Keys.A))
            moveDirection = -1;
        if (keyboardState.IsKeyDown(Keys.D))
            moveDirection = 1;
        
        float currentSpeed = Constants.PLAYER_SPEED;
        if (MagnetAbility.IsActive && IsOnMetal)
            currentSpeed *= 0.7f;
        
        Velocity = new Vector2(moveDirection * currentSpeed, Velocity.Y);
        
        // Направление взгляда
        if (moveDirection > 0) _facingRight = true;
        if (moveDirection < 0) _facingRight = false;
        
        // Прыжок
        if (keyboardState.IsKeyDown(Keys.W) && _previousKeyboardState.IsKeyUp(Keys.W) && 
            (IsGrounded || (MagnetAbility.IsActive && IsOnMetal)))
        {
            Velocity = new Vector2(Velocity.X, Constants.PLAYER_JUMP_FORCE);
            IsGrounded = false;
            IsOnMetal = false;
            _animationState = AnimationState.Jumping;
            _currentAnimationIndex = 0;
        }
        
        // Гравитация
        if (!(IsOnMetal && MagnetAbility.IsActive))
            Velocity = new Vector2(Velocity.X, Velocity.Y + Constants.GRAVITY * deltaTime);
        
        Position += Velocity * deltaTime;
        
        // Проверка на приземление
        _justLanded = !IsGrounded && _wasGrounded;
        if (_justLanded)
        {
            _animationState = AnimationState.Landing;
            _currentAnimationIndex = 0;
        }
        
        // === АНИМАЦИЯ ===
        _animationTimer += deltaTime;
        
        if (!_justLanded)
        {
            if (IsGrounded)
            {
                if (MathHelper.Distance(Velocity.X, 0) > 10)
                {
                    if (_animationState != AnimationState.Walking)
                    {
                        _animationState = AnimationState.Walking;
                        _currentAnimationIndex = 0;
                    }
                }
                else
                {
                    if (_animationState != AnimationState.Idle)
                    {
                        _animationState = AnimationState.Idle;
                        _currentAnimationIndex = 0;
                    }
                }
            }
            else
            {
                if (_animationState != AnimationState.Jumping)
                {
                    _animationState = AnimationState.Jumping;
                    _currentAnimationIndex = 0;
                }
            }
        }
        
        if (_animationTimer >= _animationSpeed)
        {
            _animationTimer = 0;
            int[] currentFrames = GetCurrentFrames();
            _currentAnimationIndex++;
            
            if (_currentAnimationIndex >= currentFrames.Length)
            {
                if (_animationState == AnimationState.Landing)
                {
                    _animationState = AnimationState.Idle;
                    _currentAnimationIndex = 0;
                }
                else
                {
                    _currentAnimationIndex = 0;
                }
            }
            
            _currentFrame = currentFrames[_currentAnimationIndex];
        }
        
        // Прилипание к металлу
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
        
        _wasGrounded = IsGrounded;
        _previousKeyboardState = keyboardState;
    }
    
    // === ПОЛУЧЕНИЕ МАССИВА КАДРОВ ДЛЯ ТЕКУЩЕЙ АНИМАЦИИ ===
    private int[] GetCurrentFrames()
    {
        return _animationState switch
        {
            AnimationState.Idle => _idleFrames,
            AnimationState.Walking => _walkFrames,
            AnimationState.Jumping => _jumpFrames,
            AnimationState.Landing => _landFrames,
            _ => _idleFrames,
        };
    }
    
    // === ОТРИСОВКА ===
    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effect = _facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        Color drawColor = MagnetAbility.IsActive ? magnetColor : normalColor;
        _spriteSheet.Draw(spriteBatch, _currentFrame, Position, drawColor, effect);
    }
    
    // === ВОЗРОЖДЕНИЕ ===
    public void Reset(Vector2 respawnPosition)
    {
        Position = respawnPosition;
        Velocity = Vector2.Zero;
        IsGrounded = false;
        IsOnMetal = false;
        MagnetAbility.Deactivate();
        ScanAbility.Deactivate();
        _animationState = AnimationState.Idle;
        _currentFrame = _idleFrames[0];
        _currentAnimationIndex = 0;
        _wasGrounded = false;
        _justLanded = false;
    }
}
