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
        List<IMovable> Movables;
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
            Movables = new List<IMovable>();
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
            Movables.Add(new Otter(Content.Load<Texture2D>("Character/Otterly Idle"), new Vector2(100, 100)));//Main character will always be index 0;
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
            int velocityX = 0;
            if (keyState.IsKeyDown(Keys.D))
            {
                velocityX = 5;
            }
            else if (keyState.IsKeyDown(Keys.Q))
            {
                velocityX = -5;
            }
            if (keyState.IsKeyDown(Keys.Space))
            {
                Movables[0].Velocity = new Vector2(velocityX, -10);
            }
            else
            {
                Movables[0].Velocity = new Vector2(velocityX, 0);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
               //attack
            }

            #endregion
            // TODO: Add your update logic here
            foreach(IMovable movable in Movables)
            {
                movable.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            
            foreach (Texture2D background in Background)
            {
                Vector2 Scale = new (_graphics.PreferredBackBufferHeight / background.Height,
                                     _graphics.PreferredBackBufferWidth / background.Width);
                _spriteBatch.Draw(background, Vector2.Zero, null,Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
            // TODO: Add your drawing code here
            _spriteBatch.DrawString(Font, "Hello World", new Vector2(100, 100), Color.White);
            foreach (IMovable movable in Movables)
            {
                movable.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}