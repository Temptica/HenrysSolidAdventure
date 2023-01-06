using System;
using System.Collections.Generic;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Graphics.Clickables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HenrySolidAdventure.GameScreens
{
    internal class PausedScreen : IGameScreen
    {
        private readonly List<Text> _texts;
        public PlayingScreen PlayingScreen { get; }
        private readonly List<Button> _buttons;
        private readonly Screen _screen;
        private bool _loaded;
        public PausedScreen(Screen screen,ContentManager content, PlayingScreen playScreen)
        {
            _loaded = true;
            PlayingScreen = playScreen;
            _screen = screen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "Press \"Escape\" to resume.";
            var length = Game1.MainFont.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2 * 0.5f, 150);
            var setting = content.Load<Texture2D>("Buttons/Cog");
            var menuButton = content.Load<Texture2D>("Buttons/EmptyButton");
            _buttons = new List<Button>
            {
                new ("Setting", setting, new Vector2(screen.Width - setting.Width - 25, 25)),
                new ("GiveUp", menuButton, new Vector2(halfWidth - menuButton.Width / 2f, halfHeight - menuButton.Height / 2f + 100), "Give up",0.75f)
            };
            _texts = new List<Text> {
            new(textPosition,title , Color.White, 0.5f, 0f, Game1.MainFont)
            };
        }
        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            PlayingScreen.Draw(spriteBatch, sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
            _buttons.ForEach(button => button.Draw(sprites, spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            //makes teh text fade in and out for the nostalgic feeling
            _texts.ForEach(text => text.Color = Color.Lerp(Color.White, Color.Transparent, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)));
            if (_loaded && InputController.ExitInput) return;
            _loaded = false;

            var selected = ClickableChecker.CheckHits(_buttons);
            if (MouseController.IsLeftClicked && selected is Button button)
            {
                switch (button.Name) 
                {
                    case "Setting":
                        _loaded = true;
                        Game1.SetState(GameState.Settings);
                        return;
                    case "GiveUp":
                        _loaded = true;
                        Game1.SetState(GameState.GameOver);
                        return;
                }
            }
            else Mouse.SetCursor(MouseCursor.Arrow);
            if (InputController.ExitInput)
            {
                _loaded = true;
                Game1.SetState(GameState.Playing);
            }
            
        }
    }
}
