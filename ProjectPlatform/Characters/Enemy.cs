using Microsoft.Xna.Framework;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.Characters
{
    internal abstract class Enemy:Character
    {
        protected Enemy()
        {
            CanAttack = true;
        }
        public bool Remove { get; set; }
        public Animation CurrentAnimation=> Animations.CurrentAnimation;
        internal State State { get; set; }

        public override void Update(GameTime gameTime)
        {
            if((State == State.Attacking && CurrentAnimation.IsFinished) || State is not State.Attacking)
            {
                CanAttack = true;
            }
            
        }
        public abstract bool CheckDamage();

        public virtual bool GetDamage(float i)
        {
            CurrentHp -= i;
            IsHit = true;
            if (!(CurrentHp < 0)) return false;
            IsDead = true;
            return true;
        }
        
        public virtual int Attack()
        {
            if (CanDamage && CheckDamage() && State is State.Attacking) {
                CanDamage = false; //attacked once so can't damage anymore till next attack;
                return Damage; 
            }
            return 0;
        }
    }
}
