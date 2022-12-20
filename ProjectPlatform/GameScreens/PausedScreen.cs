using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Graphics;
using ProjectPlatform.Mapfolder;
using ProjectPlatform.OtterFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPlatform.Controller;

namespace ProjectPlatform.GameScreens
{
    internal class PausedScreen : IGameScreen
    {
        List<Text> _texts;
        public PlayingScreen _playingScreen { get; }
        public PausedScreen(Screen screen,SpriteFont font, ContentManager content, PlayingScreen playScreen)
        {
            _playingScreen = playScreen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "press enter to resume";
            var length = font.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2, screen.Height / 2f);

            _texts = new List<Text> {
            new(textPosition,"Press \"E\" or \"enter\" to resume" , Color.White, 1f, 0f, font)
            };
        }
        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            _playingScreen.Draw(spriteBatch, sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            if (InputController.InteractInput)
            {
                Game1.SetState(GameState.Playing);
            }
        }
    }
}
