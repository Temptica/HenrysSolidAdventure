using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProjectPlatform
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        List<IMovable> Movables;

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
            ;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
            Movables.Add(new Otter(Content.Load<Texture2D>("Otterly Idle"), new Vector2(100, 100)));
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
            if (keyState.IsKeyDown(Keys.D))
                _ = "";//Right
            if (keyState.IsKeyDown(Keys.Q))
                _ = "";//Left
            if (keyState.IsKeyDown(Keys.Z) )
                _ = ""; //jump
            if (keyState.IsKeyDown(Keys.S))
                _ = ""; //sleep
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
            // TODO: Add your drawing code here
            foreach (IMovable movable in Movables)
            {
                movable.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}