using System.Collections.Generic;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Graphics.Clickables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.GameScreens
{
    internal class GameOverScreen : IGameScreen
    {
        private readonly List<Text> _texts;
        private readonly List<Button> _buttons;
        public GameOverScreen(Screen screen, ContentManager content)
        {
            
            var halfWidth = screen.Width / 2f;
            string title = "Game Over";
            var length = Game1.MainFont.MeasureString(title).Length();
            Vector2 textPosition = new(halfWidth - length / 2, screen.Height / 10f);

            _texts = new List<Text> {
                new(textPosition,title , Color.Red, 1f, 0f, Game1.MainFont),
                new(new Vector2(50,150),StatsController.GetStats(),Color.White,0.25f,0f,Game1.MainFont)
            };
            var startTexture = content.Load<Texture2D>("buttons/EmptyButton");
            _buttons = new List<Button>
            {
                new("Menu", startTexture, new Vector2((screen.Width - startTexture.Width) / 2f, (screen.Height - startTexture.Height) / 2f), "Menu"),
                new("Restart", startTexture, new Vector2((screen.Width - startTexture.Width) / 2f, (screen.Height - startTexture.Height) / 2f+startTexture.Height + 20f), "Restart")

            };
            AudioController.Instance.PlaySong(Songs.GameOver);
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
            var selected = ClickableChecker.CheckHits(_buttons);
            if (MouseController.IsLeftClicked && selected is Button button)
            {
                switch (button.Name)
                {
                    case "Menu":
                        Game1.SetState(GameState.Menu);
                        break;
                    case "Restart":
                        Game1.SetState(GameState.Playing);
                        break;
                }
            }
        }
    }
}
