using System.Collections.Generic;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.MapFolder;
using HenrySolidAdventure.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.GameScreens
{
    internal class PlayingScreen : IGameScreen
    {
        private readonly BackGround _backGround;
        private List<Text> _texts;
        private readonly SpriteFont _font;
        private readonly Coin _displayCoin;
        private readonly HealthBar _healthBar;
        private bool _loaded;
        private Screen _screen;
        public PlayingScreen(Screen screen, ContentManager content)
        {
            MapLoader.SetMapId(1);
            MapLoader.LoadMap(screen.Height, content);
            _screen = screen;
            //_gameState = GameState.Playing;
            Hero.Instance.Position = Map.Instance.Spawn;
            _backGround = BackGround.Instance;
            _backGround.Reset();
            _font = Game1.MainFont;
            Hero.Instance.Reset();
            _displayCoin = new Coin(new Vector2(20,20));
            AudioController.Instance.PlaySong(Songs.GamePlay);
            HealthBar.Texture ??= content.Load<Texture2D>("Items/HealthBarEmpty");
            HealthBar.BarTexture ??= content.Load<Texture2D>("Items/HealthBar");
            _healthBar = new HealthBar(new Vector2(20, 50),2f);
            _texts = new List<Text>
            {
                new(new Vector2(55, 25), $": {Hero.Instance.Coins}", Color.White, 0.2f, 0f, _font),
                //timer for speedrunning
                
            };
            StatsController.Reset();
            Hero.Instance.Inventory.PotionAdd(PotionLoader.MakeHealingPotion());
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            _backGround.Draw(sprites);
            
            Map.Instance.Draw(sprites, spriteBatch);
            Hero.Instance.Draw(sprites, spriteBatch);
            _displayCoin.Draw(sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
            _healthBar.Draw(sprites);
        }

        public void Update(GameTime gameTime)
        {
            _healthBar.SetHealth(Hero.Instance.HealthPercentage);
            _displayCoin.Update(gameTime);
            StatsController.UpdatePlayTime(gameTime);
            _texts = new List<Text>
            {
                new(new Vector2(55, 25), $"= {Hero.Instance.Coins}", Color.White, 0.2f, 0f, _font)
            };
            Map.Instance.Update(gameTime);
            Hero.Instance.Update(gameTime);
            if (_loaded && InputController.ExitInput) return;
            _loaded = false;
            if (InputController.ExitInput)
            {
                _loaded = true;
                Game1.SetState(GameState.Paused);
            }
        }
    }
}
