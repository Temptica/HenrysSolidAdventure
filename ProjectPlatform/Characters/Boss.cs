using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Characters
{
    internal class Boss : Enemy
    {//https://luizmelo.itch.io/evil-wizard-2
        public static Texture2D Texture { get; set; }
        public Boss(Vector2 position)
        {
            Position = position;
            Animations = new AnimationList<Animation>();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Sprites spriteBatch)
        {
            throw new NotImplementedException();
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
