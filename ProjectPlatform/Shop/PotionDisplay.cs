using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Shop
{
    internal class PotionDisplay
    {
        private List<Potion> _potions;
        private List<Text> _descriptions;
        private readonly SpriteFont _font;
        private readonly Store _store;
        private bool _clicked;
        private readonly Text _title;

        public PotionDisplay(IEnumerable<Potion> potions, Store store = null)
        {
            _font = Game1.MainFont;
            _store = store;
            _potions = potions.ToList();
            SetPotionPosition();
            var titleText = store == null ? "Inventory" : "Store";
            var halfWidth = Game1.Screen.Width / 2f;
            var x = halfWidth - _font.MeasureString(titleText).X/2;
            _title = new Text(new Vector2(x, 30), titleText, Color.White, 1f, 0f, _font);
        }

        public void Update()
        {
           
            var potion = ClickableChecker.CheckHits(_potions);
            if (!MouseController.IsLeftClicked)
            {
                _clicked = false; //avoids infinite clicking
                return;
            }
            if (_clicked) return;
            _clicked = true;
            var hero = Hero.Instance;
            if (potion is Potion potion1 )
            {
                
                if (_store != null) //then buy
                {
                    //buy potion
                    if (potion1.Cost <= hero.Coins)
                    {
                        hero.Inventory.PotionAdd(potion1);
                        hero.Coins -= potion1.Cost;
                        StatsController.Instance.RemoveCoins(potion1.Cost);
                        RemovePotion(potion1);
                        _store.RemovePotion(potion1);
                    }
                    return;
                }//else use
                potion1.Use();
                RemovePotion(potion1);
                hero.Inventory.RemovePotion(potion1);
                
            }
        }
        private void SetPotionPosition()
        {
            
            _potions = _potions.OrderBy(po => po.Type).ToList();
            const int width = PotionLoader.Width;
            var x = 50;
            var y = 200;
            
            for (int i = 0; i < _potions.Count(); i++)
            {
                //4 on a row
                if (i % 6 == 0 && i != 0)
                {
                    x = 75;
                    y += width + 75;
                }
                _potions.ElementAt(i).Position = new Vector2(x, y); 
                x += 200;
            }

            _descriptions = new List<Text>();
            foreach (var potion in _potions)
            {
                var descriptionText = "";
                if(_store is not null) descriptionText = $"Cost: {potion.Cost}";
                descriptionText += $"\n{potion.Description}";
                var text = new Text(new Vector2(potion.Position.X + 2, potion.Position.Y + width + 12), descriptionText, Color.Black, 0.15f, 0f, _font);
                _descriptions.Add(text);
                text = new Text(new Vector2(potion.Position.X, potion.Position.Y + width + 10) , descriptionText,Color.White,0.15f,0f, _font);
                _descriptions.Add(text);
                
            }
        }
        public void AddPotion(Potion potion)
        {
            _potions.ToList().Add(potion);
            SetPotionPosition();
        }
        public void RemovePotion(Potion potion)
        {
            _potions.Remove(potion);
            SetPotionPosition();

        }
        public void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            foreach (var potion in _potions)
            {
                potion.Draw(sprites);
            }
            foreach (var text in _descriptions)
            {
                text.Draw(spriteBatch);
            }

            _title.Draw(spriteBatch);

        }
    }
}
