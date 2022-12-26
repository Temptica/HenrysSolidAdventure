using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using OtterlyAdventure.Controller;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.GameScreens
{
    internal class GameOverScreen : IGameScreen
    {
        private List<Text> _texts;
        private List<Button> _buttons;
        Screen _screen;
        public GameOverScreen(Screen screen, ContentManager content, SpriteFont font)
        {
            _screen = screen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "Game Over";
            var length = font.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2, screen.Height / 10f);

            _texts = new List<Text> {
                new(textPosition,title , Color.Red, 1f, 0f, font)
            };
            var startTexture = content.Load<Texture2D>("buttons/EmptyButton");
            _buttons = new List<Button>
            {
                new("Menu", startTexture, new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f), "Menu"),
                new("Restart", startTexture, new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f+startTexture.Height + 20f), "Restart")

            };
            AudioController.Instance.PlaySong("GameOver");
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            BackGround.Instance.Draw(sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
            _buttons.ForEach(b => b.Draw(sprites, spriteBatch));
        }

        public void Update(GameTime gameTime)
        {

            BackGround.Instance.Update(gameTime);
            var selected = _buttons.Where(b => b.CheckHit(MouseController.GetScreenPosition(_screen))).ToList();
            if (selected.Count == 0)
            {
                Mouse.SetCursor(MouseCursor.Arrow);
                return;
            }
            Mouse.SetCursor(MouseCursor.Hand);
            if (MouseController.IsLeftClicked)
            {
                if (selected[0].Name == "Menu")
                {
                    Game1.SetState(GameState.Menu);
                }
                if (selected[0].Name == "Restart")
                {
                    Game1.SetState(GameState.Playing);
                }
            }
        }
    }
}
