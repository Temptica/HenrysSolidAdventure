using System;
using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Graphics.Clickables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.GameScreens
{
    internal class WinScreen: IGameScreen
    {
        private readonly List<Text> _texts;
        private readonly List<Button> _buttons;
        private readonly Screen _screen;

        public WinScreen(Screen screen, ContentManager content)
        {
            _screen = screen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "You won!";
            var length = Game1.MainFont.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2, screen.Height / 10f);

            _texts = new List<Text> {
                new(textPosition,title , Color.Red, 1f, 0f, Game1.MainFont),
                new(new Vector2(50,150),StatsController.GetStats(),Color.White,0.25f,0f,Game1.MainFont)
            };
            var startTexture = content.Load<Texture2D>("buttons/EmptyButton");
            _buttons = new List<Button>
            {
                new("Menu", startTexture, new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f), "Menu"),
                new("Play again", startTexture, new Vector2((_screen.Width - startTexture.Width) / 2f, (_screen.Height - startTexture.Height) / 2f+startTexture.Height + 20f), "Restart")

            };
            AudioController.Instance.PlaySong(Songs.MainMenu);
        }
        public void Update(GameTime gameTime)
        {
            BackGround.Instance.Update(gameTime);
            var selected = ClickableChecker.CheckHits(_buttons);
            if (MouseController.IsLeftClicked && selected is Button button)
            {
                switch (button.Name)
                {
                    case "Menu":
                        Game1.SetState(GameState.Menu);
                        return;
                    case "Play again":
                        Game1.SetState(GameState.Playing);
                        return;
                }
            }
            //change color between orange and yellow. Who doesn't like to party?
            var color = Color.Lerp(Color.Orange, Color.Yellow, (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds/500));
            _texts.First().Color = color;
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            BackGround.Instance.Draw(sprites);
            _texts.ForEach(t => t.Draw(spriteBatch));
            _buttons.ForEach(b => b.Draw(sprites, spriteBatch));
            
        }
    }
}
