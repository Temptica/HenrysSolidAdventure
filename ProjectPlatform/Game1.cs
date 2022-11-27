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
using ProjectPlatform.Audio;
using ProjectPlatform.Graphics;

namespace ProjectPlatform
{
    enum GameState { Menu, Paused, Playing }
    public class Game1 : Game
    {       
        
        private readonly GraphicsDeviceManager _graphics;
        private BackGround _backGround;
        private SpriteFont _font;
        private GameState _gameState;
        private Otter _otter;
        private List<Button> _buttons;
        private Texture2D _hitbox;
        private SpriteBatch _sprite;
        private Screen _screen;
        private Sprites _sprites;
        private Camera _camera;

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
            //_graphics.PreferredBackBufferWidth = 1200;
            //_graphics.PreferredBackBufferHeight = 675;
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
            Coin.Texture = Content.Load<Texture2D>("Items/Coin");
            Store.Texture = Content.Load<Texture2D>("Decoration/shop_anim");
            var startTexture = Content.Load<Texture2D>("buttons/StartButton");
            _buttons.Add(new Button("StartButton", startTexture, new Vector2((_screen.Width - startTexture.Width)/2f, (_screen.Height- startTexture.Height)/2f), GameState.Menu));
            
            var map = Map.Instance;
            map.Initialise(Content);
            //_otter = new Otter(Content.Load<Texture2D>("Character/Otterly Idle"), new Vector2(100, 100), 0.0005f, 0.20f);
            _otter = new Otter(Content.Load<Texture2D>("Character/rsz_otterly_idle"), new Vector2(100, 100), 0.0005f, 1f);

            _hitbox = new Texture2D(GraphicsDevice, 1, 1);
            _hitbox.SetData(new[] { Color.White });
            AudioController.Initialise(Content);

            SetMenu();


        }

        protected override void Update(GameTime gameTime)
        {
            #region Controlls

            InputController.Update();
            
            if (InputController.ExitInput) Exit();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (_gameState == GameState.Menu)
                {
                    if (_buttons.First(button => button.Name == "StartButton")
                        .CheckHit(Mouse.GetState().Position.ToVector2()))
                    {
                        BeginGame();
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                if(_gameState is GameState.Playing) _otter.Position = Map.Instance.Spawn;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _graphics.ToggleFullScreen();
                _graphics.ApplyChanges();
            }
            #endregion
            
            switch (_gameState)
            {
                case GameState.Menu:
                    _backGround.Update(gameTime);
                    _otter.CurrentAnimation.Update(gameTime);
                    break;
                case GameState.Paused:
                    break;
                case GameState.Playing:
                    Map.Instance.Update(gameTime);
                    _otter.Update(gameTime);
                    break;
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _screen.Set();
            GraphicsDevice.Clear(Color.CornflowerBlue);
                
            _sprites.Begin(_camera,true);
            _sprite.Begin();
            _backGround.Draw(_sprites, new Vector2(_screen.Width, _screen.Height));
            switch (_gameState)
            {
                case GameState.Menu:
                    string title = "Otterly Adventure";
                    var length = _font.MeasureString(title).Length();
                    var halfWidth = _screen.Width/2f;
                    var halfHeight = _screen.Height/2f;
                    Vector2 textPosition = new(halfWidth-length/2, _screen.Height/10f);

                    _sprite.DrawString(_font, title,
                        textPosition + new Vector2(5,5)//center text
                        , Color.Black);//background
                    
                    _sprite.DrawString(_font, title,
                        textPosition //center text
                        , Color.SandyBrown);//foreground
                    
                    _otter.Position = new Vector2(50, halfHeight);
                    _otter.Draw(_sprites); //draw otter                   

                    break;
                case GameState.Paused:
                    _sprite.DrawString(_font, "Press Enter to resume", new Vector2(100, 100), Color.White);
                    break;
                case GameState.Playing:

                    //Map.Instance.Coins.ForEach(coin => _spriteBatch.Draw(_hitbox, coin.HitBox, Color.Yellow));
                    Map.Instance.Draw(_sprites);
                    //_sprites.Draw(_hitbox, _otter.HitBox, Color.Red);
                    _otter.Draw(_sprites);
                    _sprite.DrawString(_font, $"Coins: {_otter.Coins}", new Vector2(100, 100), Color.White);
                    break;
            }
            _buttons.Where(button => button.IsActive).ToList().ForEach(button => button.Draw(_sprites));// draw active buttons
            
            
            _sprites.End();
            _sprite.End();
            _screen.UnSet();
            _screen.Present(_sprites, false);
            base.Draw(gameTime);
        }
        public void SetMenu()
        {
            _gameState = GameState.Menu;
            var halfHeight = _screen.Height / 2f;
            _otter.Position = new Vector2(50, halfHeight);
            _otter.SetWalk(false);
            _backGround.Reset();
            _buttons.ForEach(button => button.UpdateActive(_gameState));
            AudioController.Instance.PlaySong("MainMenu");
        }

        public void BeginGame()
        {
            MapLoader.LoadMap(@$"{Directory.GetCurrentDirectory()}..\..\..\..\..\..\Map\Level2.json", _screen.Height);
            _gameState = GameState.Playing;
            _otter.Position = Map.Instance.Spawn;
            _backGround.Reset();
            _buttons.ForEach(button => button.UpdateActive(_gameState));
            _otter.SetWalk(true);
            AudioController.Instance.PlaySong("GamePlay");
        }
    }
}