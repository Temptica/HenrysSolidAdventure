using System;
using System.IO;
using HenrySolidAdventure.Characters.Enemies.Tracking;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.GameScreens;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Graphics.Clickables;
using HenrySolidAdventure.MapFolder;
using HenrySolidAdventure.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure
{
    internal enum GameState { Menu, Paused, Playing, GameOver, Settings, Win }
    public class Game1 : Game
    {
        #region Properties

        public static SpriteFont MainFont { get; private set; }
        public static Screen Screen { get; private set; }

        #endregion

        #region Fields

        private readonly GraphicsDeviceManager _graphics;
        private BackGround _backGround;
        private SpriteBatch _sprite;
        private Sprites _sprites;
        private Camera _camera;
        private IGameScreen _currentScreen;

        #endregion

        #region Static fields

        private static bool _exit; 
        private static bool _isFullScreen; 
        private static GameState _gameState;
        private static bool _stateChanged;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _gameState = GameState.Menu;
            _sprite = new SpriteBatch(_graphics.GraphicsDevice);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Screen = new Screen(this, 1200, 675);
            _camera = new Camera(Screen);
            _sprites = new Sprites(this);
            _ = DiscordRichPresence.Instance; //initialise
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
            MainFont = Content.Load<SpriteFont>("Fonts/ThaleahFat");
            StateButton.ClickedTexture = Content.Load<Texture2D>("Buttons/knobSelected");
            StateButton.UnClickedTexture = Content.Load<Texture2D>("Buttons/KnobNotSelected");
            Slider.SliderTexture = Content.Load<Texture2D>("Buttons/slider");
            Slider.SliderKnobTexture = Content.Load<Texture2D>("Buttons/sliderKnob");
            PotionLoader.Initialise(Content);
            Bat.Texture = Content.Load<Texture2D>("Enemies/Bat");
            var map = Map.Instance;
            map.Initialise(Content, Screen);
            Hero.Instance.Initialise(Content.Load<Texture2D>("Character/HeroKnight"), new Vector2(100, 100), 0.0005f, 1f);
            AudioController.Instance.Initialise(Content);

            _gameState = GameState.Menu;
            _stateChanged = true;

            Settings.Instance.Initialise(Directory.GetCurrentDirectory() + "/settings.txt");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_exit) Exit();

            #region Fullscreen

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
            

            #endregion

            _currentScreen?.Update(gameTime);

            #region Game state

            if (_stateChanged)
            {
                if (_currentScreen is SettingsScreen settingsScreen)
                {
                    _currentScreen = settingsScreen.LastScreen;
                    _gameState = _currentScreen switch
                    {
                        PlayingScreen => GameState.Playing,
                        TittleScreen => GameState.Menu,
                        GameOverScreen => GameState.GameOver,
                        PausedScreen => GameState.Paused,
                        _ => _gameState
                    };
                }
                else
                {
                    switch (_gameState)
                    {
                        case GameState.Playing when _currentScreen is PausedScreen screen:
                            _currentScreen = screen.PlayingScreen;
                            break;
                        case GameState.Playing:
                            _currentScreen = new PlayingScreen(Screen, Content);
                            break;
                        case GameState.Menu:
                            _currentScreen = new TittleScreen(Screen, Content);
                            break;
                        case GameState.Paused:
                            _currentScreen = new PausedScreen(Screen, Content, (PlayingScreen)_currentScreen);
                            break;
                        case GameState.GameOver:
                            _currentScreen = new GameOverScreen(Screen, Content);
                            break;
                        case GameState.Settings:
                            _currentScreen = new SettingsScreen(_currentScreen, Screen, Content);
                            break;
                        case GameState.Win:
                            _currentScreen = new WinScreen(Screen, Content);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(_gameState), _gameState, null);
                    }
                }
                
                _stateChanged = false;
                DiscordRichPresence.Instance.UpdateState(_gameState);
                _currentScreen?.Update(gameTime);
            }

            #endregion
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            Screen.Set();
            GraphicsDevice.Clear(Color.CornflowerBlue);
                
            _sprites.Begin(_camera,true);
            _sprite.Begin();
            
            _currentScreen.Draw(_sprite, _sprites);
            
            _sprites.End();
            _sprite.End();
            Screen.UnSet();
            Screen.Present(_sprites, false);
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