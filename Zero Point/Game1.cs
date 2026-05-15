using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.Core;
using ZeroPoint.Entities;
using ZeroPoint.Managers;
using ZeroPoint.States;
using ZeroPoint.Utils;

namespace ZeroPoint;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixelTexture;

    private Player _player;
    private Camera _camera;
    private LevelManager _levelManager;

    private GameState _currentState;
    private MenuState _menuState;
    private PauseMenuState _pauseMenuState;

    private SpriteFont _font;
    private KeyboardState _previousKeyboardState;

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
        _levelManager = new LevelManager();
        _currentState = GameState.Menu;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });

        _font = Content.Load<SpriteFont>("Fonts/PixelFont");

        _menuState = new MenuState(GraphicsDevice, _font);
        _pauseMenuState = new PauseMenuState(GraphicsDevice, _font);

        _player = new Player(_levelManager.CurrentLevel.PlayerStartPosition);
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            if (_currentState == GameState.Playing)
                _currentState = GameState.Pause;
            else if (_currentState == GameState.Pause)
                _currentState = GameState.Playing;
            else if (_currentState == GameState.Menu)
                Exit();
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
                UpdatePlaying(gameTime, keyboardState);
                break;

            case GameState.Pause:
                _pauseMenuState.Update(out bool resumeClicked, out bool menuClicked, out bool exitClicked2);
                if (resumeClicked)
                    _currentState = GameState.Playing;
                if (menuClicked)
                {
                    _currentState = GameState.Menu;
                    _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
                }
                if (exitClicked2)
                    Exit();
                break;
        }

        _previousKeyboardState = keyboardState;
        base.Update(gameTime);
    }

    private void UpdatePlaying(GameTime gameTime, KeyboardState keyboardState)
    {
        _player.Update(gameTime, keyboardState,
            _levelManager.CurrentLevel.MetalSurfaces,
            _levelManager.CurrentLevel.HiddenPlatforms);

        CollisionManager.HandleCollisions(_player,
            _levelManager.CurrentLevel.Platforms,
            _levelManager.CurrentLevel.MetalSurfaces,
            _levelManager.CurrentLevel.HiddenPlatforms);

        if (CollisionManager.CheckSpikeCollision(_player, _levelManager.CurrentLevel.Spikes))
            _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);

        if (CollisionManager.CheckCollision(_player.Bounds, _levelManager.CurrentLevel.ExitDoor))
        {
            _levelManager.NextLevel();
            _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
        }

        _camera.Follow(_player);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_currentState == GameState.Menu)
        {
            _spriteBatch.Begin();
            _menuState.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        else if (_currentState == GameState.Playing)
        {
            DrawGameplay();
        }
        else if (_currentState == GameState.Pause)
        {
            DrawGameplay();

            _spriteBatch.Begin();
            _pauseMenuState.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }

    private void DrawGameplay()
    {
        GraphicsDevice.Clear(new Color(50, 50, 60));
        _spriteBatch.Begin(transformMatrix: _camera.Transform);
        _levelManager.CurrentLevel.Draw(_spriteBatch, _pixelTexture);
        _player.Draw(_spriteBatch, _pixelTexture);
        _spriteBatch.End();
    }
}
