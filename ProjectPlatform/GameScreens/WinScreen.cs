using System;
using System.Collections.Generic;
using System.Linq;
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

        public WinScreen(Screen screen, ContentManager content, SpriteFont font)
        {
            _screen = screen;
            var halfWidth = screen.Width / 2f;
            var halfHeight = screen.Height / 2f;
            string title = "You won!";
            var length = font.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2, screen.Height / 10f);

            _texts = new List<Text> {
                new(textPosition,title , Color.Red, 1f, 0f, font),
                new(new Vector2(50,150),StatsController.Instance.GetStats(),Color.White,0.25f,0f,font)
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
            var selected = ClickableChecker.CheckHits(_buttons, _screen);
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
            //change color as a rainbow trough multiple colors 
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
