using System;
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
    // Observer Pattern Events
    /// <summary>Raised when player's health changes. Parameter is new health value.</summary>
    public event Action<int>? HealthChanged;
    
    /// <summary>Raised when player dies (health reaches 0).</summary>
    public event Action? PlayerDied;

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsOnMetal { get; set; }
    
    public MagnetAbility MagnetAbility { get; private set; }
    public ScanAbility ScanAbility { get; private set; }
    
    private bool _facingRight = true;
    public bool FacingRight { get => _facingRight; private set => _facingRight = value; }
    public float DrawScale { get; private set; } = 2.0f;
    
    private int _health = 1;
    public int Health { get => _health; private set => _health = value; }
    
    private int[] _idleFrames = { 0, 1, 2, 3, 4 };
    private int[] _walkFrames = { 5, 6, 7, 8, 9, 10 };
    private int[] _jumpFrames = { 11, 12, 13, 14, 15 };
    private int[] _landFrames = { 16, 17, 18 };
    
    private int _currentFrame;
    public int CurrentFrame { get => _currentFrame; private set => _currentFrame = value; }
    private int _currentAnimationIndex;
    private double _animationTimer;
    private double _animationSpeed = 0.08;
    
    private enum AnimationState { Idle, Walking, Jumping, Landing }
    private AnimationState _animationState;
    private bool _wasGrounded;
    private bool _justLanded;
    
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Constants.PLAYER_WIDTH, Constants.PLAYER_HEIGHT);
    public Rectangle PreviousBounds { get; private set; }
    
    private Color normalColor;
    private Color magnetColor;
    private KeyboardState _previousKeyboardState;
    
    public Player(Vector2 startPosition)
    {
        Position = startPosition;
        Velocity = Vector2.Zero;
        IsGrounded = false;
        IsOnMetal = false;
        // SpriteSheet is removed from model; rendering is handled by PlayerRenderer
        
        normalColor = Color.White;
        magnetColor = new Color(100, 150, 255);
        
        MagnetAbility = new MagnetAbility();
        ScanAbility = new ScanAbility();
        
        _animationState = AnimationState.Idle;
        _currentFrame = _idleFrames[0];
        CurrentFrame = _currentFrame;
        _currentAnimationIndex = 0;
        _wasGrounded = false;
        _justLanded = false;
    }
    
    public void Update(GameTime gameTime, KeyboardState keyboardState, 
                       List<MetalSurface> metalSurfaces,
                       List<HiddenPlatform> hiddenPlatforms)
    {
        PreviousBounds = Bounds;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        MagnetAbility.Update(gameTime);
        ScanAbility.Update(gameTime);
        
        if (keyboardState.IsKeyDown(Keys.LeftShift))
            MagnetAbility.Activate();
        
        if (keyboardState.IsKeyDown(Keys.E) && _previousKeyboardState.IsKeyUp(Keys.E))
            ScanAbility.Activate();
        
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
        
        float moveDirection = 0;
        if (keyboardState.IsKeyDown(Keys.A))
            moveDirection = -1;
        if (keyboardState.IsKeyDown(Keys.D))
            moveDirection = 1;
        
        float currentSpeed = Constants.PLAYER_SPEED;
        if (MagnetAbility.IsActive && IsOnMetal)
            currentSpeed *= 0.7f;
        
        Velocity = new Vector2(moveDirection * currentSpeed, Velocity.Y);
        
        if (moveDirection > 0) _facingRight = true;
        if (moveDirection < 0) _facingRight = false;

        if (keyboardState.IsKeyDown(Keys.W) && _previousKeyboardState.IsKeyUp(Keys.W) && 
            (IsGrounded || (MagnetAbility.IsActive && IsOnMetal)))
        {
            Velocity = new Vector2(Velocity.X, Constants.PLAYER_JUMP_FORCE);
            IsGrounded = false;
            IsOnMetal = false;
            _animationState = AnimationState.Jumping;
            _currentAnimationIndex = 0;
        }
        
        if (!(IsOnMetal && MagnetAbility.IsActive))
            Velocity = new Vector2(Velocity.X, Velocity.Y + Constants.GRAVITY * deltaTime);
        
        Position += Velocity * deltaTime;

        _justLanded = !IsGrounded && _wasGrounded;
        if (_justLanded)
        {
            _animationState = AnimationState.Landing;
            _currentAnimationIndex = 0;
        }
 
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
            CurrentFrame = _currentFrame;
        }

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
    
    // Rendering removed from model; use PlayerRenderer to draw.
    
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
        
        // Restore health when respawning
        Health = 1;
        HealthChanged?.Invoke(Health);
    }
    
    /// <summary>Observer Pattern: Reduce health by 1 and raise HealthChanged event.</summary>
    public void TakeDamage()
    {
        if (Health > 0)
        {
            Health--;
            HealthChanged?.Invoke(Health);
            
            if (Health <= 0)
            {
                Die();
            }
        }
    }
    
    /// <summary>Observer Pattern: Raise PlayerDied event.</summary>
    private void Die()
    {
        PlayerDied?.Invoke();
    }
}
