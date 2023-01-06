using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Enemies
{
    internal abstract class Enemy : Character
    {
        protected Enemy()
        {
            CanAttack = true;
        }
        public bool Remove { get; set; }
        public Animation CurrentAnimation => Animations.CurrentAnimation;
        internal State State { get; set; }

        public override void Update(GameTime gameTime)
        {
            if (!Hero.Instance.IsInvisible && State == State.Attacking && CurrentAnimation.IsFinished || State is not State.Attacking)
            {
                CanAttack = true;
            }
            Animations.Update(State, gameTime);

        }
        public abstract bool CheckDamage();

        public virtual bool GetDamage(int i)
        {
            Health -= i;
            StatsController.Instance.AddDamageDealt(i);
            IsHit = true;
            if (!(Health <= 0)) return false;
            IsDead = true;
            return true;
        }

        public override void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            Animations.Draw(sprites, Position, IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
            //If you read this, you're awesome!
        }

        public virtual int Attack()
        {
            if (CanDamage && CheckDamage() && State is State.Attacking)
            {
                CanDamage = false; //attacked once so can't damage anymore till next attack;
                return Damage;
            }
            return 0;
        }

    }
}
