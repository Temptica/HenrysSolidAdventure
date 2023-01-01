using System;
using System.Collections.Generic;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.GameScreens
{
    internal class WinScreen: IGameScreen
    {
        private List<Text> _texts;
        private List<Button> _buttons;
        Screen _screen;

        public WinScreen(Screen screen, SpriteFont font, ContentManager content)
        {
            _screen = screen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "You won!";
            var length = font.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2, screen.Height / 10f);

            _texts = new List<Text> {
                new(textPosition,title , Color.Red, 1f, 0f, font)
            };
            var startTexture = content.Load<Texture2D>("buttons/EmptyButton");
            _buttons = new List<Button>
            {
                new("Menu", startTexture, new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f), "Menu"),
                new("Play again", startTexture, new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f+startTexture.Height + 20f), "Restart")

            };
            AudioController.Instance.PlaySong("MainMenu");
        }
        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            throw new NotImplementedException();
        }
    }
}
