using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OtterlyAdventure.Shop;
using System.Threading;
using OtterlyAdventure.Controller;
using OtterlyAdventure.EnemyFolder;
using OtterlyAdventure.GameScreens;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Mapfolder;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure
{
    enum GameState { Menu, Paused, Playing, GameOver, Settings }
    public class Game1 : Game
    {       
        
        private readonly GraphicsDeviceManager _graphics;
        private BackGround _backGround;
        private SpriteFont _font;
        private static GameState _gameState;
        private static bool _stateChanged;
        private List<Button> _buttons;
        private Texture2D _hitbox;
        private SpriteBatch _sprite;
        private Screen _screen;
        private Sprites _sprites;
        private Camera _camera;
        private static bool _isFullScreen;
        private IGameScreen currentScreen;
        private static bool _exit = false;

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
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            _screen = new Screen(this, 1200, 675);
            _camera = new Camera(_screen);
            _sprites = new Sprites(this);
            base.Initialize();

        }

        internal static void SetFullScreen(bool isClicked)
        {
            _isFullScreen = isClicked;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        }

        protected override void LoadContent()
        {
            _backGround = BackGround.Instance;
            _backGround.Initialise(Content);
            _font = Content.Load<SpriteFont>("Fonts/ThaleahFat");
            StateButton.ClickedTexture = Content.Load<Texture2D>("Buttons/knobSelected");
            StateButton.UnClickedTexture = Content.Load<Texture2D>("Buttons/KnobNotSelected");
            Slider.SliderTexture = Content.Load<Texture2D>("Buttons/slider");
            Slider.SliderKnobTexture = Content.Load<Texture2D>("Buttons/sliderKnob");

            Bat.Texture = Content.Load<Texture2D>("Enemies/Bat");
            var map = Map.Instance;
            map.Initialise(Content, _screen);
            Otter.Instance.Initialise(Content.Load<Texture2D>("Character/OtterAni"), new Vector2(100, 100), 0.0005f, 1f);
            Button.Font = _font;
            _hitbox = new Texture2D(GraphicsDevice, 1, 1);
            _hitbox.SetData(new[] { Color.White });
            AudioController.Initialise(Content);

            _gameState = GameState.Menu;
            _stateChanged = true;

            Settings.Instance.Initialise(Directory.GetCurrentDirectory() + "/settings.txt");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_exit) Exit();
            if (_isFullScreen && !_graphics.IsFullScreen)
            {
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();
            }
            else if (!_isFullScreen && _graphics.IsFullScreen)
            {
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
            }
            InputController.Update();

            
            currentScreen?.Update(gameTime);
            if (_stateChanged)
            {
                if (currentScreen is SettingsScreen settingsScreen)
                {
                    currentScreen = settingsScreen.LastScreen;
                    _gameState = currentScreen switch
                    {
                        PlayingScreen => GameState.Playing,
                        StartScreen => GameState.Menu,
                        GameOverScreen => GameState.GameOver,
                        PausedScreen => GameState.Paused,
                        _ => _gameState
                    };
                }
                else
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
                        case GameState.Settings:
                            currentScreen = new SettingsScreen(currentScreen, _screen, _font);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(_gameState), _gameState, null);
                    }
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
        internal static void ExitGame()
        {
            _exit = true;
        }
    }
}