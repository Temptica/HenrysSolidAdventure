using HenrySolidAdventure.Characters.Enemies;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Characters.Traps
{
    public enum TrapTier{One,Two,Three}
    
    internal abstract class Trap:Enemy
    {

        public TrapTier Tier { get; }
        protected const int TextureSizeWidth = 32;
        protected bool Loop;
        protected bool IsActivated;
        protected float AnimationDelay;
        protected float Timer;
        public bool IsFinished { get; protected set; }

        protected Trap(Vector2 position, TrapTier tier, bool direction)
        {
            Position = position;
            Tier = tier;
            IsFacingLeft = direction;
            CanAttack = true;
            CanDamage = true;
            State = State.Other;
        }
        public override void Update(GameTime gameTime)
        {
            if (State is State.Other)//build up animation
            {
                Animations.Update(State, gameTime);
                if (!Animations.CurrentAnimation.IsFinished) return;
                State = State.Attacking;
                Animations.Update(State, gameTime);//update just once
            }

            
            if (IsFinished && AnimationDelay > 0 && Timer <= AnimationDelay)
            {
                Timer += (float)gameTime.ElapsedGameTime.Milliseconds;
                CanDamage = false;
                return;
            }
            if (!IsActivated || (!Loop && IsFinished)) return;
            if (Timer >= AnimationDelay)
            {
                IsFinished = false;
                CanDamage = true;
            }
            Animations.Update(State, gameTime);
            if (!Animations.CurrentAnimation.IsFinished) return;
            IsFinished = true;
            Timer = 0;


        }
        public override bool GetDamage(int i)
        {
            return false; //all traps except worm can't get damage
        }
    }
}
