using System;
using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Shop
{
    internal class Store
    {
        public static Texture2D Texture;
        private readonly BasicAnimation _animation;
        private readonly List<Potion> _potions;
        private readonly Random _rng;
        private bool interacting = false;
        private readonly Rectangle HitBox;
        private PotionDisplay _display;
        private bool _buttonReleased = true;

        public Store(Vector2 position)
        {
            _rng = new Random();
            _animation = new BasicAnimation(Texture, "idle", 5, 6, (int)(Texture.Width / 6f),Texture.Height, 0);
            HitBox = new Rectangle((int)position.X, (int)position.Y, (int)(Texture.Width / 6f), Texture.Height);
            _potions = PotionLoader.MakePotions(_rng.Next(3, 6)).ToList();
        }
        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            if (_potions.Count == 0)
            {
                _display = null;
                return;
            }
            if (Hero.Instance.HitBox.Intersects(HitBox) && InputController.InteractInput)
            {
                if (_buttonReleased)
                {
                    GetMenu();
                }
            }
            if (!InputController.InteractInput) _buttonReleased = true;

            _display?.Update();
        }
        public void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            _animation.Draw(sprites, new Vector2(HitBox.X, HitBox.Y), SpriteEffects.None,1f);
            _display?.Draw(sprites, spriteBatch);
        }
        public void GetMenu()
        {
            if (interacting == false)
            {
                _display = new PotionDisplay(_potions, this);
                interacting = true;
                Hero.Instance.CanWalk = false;
            }
            else
            {
                _display = null;
                interacting = false;
                Hero.Instance.CanWalk = true;
            }

            _buttonReleased = false;
        }

        public void RemovePotion(Potion potion)
        {
            _potions.Remove(potion);

        }
        
    }
}
