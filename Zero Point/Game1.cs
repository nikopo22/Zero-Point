using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.Core;
using ZeroPoint.Entities;
using ZeroPoint.Managers;
using ZeroPoint.States;
using ZeroPoint.UI;
using ZeroPoint.Utils;
using ZeroPoint.Controllers;
using System.Collections.Generic;

namespace ZeroPoint;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixelTexture;
    private SpriteSheet _playerSpriteSheet;
    private SpriteSheet _portalSpriteSheet;
    private Texture2D _menuTexture;
    private Texture2D _blockTexture;
    private Texture2D _spikeTexture;
    private List<(Texture2D texture, float speed)> _backgroundLayers;

    private Player _player;
    private PlayerController _playerController;
    private Camera _camera;
    private LevelManager _levelManager;

    private int _portalFrame;
    private double _portalFrameTimer;
    private const double PortalFrameDuration = 0.16;

    private GameState _currentState;
    private MenuState _menuState;
    private PauseMenuState _pauseMenuState;
    private VictoryState _victoryState;
    private int _lastCompletedLevelIndex;

    private SpriteFont _font;
    private KeyboardState _previousKeyboardState;
    private MouseState _previousMouseState;
    private Button _helpButton;
    private bool _isHelpVisible;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _camera = new Camera();
        _levelManager = new LevelManager(Content);
        _currentState = GameState.Menu;
        _previousKeyboardState = Keyboard.GetState();
        _previousMouseState = Mouse.GetState();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });

        // Правильный путь к текстуре
        Texture2D playerTexture = Content.Load<Texture2D>("Sprites/OrangeRobot_SpriteSheet");
        System.Diagnostics.Debug.WriteLine($"Размер текстуры: {playerTexture.Width} x {playerTexture.Height}");

        int frameWidth = 32;   
        int frameHeight = 32;  

        _playerSpriteSheet = new SpriteSheet(playerTexture, frameWidth, frameHeight);

        _menuTexture = Content.Load<Texture2D>("Menu/menu");
        _blockTexture = Content.Load<Texture2D>("Block/block");
        _spikeTexture = Content.Load<Texture2D>("Spike/spike");
        _portalSpriteSheet = new SpriteSheet(Content.Load<Texture2D>("End/portal"), 230, 545);

        _font = Content.Load<SpriteFont>("Fonts/PixelFont");
        _menuState = new MenuState(GraphicsDevice, _font, _menuTexture);
        _pauseMenuState = new PauseMenuState(GraphicsDevice, _font);
        _victoryState = new VictoryState(GraphicsDevice, _font);
        _helpButton = new Button(new Rectangle(20, 20, 48, 48), "?", _font);
        _backgroundLayers = new List<(Texture2D, float)>
        {
            (Content.Load<Texture2D>("Backgrounds/1"), 0.0f),
            (Content.Load<Texture2D>("Backgrounds/2"), 0.05f),
            (Content.Load<Texture2D>("Backgrounds/3"), 0.2f),
            (Content.Load<Texture2D>("Backgrounds/4"), 0.45f),
            (Content.Load<Texture2D>("Backgrounds/5"), 0.75f),
        };
        _player = new Player(
            _levelManager.CurrentLevel.PlayerStartPosition
        );
        _playerController = new PlayerController();
        
        // Observer Pattern: Subscribe to events
        _player.PlayerDied += OnPlayerDied;
        _levelManager.LevelCompleted += OnLevelCompleted;
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var mouseState = Mouse.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            if (_isHelpVisible)
            {
                _isHelpVisible = false;
            }
            else if (_currentState == GameState.Playing)
            {
                _currentState = GameState.Pause;
            }
            else if (_currentState == GameState.Pause)
            {
                _currentState = GameState.Playing;
            }
            else if (_currentState == GameState.Menu)
            {
                Exit();
            }
        }

        if (_currentState != GameState.Menu && _currentState != GameState.Victory)
        {
            _helpButton.Update(mouseState);
            if (_helpButton.WasReleased(mouseState, _previousMouseState))
            {
                _isHelpVisible = !_isHelpVisible;
            }
        }

        switch (_currentState)
        {
            case GameState.Menu:
                _menuState.Update(out bool playClicked, out bool exitClicked);
                if (playClicked)
                {
                    _currentState = GameState.Playing;
                    _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
                }
                if (exitClicked)
                    Exit();
                break;

            case GameState.Playing:
                if (!_isHelpVisible)
                    UpdatePlaying(gameTime, keyboardState);
                break;

            case GameState.Victory:
                _victoryState.Update(_lastCompletedLevelIndex, out bool continueClicked, out bool retryClicked, out bool victoryMenuClicked);
                if (continueClicked)
                {
                    _levelManager.NextLevel();
                    _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
                    _currentState = GameState.Playing;
                }
                if (retryClicked)
                {
                    if (_lastCompletedLevelIndex == 3)
                    {
                        _levelManager.StartFirstLevel();
                    }
                    else
                    {
                        _levelManager.ReloadLevel();
                    }
                    _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
                    _currentState = GameState.Playing;
                }
                if (victoryMenuClicked)
                {
                    _currentState = GameState.Menu;
                    _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
                }
                break;

            case GameState.Pause:
                _pauseMenuState.Update(out bool resumeClicked, out bool pauseMenuClicked, out bool exitClicked2);
                if (resumeClicked)
                    _currentState = GameState.Playing;
                if (pauseMenuClicked)
                {
                    _currentState = GameState.Menu;
                    _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
                }
                if (exitClicked2)
                    Exit();
                break;
        }

        if (_currentState != GameState.Menu && _currentState != GameState.Victory)
            UpdatePortalAnimation(gameTime);

        _previousKeyboardState = keyboardState;
        _previousMouseState = mouseState;
        base.Update(gameTime);
    }

    private void UpdatePortalAnimation(GameTime gameTime)
    {
        _portalFrameTimer += gameTime.ElapsedGameTime.TotalSeconds;
        while (_portalFrameTimer >= PortalFrameDuration)
        {
            _portalFrame = (_portalFrame + 1) % 4;
            _portalFrameTimer -= PortalFrameDuration;
        }
    }

    private void UpdatePlaying(GameTime gameTime, KeyboardState keyboardState)
    {
        // Handle input via controller
        _playerController.Update(_player, keyboardState);
        _player.Update(gameTime,
            _levelManager.CurrentLevel.MetalSurfaces,
            _levelManager.CurrentLevel.HiddenPlatforms);

        CollisionManager.HandleCollisions(_player,
            _levelManager.CurrentLevel.Platforms,
            _levelManager.CurrentLevel.MetalSurfaces,
            _levelManager.CurrentLevel.HiddenPlatforms,
            _levelManager.CurrentLevel.InvisibleWalls);

        // Observer Pattern: Raise TakeDamage instead of directly resetting player
        if (CollisionManager.CheckSpikeCollision(_player, _levelManager.CurrentLevel.Spikes))
            _player.TakeDamage();

        // Observer Pattern: Raise LevelCompleted event
        if (CollisionManager.CheckCollision(_player.Bounds, _levelManager.CurrentLevel.ExitDoor))
        {
            _lastCompletedLevelIndex = _levelManager.CurrentLevelIndex;
            _levelManager.OnLevelCompleted();
            _isHelpVisible = false;
        }

        _camera.Follow(_player, _levelManager.CurrentLevel.InvisibleWalls);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_currentState == GameState.Menu ? new Color(25, 25, 40) : new Color(50, 50, 60));

        if (_currentState == GameState.Menu)
        {
            _spriteBatch.Begin();
            _menuState.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        else
        {
            DrawGameplay();

            _spriteBatch.Begin();
            _helpButton.Draw(_spriteBatch, _pixelTexture);
            if (_isHelpVisible)
                DrawHelpOverlay();
            _spriteBatch.End();

            if (_currentState == GameState.Pause)
            {
                _spriteBatch.Begin();
                _pauseMenuState.Draw(_spriteBatch);
                _spriteBatch.End();
            }
            else if (_currentState == GameState.Victory)
            {
                _spriteBatch.Begin();
                _victoryState.Draw(_spriteBatch, _lastCompletedLevelIndex);
                _spriteBatch.End();
            }
        }

        base.Draw(gameTime);
    }

    private void DrawHelpOverlay()
    {
        var overlayWidth = 560;
        var overlayHeight = 380;
        var overlayRect = new Rectangle((Constants.SCREEN_WIDTH - overlayWidth) / 2, (Constants.SCREEN_HEIGHT - overlayHeight) / 2 + 20, overlayWidth, overlayHeight);

        _spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT), new Color(0, 0, 0, 150));
        _spriteBatch.Draw(_pixelTexture, overlayRect, new Color(30, 30, 50, 230));
        _spriteBatch.Draw(_pixelTexture, new Rectangle(overlayRect.X, overlayRect.Y, overlayRect.Width, 3), Color.White);
        _spriteBatch.Draw(_pixelTexture, new Rectangle(overlayRect.X, overlayRect.Y + overlayRect.Height - 3, overlayRect.Width, 3), Color.White);
        _spriteBatch.Draw(_pixelTexture, new Rectangle(overlayRect.X, overlayRect.Y, 3, overlayRect.Height), Color.White);
        _spriteBatch.Draw(_pixelTexture, new Rectangle(overlayRect.X + overlayRect.Width - 3, overlayRect.Y, 3, overlayRect.Height), Color.White);

        string title = "Управление";
        Vector2 titleSize = _font.MeasureString(title);
        _spriteBatch.DrawString(_font, title, new Vector2(overlayRect.X + (overlayRect.Width - titleSize.X) / 2, overlayRect.Y + 20), Color.LightGoldenrodYellow);

        var lines = new[]
        {
            "A/D - влево/вправо",
            "W - прыжок",
            "E - сканирование",
            "SHIFT - прилипание",
            "ESC - пауза",
            "ALT+ENTER - на полный экран"
        };

        float lineY = overlayRect.Y + 90;
        foreach (var line in lines)
        {
            _spriteBatch.DrawString(_font, line, new Vector2(overlayRect.X + 40, lineY), Color.White);
            lineY += 48;
        }
    }


    private void DrawParallaxBackground()
    {
        _spriteBatch.Begin();

        foreach (var layer in _backgroundLayers)
        {
            float x = -_camera.CameraPosition.X * layer.speed;

            int drawWidth = Constants.SCREEN_WIDTH;
            int drawHeight = Constants.SCREEN_HEIGHT;

            x %= drawWidth;

            _spriteBatch.Draw(
                layer.texture,
                new Rectangle((int)x, 0, drawWidth, drawHeight),
                Color.White);

            _spriteBatch.Draw(
                layer.texture,
                new Rectangle((int)x + drawWidth, 0, drawWidth, drawHeight),
                Color.White);
        }

        _spriteBatch.End();
    }

    private void DrawGameplay()
    {
        GraphicsDevice.Clear(new Color(20, 20, 30));

        DrawParallaxBackground();

        _spriteBatch.Begin(transformMatrix: _camera.Transform);

        _levelManager.CurrentLevel.Draw(_spriteBatch, _pixelTexture, _blockTexture, _spikeTexture, _portalSpriteSheet, _portalFrame);
        // draw player via renderer
        ZeroPoint.Renderers.PlayerRenderer.Draw(_spriteBatch, _player, _playerSpriteSheet);

        _spriteBatch.End();
    }
    
    // Observer Pattern: Event handlers
    
    /// <summary>Handles PlayerDied event: reset player to respawn position.</summary>
    private void OnPlayerDied()
    {
        _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
    }
    
    /// <summary>Handles LevelCompleted event: transition to victory state.</summary>
    private void OnLevelCompleted()
    {
        _currentState = GameState.Victory;
    }
}
