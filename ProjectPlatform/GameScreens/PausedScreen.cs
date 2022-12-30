using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using OtterlyAdventure.Controller;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.GameScreens
{
    internal class PausedScreen : IGameScreen
    {
        List<Text> _texts;
        public PlayingScreen _playingScreen { get; }
        private List<Button> _buttons;
        private Screen _screen;
        bool _loaded;
        public PausedScreen(Screen screen,SpriteFont font, ContentManager content, PlayingScreen playScreen)
        {
            _loaded = true;
            _playingScreen = playScreen;
            _screen = screen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "Press \"E\" or \"enter\" to resume.";
            var length = font.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2 * 0.5f, screen.Height / 2f);
            var setting = content.Load<Texture2D>("Buttons/Cog");
            _buttons = new List<Button>
            {
                new ("Setting", setting, new Vector2(screen.Width - setting.Width - 50, 50))
            };
            _texts = new List<Text> {
            new(textPosition,"Press \"E\" or \"enter\" to resume. \nPress \"Escape\" to go to menu." , Color.White, 0.5f, 0f, font)
            };
        }
        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            _playingScreen.Draw(spriteBatch, sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
            _buttons.ForEach(button => button.Draw(sprites, spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            _texts.ForEach(text => text.Color = Color.Lerp(Color.White, Color.Transparent, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)));
            if (_loaded && InputController.ExitInput) return;
            _loaded = false;

            var selected = _buttons.Where(button => button.CheckHit(_screen)).ToList();
            if (selected.Count > 0)
            {
                Mouse.SetCursor(MouseCursor.Hand);
                if (MouseController.IsLeftClicked)
                {
                    switch (selected[0].Name)
                    {
                        case "Setting":
                            _loaded = true;
                            Game1.SetState(GameState.Settings);
                            return;
                    }
                }
            }
            else Mouse.SetCursor(MouseCursor.Arrow);
            if (InputController.InteractInput)
            {
                _loaded = true;
                Game1.SetState(GameState.Playing);
                return;
            }
            if (InputController.ExitInput)
            {
                _loaded = true;
                Game1.SetState(GameState.Menu);
            }
            
        }
    }
}
