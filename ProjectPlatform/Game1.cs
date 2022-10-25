using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProjectPlatform
{
    public class Game1 : Game
    {
        enum GameState { Menu, Paused, Playing}
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        List<IMoveable> Movables;
        Texture2D[] Background;
        SpriteFont Font;
        GameState gameState;


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
            Background = new Texture2D[3];
            Background[0] = Content.Load<Texture2D>("Background/background_layer_1");
            Background[1] = Content.Load<Texture2D>("Background/background_layer_2");
            Background[2] = Content.Load<Texture2D>("Background/background_layer_3");
            Font = Content.Load<SpriteFont>("Fonts/ThaleahFat");
            Movables.Add(new Otter(Content.Load<Texture2D>("Character/Otterly Idle"), new Vector2(100, 100), 0.001f));//Main character will always be index 0;
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
                Movables[0].MoveRight();
            }
            else if (keyState.IsKeyDown(Keys.Q) || keyState.IsKeyDown(Keys.Left))
            {
                Movables[0].MoveLeft();
            }
            if (keyState.IsKeyDown(Keys.Space) || keyState.IsKeyDown(Keys.Up))
            {
                Movables[0].Jump();
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
               //attack
            }

            if (keyState.IsKeyDown(Keys.P))
            {
                switch (gameState)
                {
                    case GameState.Menu:
                        gameState = GameState.Playing;
                        break;
                    case GameState.Playing:
                        gameState = GameState.Menu;
                        break;
                    case GameState.Paused:
                        gameState = GameState.Playing;
                        break;
                    default:
                        gameState = GameState.Playing;
                        break;

                }
            }

            #endregion
            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameState.Menu:
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
            foreach (Texture2D background in Background)
            {
                Vector2 scale = new(_graphics.PreferredBackBufferHeight / background.Height,
                    _graphics.PreferredBackBufferWidth / background.Width);//checks how many times background fits in screen. Code below will multiply this scale number to make it fit
                _spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            switch (gameState)
            {
                case GameState.Menu:
                    _spriteBatch.DrawString(Font, "Press Enter to start", new Vector2(100, 100), Color.White);
                    break;
                case GameState.Paused:
                    _spriteBatch.DrawString(Font, "Press Enter to resume", new Vector2(100, 100), Color.White);
                    break;
                case GameState.Playing:
                    foreach (var movable in Movables)
                    {
                        movable.Draw(_spriteBatch);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // TODO: Add your drawing code here
            foreach (var movable in Movables)
            {
                movable.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}