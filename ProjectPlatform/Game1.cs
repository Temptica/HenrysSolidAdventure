using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPlatform
{
    enum GameState { Menu, Paused, Playing }
    public class Game1 : Game
    {
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        List<IMoveable> Movables;
        private BackGround backGround;
        SpriteFont Font;
        GameState gameState;
        Otter otter;
        List<Button> Buttons;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Movables = new List<IMoveable>();
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
            var BackgroundTextures = new Texture2D[3];
            BackgroundTextures[0] = Content.Load<Texture2D>("Background/background_layer_1");
            BackgroundTextures[1] = Content.Load<Texture2D>("Background/background_layer_2");
            BackgroundTextures[2] = Content.Load<Texture2D>("Background/background_layer_3");
            backGround = new BackGround(BackgroundTextures);
            Font = Content.Load<SpriteFont>("Fonts/ThaleahFat");
            Movables.Add(new Otter(Content.Load<Texture2D>("Character/Otterly Idle"), new Vector2(100, 100), 0.001f));//Main character will always be index 0;
            otter = (Otter)Movables[0];
            var startTexture = Content.Load<Texture2D>("Buttons/StartButton");
            Buttons.Add(new Button("StartButton", startTexture, new Vector2((_graphics.PreferredBackBufferWidth - startTexture.Width)/2f, (_graphics.PreferredBackBufferHeight- startTexture.Height)/2f), GameState.Menu));

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

            #endregion
            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameState.Menu:
                    backGround.Update(gameTime);
                    otter.Update(gameTime);
                    break;
                case GameState.Paused:
                    break;
                case GameState.Playing:
                    foreach (var movable in Movables)
                    {
                        movable.Update(gameTime);
                    }
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
                    otter.Draw(_spriteBatch, 2f); //draw otter

                    

                    break;
                case GameState.Paused:
                    _spriteBatch.DrawString(Font, "Press Enter to resume", new Vector2(100, 100), Color.White);
                    break;
                case GameState.Playing:
                    foreach (var movable in Movables)
                    {
                        movable.Draw(_spriteBatch,0.5f);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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