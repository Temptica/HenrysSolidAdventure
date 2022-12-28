using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using OtterlyAdventure.Controller;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.GameScreens
{
    internal class PausedScreen : IGameScreen
    {
        List<Text> _texts;
        public PlayingScreen _playingScreen { get; }
        bool _loaded;
        public PausedScreen(Screen screen,SpriteFont font, ContentManager content, PlayingScreen playScreen)
        {
            _loaded = true;
            _playingScreen = playScreen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "Press \"E\" or \"enter\" to resume.";
            var length = font.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2 * 0.5f, screen.Height / 2f);

            _texts = new List<Text> {
            new(textPosition,"Press \"E\" or \"enter\" to resume. \nPress \"Escape\" to go to menu." , Color.White, 0.5f, 0f, font)
            };
        }
        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            _playingScreen.Draw(spriteBatch, sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            _texts.ForEach(text => text.Color = Color.Lerp(Color.White, Color.Transparent, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)));
            if (_loaded && InputController.ExitInput) return;
            _loaded = false;
            if (InputController.ShiftInput && InputController.ExitInput)
            {
                Game1.SetState(GameState.Settings);
                return;
            }
            if (InputController.InteractInput)
            {
                Game1.SetState(GameState.Playing);
                return;
            }
            if (InputController.ExitInput)
            {
                Game1.SetState(GameState.Menu);
            }
            
        }
    }
}
