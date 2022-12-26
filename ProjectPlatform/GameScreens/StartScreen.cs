using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using OtterlyAdventure.Controller;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.GameScreens
{
    internal class StartScreen : IGameScreen
    {
        BackGround _backGround;
        SpriteFont _font;
        List<Button> _buttons;
        Screen _screen;
        Text _title;
        Text _titleBackGround;
        Vector2 _textPosition;
        float moveSpeed = 0.2f;
        bool _loaded;
        public StartScreen(Screen screen, ContentManager content, SpriteFont font)
        {
            _loaded = true;
            _screen = screen;
            _backGround = BackGround.Instance;
            _font = font; 
            var startTexture = content.Load<Texture2D>("buttons/EmptyButton");
            _buttons = new List<Button>
            {
                new("StartButton", startTexture,
                    new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f), "START")
            };

            var halfWidth = _screen.Width / 2f;
            string title = "Otterly Adventure";
            var length = _font.MeasureString(title).Length();
            _textPosition = new(halfWidth - length / 2, _screen.Height / 10f);
            _title = new Text(new Vector2(-length,_textPosition.Y), title, Color.SandyBrown, 1f, 0f, font);
            _titleBackGround = new Text(new Vector2(-length, _textPosition.Y) + new Vector2(5, 5), title, Color.Black, 1f, 0f, font);
            Otter.Instance.Reset();
            Otter.Instance.Position = _buttons.First(b=>b.Name=="StartButton").Position + new Vector2(0,Otter.Instance.HitBox.Height);
            Otter.Instance.SetWalk(true);
            
            _backGround.Reset();
            _buttons.ForEach(button => button.UpdateActive(true));
            AudioController.Instance.PlaySong("MainMenu");
        }
        public void Draw(SpriteBatch sprite, Sprites sprites)
        {
            _backGround.Draw(sprites, new Vector2(_screen.Width, _screen.Height));
            _buttons.ForEach(button => button.Draw(sprites, sprite));
            _titleBackGround.Draw(sprite);
            _title.Draw(sprite);
            Otter.Instance.Draw(sprites); //draw otter
            //_start.Draw(sprite);
        }

        public void Update(GameTime gameTime)
        {
            
            //move title to _textPosition
            if (_title.Position.X < _textPosition.X)
            {
                var newX = (float)(moveSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);
                _title.Position += new Vector2(newX, 0);
                _titleBackGround.Position += new Vector2(newX, 0);
            }
            _backGround.Update(gameTime);
            Otter.Instance.CurrentAnimation.Update(gameTime);
            var startButton = _buttons.First(b => b.Name == "StartButton");
            Otter.Instance.MenuUpdate(gameTime, startButton.Position.X, startButton.Position.X + startButton.Texture.Width,startButton.Position.Y);
            var selected = _buttons.Where(button => button.CheckHit(MouseController.GetScreenPosition(_screen))).ToList();
            if(_loaded && (MouseController.IsLeftClicked||InputController.ExitInput))//avoids going back to the game when pressing menu button or esc too long on game-over/paused screen
            {
                return;
            }
            _loaded = false;
            if (InputController.ExitInput)
            {
                Game1.ExitGame();
            }
            if (selected.Count == 0)
            {
                Mouse.SetCursor(MouseCursor.Arrow);
                return;
            }
            Mouse.SetCursor(MouseCursor.Hand);
            if (MouseController.IsLeftClicked)
            {
                if (selected[0].Name == "StartButton")
                {
                    Game1.SetState(GameState.Playing);
                }
            }
            

        }
    }
}
