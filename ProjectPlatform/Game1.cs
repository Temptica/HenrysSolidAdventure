using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectPlatform.Mapfolder;
using ProjectPlatform.Shop;
using System.Threading;
using ProjectPlatform.EnemyFolder;
using ProjectPlatform.Graphics;
using ProjectPlatform.Controller;
using ProjectPlatform.OtterFolder;
using ProjectPlatform.GameScreens;

namespace ProjectPlatform
{
    enum GameState { Menu, Paused, Playing, GameOver }
    public class Game1 : Game
    {       
        
        private readonly GraphicsDeviceManager _graphics;
        private BackGround _backGround;
        private SpriteFont _font;
        private static GameState _gameState;
        private static bool _stateChanged;
        private OtterFolder.Otter _otter;
        private List<Button> _buttons;
        private Texture2D _hitbox;
        private SpriteBatch _sprite;
        private Screen _screen;
        private Sprites _sprites;
        private Camera _camera;
        private IGameScreen currentScreen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _buttons = new List<Button>();
            // _graphics.IsFullScreen = true;
            _gameState = GameState.Menu;
            _sprite = new SpriteBatch(_graphics.GraphicsDevice);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            _screen = new Screen(this, 1200, 675);
            _camera = new Camera(_screen);
            _sprites = new Sprites(this);            

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _backGround = BackGround.Instance;
            _backGround.Initialise(Content);
            _font = Content.Load<SpriteFont>("Fonts/ThaleahFat");
            var startTexture = Content.Load<Texture2D>("buttons/StartButton");
            //_buttons.Add(new Button("StartButton", startTexture, new Vector2((_screen.Width - startTexture.Width)/2f, (_screen.Height- startTexture.Height)/2f),"START"));
            Bat.Texture = Content.Load<Texture2D>("Enemies/Bat");
            var map = Map.Instance;
            map.Initialise(Content, _screen);
            _otter = Otter.Instance;
            _otter.Initialise(Content.Load<Texture2D>("Character/rsz_otterly_idle"), new Vector2(100, 100), 0.0005f, 1f);
            Button.Font = _font;
            _hitbox = new Texture2D(GraphicsDevice, 1, 1);
            _hitbox.SetData(new[] { Color.White });
            AudioController.Initialise(Content);

            _gameState = GameState.Menu;
            _stateChanged = true;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            
            #region Controlls

            InputController.Update();
            
            if (InputController.ExitInput) Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _graphics.ToggleFullScreen();
                _graphics.ApplyChanges();
            }
#if DEBUG
            //if (_gameState is GameState.Playing)
            //{
            //    if (InputController.NextInput)
            //    {
            //        MapLoader.LoadNextMap(_screen.Height);
            //    }
            //    else if (InputController.PreviousInput)
            //    {
            //        MapLoader.LoadPreviousMap(_screen.Height);
            //    }
            //}
#endif
#endregion

            //switch (_gameState)
            //{
            //    case GameState.Menu:
            //        _backGround.Update(gameTime);
            //        _otter.CurrentAnimation.Update(gameTime);
            //        break;
            //    case GameState.Paused:
            //        break;
            //    case GameState.Playing:
            //        Map.Instance.Update(gameTime);
            //        _otter.Update(gameTime);
            //        break;
            //}
            currentScreen?.Update(gameTime);
            if (_stateChanged)
            {
                switch (_gameState)
                {
                    case GameState.Playing when currentScreen is PausedScreen screen:
                        currentScreen = screen._playingScreen;
                        return;
                    case GameState.Playing:
                        currentScreen = new PlayingScreen(_screen, Content, _font);
                        break;
                    case GameState.Menu:
                        currentScreen = new StartScreen(_screen, Content, _font);
                        break;
                    case GameState.Paused:
                        currentScreen = new PausedScreen(_screen, _font, Content, (PlayingScreen)currentScreen);
                        break;
                    case GameState.GameOver:
                        currentScreen = new GameOverScreen(_screen, Content, _font);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_gameState), _gameState, null);
                }
                _stateChanged = false;
                currentScreen?.Update(gameTime);
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _screen.Set();
            GraphicsDevice.Clear(Color.CornflowerBlue);
                
            _sprites.Begin(_camera,true);
            _sprite.Begin();
            
            currentScreen.Draw(_sprite, _sprites);

            _sprites.End();
            _sprite.End();
            _screen.UnSet();
            _screen.Present(_sprites, false);
            base.Draw(gameTime);
        }
        
        internal static void SetState(GameState state)
        {
            _gameState = state;
            _stateChanged = true;
        }
    }
}