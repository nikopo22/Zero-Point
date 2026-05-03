using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZeroPoint.Core;
using ZeroPoint.Entities;
using ZeroPoint.Managers;
using ZeroPoint.Utils;

namespace ZeroPoint;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixelTexture;

    // компоненты 
    private Player _player;
    private Camera _camera;
    private LevelManager _levelManager;

    // состояния
    private bool _gameOver;
    private KeyboardState _previousKeyboardState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // размер окна
        _graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _camera = new Camera();
        _levelManager = new LevelManager();

        // создаём игрока на старте
        _player = new Player(_levelManager.CurrentLevel.PlayerStartPosition);

        _gameOver = false;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // текстура 1x1 пиксель для рисования прямоугольников
        _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // выход по Escape
        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (!_gameOver)
        {
            // обновляем игрока
            _player.Update(gameTime, keyboardState);

            // обрабатываем коллизии
            CollisionManager.HandleCollisions(_player, _levelManager.CurrentLevel.Platforms);

            // проверяем столкновение с шипами
            if (CollisionManager.CheckSpikeCollision(_player, _levelManager.CurrentLevel.Spikes))
            {
                // возрождение
                _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
            }

            // достижение выхода
            if (CollisionManager.CheckCollision(_player.Bounds, _levelManager.CurrentLevel.ExitDoor))
            {
                _levelManager.CurrentLevel.LevelCompleted = true;
                _levelManager.NextLevel();
                _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
            }

            // обновляем камеру
            _camera.Follow(_player);
        }

        _previousKeyboardState = keyboardState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(50, 50, 60));

        _spriteBatch.Begin(transformMatrix: _camera.Transform);

        // рисуем уровень
        _levelManager.CurrentLevel.Draw(_spriteBatch, _pixelTexture);

        // рисуем игрока
        _player.Draw(_spriteBatch, _pixelTexture);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

}
