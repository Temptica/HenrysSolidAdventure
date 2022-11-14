using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using ProjectPlatform.Mapfolder;

namespace ProjectPlatform
{
    enum GameState { Menu, Paused, Playing }
    public class Game1 : Game
    {
        
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BackGround backGround;
        private SpriteFont Font;
        private GameState gameState;
        private Otter otter;
        private List<Button> Buttons;
        private Texture2D hitbox;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Buttons = new List<Button>();
            // _graphics.IsFullScreen = true;
            gameState = GameState.Menu;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            backGround = BackGround.Instance;
            backGround.Initialise(Content);
            Font = Content.Load<SpriteFont>("Fonts/ThaleahFat");
            //Movables.Add(otter);//Main character will always be index 0;
            var startTexture = Content.Load<Texture2D>("Buttons/StartButton");
            Buttons.Add(new Button("StartButton", startTexture, new Vector2((_graphics.PreferredBackBufferWidth - startTexture.Width)/2f, (_graphics.PreferredBackBufferHeight- startTexture.Height)/2f), GameState.Menu));
            var map = Map.GetInstance();
            map.Initialise(Content, _graphics.PreferredBackBufferWidth);
            otter = new Otter(Content.Load<Texture2D>("Character/Otterly Idle"), new Vector2(100, 100), 0.001f, map.Scale/5f);//dyncamic
            hitbox = new Texture2D(GraphicsDevice, 1, 1);
            hitbox.SetData(new[] { Color.White });
            var x = 0f;
            var rng = new Random();
            var testMap = new List<MapTile>();
            while (x < _graphics.PreferredBackBufferWidth)
            {
                testMap.Add(new MapTile(map.GetTile(rng.Next(0, map.TileSet.Count)), new Vector2(x, _graphics.PreferredBackBufferHeight - 100)));
                x += testMap[0].Tile.Rectangle.Width*map.Scale;
            }
            map.FrontMap = testMap;
            
        }

        protected override void Update(GameTime gameTime)
        {
            #region Controlls
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.D)||keyState.IsKeyDown(Keys.Right))
            {
                otter.MoveRight();
            }
            else if (keyState.IsKeyDown(Keys.Q) || keyState.IsKeyDown(Keys.Left))
            {
                otter.MoveLeft();
            }
            if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.Up))
            {
                otter.Jump();
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (gameState == GameState.Menu)
                {
                    if (Buttons.First(button => button.Name == "StartButton")
                        .CheckHit(Mouse.GetState().Position.ToVector2()))
                    {
                        BeginGame();
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.P))
            {
                switch (gameState)
                {
                    case GameState.Menu:
                        BeginGame();
                        break;
                    case GameState.Playing:
                        gameState = GameState.Paused;
                        break;
                    case GameState.Paused:
                        gameState = GameState.Playing;
                        break;
                    default:
                        gameState = GameState.Playing;
                        otter.Position = new Vector2(100, 100);
                        break;
                }
            }
            if (keyState.IsKeyDown(Keys.LeftAlt) && keyState.IsKeyDown(Keys.Enter))
            {
                _graphics.ToggleFullScreen();
                _graphics.ApplyChanges();
            }
            #endregion
            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameState.Menu:
                    backGround.Update(gameTime);
                    otter.CurrentAnimation.Update(gameTime);
                    break;
                case GameState.Paused:
                    break;
                case GameState.Playing:
                    otter.Update(gameTime);
                    break;
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            backGround.Draw(_spriteBatch, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            switch (gameState)
            {
                case GameState.Menu:
                    
                    string title = "Otterly Adventure";
                    var length = Font.MeasureString(title).Length();
                    var halfWidth = _graphics.PreferredBackBufferWidth/2f;
                    var halfHeight = _graphics.PreferredBackBufferHeight/2f;
                    Vector2 textPosition = new(halfWidth-length/2, 200);

                    _spriteBatch.DrawString(Font, title,
                        textPosition + new Vector2(5,5)//center text
                        , Color.Black);//background
                    
                    _spriteBatch.DrawString(Font, title,
                        textPosition //center text
                        , Color.SandyBrown);//foreground

                    otter.Position = new Vector2(50, halfHeight);
                    otter.Draw(_spriteBatch); //draw otter                   

                    break;
                case GameState.Paused:
                    _spriteBatch.DrawString(Font, "Press Enter to resume", new Vector2(100, 100), Color.White);
                    break;
                case GameState.Playing:

                    //var tile = Map.GetInstance().GetTile(8);

                    //var mapTile = new MapTile(tile, new Vector2(0, _graphics.PreferredBackBufferHeight-100));
                    //mapTile.Draw(_spriteBatch);
                    Map.GetInstance().FrontMap.ForEach(tile => tile.Draw(_spriteBatch));
                    //foreach (var tile in Map.GetInstance().FrontMap)
                    //{
                    //    _spriteBatch.Draw(hitbox, tile.HitBox, Color.Green);
                    //}
                    _spriteBatch.Draw(hitbox, otter.HitBox, Color.Red);
                    otter.Draw(_spriteBatch);
                    //_spriteBatch.Draw(hitbox, )
                    break;
            }
            
            Buttons.Where(button => button.IsActive).ToList().ForEach(button => button.Draw(_spriteBatch));// draw active buttons

            // TODO: Add your drawing code here
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void SetMenu()
        {
            gameState = GameState.Playing;
            otter.Position = new Vector2(100, 100);
            otter.SetCanWalk(false);
            backGround.Reset();
            Buttons.ForEach(button => button.UpdateActive(gameState));
        }

        public void BeginGame()
        {
            gameState = GameState.Playing;
            otter.Position = new Vector2(100, 100);
            backGround.Reset();
            Buttons.ForEach(button => button.UpdateActive(gameState));
            otter.SetCanWalk(true);
        }
    }
}