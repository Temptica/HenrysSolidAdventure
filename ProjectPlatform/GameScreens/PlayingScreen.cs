﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OtterlyAdventure.Controller;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Mapfolder;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.GameScreens
{
    internal class PlayingScreen : IGameScreen
    {
        BackGround _backGround;
        List<Button> _buttons;
        List<Text> _texts;
        SpriteFont _font;
        Coin _displayCoin;
        HealthBar _healthBar;
        public PlayingScreen(Screen screen, ContentManager content, SpriteFont font)
        {
            MapLoader.SetMapId(1);
            MapLoader.LoadMap(screen.Height, content);
            //_gameState = GameState.Playing;
            Otter.Instance.Position = Map.Instance.Spawn;
            _backGround = BackGround.Instance;
            _backGround.Reset();
            _buttons = new List<Button>();
            _font = font;
            Otter.Instance.Reset();
            _displayCoin = new Coin(new Vector2(20,20));
            Otter.Instance.SetWalk(true);
            AudioController.Instance.PlaySong("GamePlay");
            HealthBar.Texture ??= content.Load<Texture2D>("Items/HealthBarEmpty");
            HealthBar.BarTexture ??= content.Load<Texture2D>("Items/HealthBar");
            _healthBar = new HealthBar(new Vector2(20, 50),2f);
            _texts = new List<Text>
            {
                new(new Vector2(55, 25), $": {Otter.Instance.Coins}", Color.White, 0.2f, 0f, font)
            };
        }

        public void Draw(SpriteBatch spriteBatch, Sprites sprites)
        {
            _backGround.Draw(sprites);
            
            Map.Instance.Draw(sprites);
            Otter.Instance.Draw(sprites);
            _displayCoin.Draw(sprites);
            _texts.ForEach(text => text.Draw(spriteBatch));
            _healthBar.Draw(sprites);
        }

        public void Update(GameTime gameTime)
        {
            _healthBar.SetHealth(Otter.Instance.HealthPercentage);
            _displayCoin.Update(gameTime);
            _texts = new List<Text>
            {
                new(new Vector2(55, 25), $"= {Otter.Instance.Coins}", Color.White, 0.2f, 0f, _font)
            };
            Map.Instance.Update(gameTime);
            Otter.Instance.Update(gameTime);
            if (InputController.ExitInput) Game1.SetState(GameState.Paused);
            if (InputController.DeadInput) Game1.SetState(GameState.GameOver);
        }
    }
}
