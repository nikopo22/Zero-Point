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
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // выход
        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        _player.Update(gameTime, keyboardState,
            _levelManager.CurrentLevel.MetalSurfaces,
            _levelManager.CurrentLevel.HiddenPlatforms);

        //столкновения с платформами
        var allSolidObjects = new List<Platform>();

        //обычные платформы
        allSolidObjects.AddRange(_levelManager.CurrentLevel.Platforms);

        //металлические поверхности как платформы 
        foreach (var metal in _levelManager.CurrentLevel.MetalSurfaces)
        {
            allSolidObjects.Add(new Platform(
                metal.Bounds.X,
                metal.Bounds.Y,
                metal.Bounds.Width,
                metal.Bounds.Height
            ));
        }

        //коллизии
        CollisionManager.HandleCollisions(_player, allSolidObjects);

        //столкновение с шипами
        if (CollisionManager.CheckSpikeCollision(_player, _levelManager.CurrentLevel.Spikes))
        {
            _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
        }

        //достижение выхода
        if (CollisionManager.CheckCollision(_player.Bounds, _levelManager.CurrentLevel.ExitDoor))
        {
            _levelManager.NextLevel();
            _player.Reset(_levelManager.CurrentLevel.PlayerStartPosition);
        }

        _camera.Follow(_player);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(50, 50, 60));

        _spriteBatch.Begin(transformMatrix: _camera.Transform);

        _levelManager.CurrentLevel.Draw(_spriteBatch, _pixelTexture);

        _player.Draw(_spriteBatch, _pixelTexture);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}