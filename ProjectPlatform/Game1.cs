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
        private List<Button> buttons;
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
            buttons = new List<Button>();
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
            Coin.Texture = Content.Load<Texture2D>("Items/Coin");
            Store.Texture = Content.Load<Texture2D>("Decoration/shop_anim");
            var startTexture = Content.Load<Texture2D>("buttons/StartButton");
            buttons.Add(new Button("StartButton", startTexture, new Vector2((_graphics.PreferredBackBufferWidth - startTexture.Width)/2f, (_graphics.PreferredBackBufferHeight- startTexture.Height)/2f), GameState.Menu));
            var map = Map.Instance;
            map.Initialise(Content, _graphics.PreferredBackBufferWidth);
            otter = new Otter(Content.Load<Texture2D>("Character/Otterly Idle"), new Vector2(100, 100), 0.0005f, map.Scale/5f);//dyncamic
            hitbox = new Texture2D(GraphicsDevice, 1, 1);
            hitbox.SetData(new[] { Color.White });
            AudioController.Initialise(Content);

            SetMenu();


        }

        protected override void Update(GameTime gameTime)
        {
            #region Controlls

            InputController.Update();
            
            if (InputController.ExitInput) Exit();
            if (InputController.RightInput)
            {
                otter.MoveRight(gameTime, InputController.ShiftInput);
            }
            else if (InputController.LeftInput)
            {
                otter.MoveLeft(gameTime, InputController.ShiftInput);
            }
            if (InputController.JumpInput)
            {
                otter.Jump();
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (gameState == GameState.Menu)
                {
                    if (buttons.First(button => button.Name == "StartButton")
                        .CheckHit(Mouse.GetState().Position.ToVector2()))
                    {
                        BeginGame();
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R) && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                otter.Position = new Vector2(100, 100);
            }
            //if (keyState.IsKeyDown(Keys.P))
            //{
            //    switch (gameState)
            //    {
            //        case GameState.Menu:
            //            BeginGame();
            //            break;
            //        case GameState.Playing:
            //            gameState = GameState.Paused;
            //            break;
            //        case GameState.Paused:
            //            gameState = GameState.Playing;
            //            break;
            //        default:
            //            gameState = GameState.Playing;
            //            otter.Position = new Vector2(100, 100);
            //            break;
            //    }
            //}
            //if (keyState.IsKeyDown(Keys.LeftAlt) && keyState.IsKeyDown(Keys.Enter))
            //{
            //    _graphics.ToggleFullScreen();
            //    _graphics.ApplyChanges();
            //}
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
                    Map.Instance.Update(gameTime);
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

                    Map.Instance.Coins.ForEach(coin => _spriteBatch.Draw(hitbox, coin.HitBox, Color.Yellow));
                    Map.Instance.Draw(_spriteBatch);
                    _spriteBatch.Draw(hitbox, otter.HitBox, Color.Red);
                    otter.Draw(_spriteBatch);
                        
                    _spriteBatch.DrawString(Font, $"Coins: {otter.Coins}", new Vector2(100, 100), Color.White);
                    break;
            }
            
            buttons.Where(button => button.IsActive).ToList().ForEach(button => button.Draw(_spriteBatch));// draw active buttons

            // TODO: Add your drawing code here
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void SetMenu()
        {
            gameState = GameState.Menu;
            var halfHeight = _graphics.PreferredBackBufferHeight / 2f;
            otter.Position = new Vector2(50, halfHeight);
            otter.SetWalk(false);
            backGround.Reset();
            buttons.ForEach(button => button.UpdateActive(gameState));
            AudioController.Instance.PlaySong("MainMenu");
        }

        public void BeginGame()
        {
            MapLoader.LoadMap(@$"{Directory.GetCurrentDirectory()}..\..\..\..\..\..\Map\Level1V3.json", GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            gameState = GameState.Playing;
            otter.Position = new Vector2(100, 100);
            backGround.Reset();
            buttons.ForEach(button => button.UpdateActive(gameState));
            otter.SetWalk(true);
            AudioController.Instance.PlaySong("GamePlay");
        }
    }
}