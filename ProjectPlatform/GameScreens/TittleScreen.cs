using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HenrySolidAdventure.GameScreens
{
    internal class TittleScreen : IGameScreen
    {
        private readonly BackGround _backGround;
        private readonly SpriteFont _font;
        private readonly List<Button> _buttons;
        private readonly Screen _screen;
        private readonly Text _title;
        private readonly Text _titleBackGround;
        private readonly Vector2 _textPosition;
        private const float moveSpeed = 0.2f;
        private bool _loaded;
        public TittleScreen(Screen screen, ContentManager content)
        {
            _loaded = true;
            _screen = screen;
            _backGround = BackGround.Instance;
            _font = Game1.MainFont; 
            var startTexture = content.Load<Texture2D>("Buttons/EmptyButton");
            var setting = content.Load<Texture2D>("Buttons/Cog");

            _buttons = new List<Button>
            {
                new("StartButton", startTexture,
                    new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f), "START"),
                new ("Setting", setting, new Vector2(_screen.Width - setting.Width - 25, 25))
            };

            var halfWidth = _screen.Width / 2f;
            string title = "Henry's Solid Adventure";
            float scale = 0.75f;
            var length = _font.MeasureString(title).Length()*scale;
            _textPosition = new(halfWidth - length / 2, _screen.Height / 10f);
            _title = new Text(new Vector2(-length,_textPosition.Y), title, Color.SandyBrown, scale, 0f, _font);
            _titleBackGround = new Text(new Vector2(-length, _textPosition.Y) + new Vector2(5, 5), title, Color.Black, scale, 0f, _font);
            Hero.Instance.Reset();
            Hero.Instance.Position = _buttons.First(b=>b.Name=="StartButton").Position - new Vector2(0,Hero.Instance.HitBox.Height);
            Hero.Instance.SetWalk(true);
            
            _backGround.Reset();
            AudioController.Instance.PlaySong(Songs.MainMenu);
        }
        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            _backGround.Draw(sprites, new Vector2(_screen.Width, _screen.Height));
            _buttons.ForEach(button => button.Draw(sprites, spriteBatch));
            _titleBackGround.Draw(spriteBatch);
            _title.Draw(spriteBatch);
            Hero.Instance.Draw(sprites, spriteBatch);
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
            var startButton = _buttons.First(b => b.Name == "StartButton");
            Hero.Instance.MenuUpdate(gameTime, startButton.Position.X, startButton.Position.X + startButton.Texture.Width,startButton.Position.Y);
            var selected = ClickableChecker.CheckHits(_buttons);
            if(_loaded && (MouseController.IsLeftClicked||InputController.ExitInput))//avoids going back to the game when pressing menu button or esc too long on game-over/paused screen
            {
                return;
            }
            _loaded = false;
            if (InputController.ExitInput)
            {
                Game1.ExitGame();
            }
            if (selected is null)
            {
                Mouse.SetCursor(MouseCursor.Arrow);
                return;
            }
            var button = selected as Button;
            Mouse.SetCursor(MouseCursor.Hand);
            if (MouseController.IsLeftClicked)
            {
                if (button.Name == "StartButton")
                {
                    
                    Game1.SetState(GameState.Playing);
                }

                if (button.Name == "Setting")
                {
                    _loaded = true;
                    Game1.SetState(GameState.Settings);
                }
            }
            

        }
    }
}
