using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.Characters
{
    internal class Boss : Enemy
    {//https://luizmelo.itch.io/evil-wizard-2
        public static Texture2D Texture { get; set; }
        public Boss(Vector2 position)
        {
            Position = position;
            var width = Texture.Width / 8;
            var height = Texture.Height / 8;
            Animations = new AnimationList<Animation>()
            {
                new(Texture, State.Attacking, 16, width, height, 0, 0, 8),
                new(Texture, State.Dead, 7, width, height, height * 2, 0, 5),
                new(Texture, State.Idle, 8, width, height, height * 4, 0, 8),
                new(Texture, State.Walking, 8, width, height, height * 6, 0, 10),
                new(Texture, State.Hit, 3, width, height, Texture.Height - height, 0, 4)
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //do more logic
        }

        public override void Draw(Sprites spriteBatch)
        {
            base.Draw(spriteBatch);
            //todo
        }

        public override bool CheckDamage()
        {
            return true;
        }

        public virtual void Move(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
