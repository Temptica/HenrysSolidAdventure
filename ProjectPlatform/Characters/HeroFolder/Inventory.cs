using System.Collections.Generic;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Shop;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.HeroFolder
{
    internal class Inventory
    {
        public PotionDisplay PotionDisplay { get; private set; }
        public List<Potion> Potions { get; private set; }
        private bool _display;
        private bool _buttonReleased;

        public void Update()
        {
            if (Potions is null || Potions?.Count == 0)
            {
                PotionDisplay = null;
                return;
            }
            if (InputController.InventoryInput)
            {
                if (!_buttonReleased) return;
                _buttonReleased = false;
                if (_display)
                {
                    PotionDisplay = null;
                    _display = false;
                }
                else
                {
                    PotionDisplay = new PotionDisplay(Potions);
                    _display = true;
                }
            }
            else _buttonReleased = true;
            PotionDisplay?.Update();
        }
        public void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            PotionDisplay?.Draw(sprites, spriteBatch);
        }
        public void PotionAdd(Potion potion)
        {
            Potions ??= new List<Potion>();
            Potions.Add(potion);
        }

        public void RemovePotion(Potion potion)
        {
            Potions.Remove(potion);
        }
    }
}
