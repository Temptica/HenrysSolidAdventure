using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using ProjectPlatform.Controller;

namespace ProjectPlatform.GameScreens
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
                new("Menu", startTexture, new Vector2(halfWidth, halfHeight), "Menu")
            };
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            BackGround.Instance.Draw(sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
            _buttons.ForEach(b => b.Draw(sprites, spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            if (InputController.InteractInput) Game1.SetState(GameState.Menu);
            _buttons.ForEach(b => b.CheckHit(MouseController.GetScreenPosition(_screen)));
        }
    }
}
