using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Characters.Traps
{
    public enum TrapTier{One,Two,Three}
    
    internal abstract class Trap:Enemy
    {

        public TrapTier Tier { get; }
        protected const int TextureSizeWidth = 32;
        protected bool _loop;
        protected bool _isActivated;
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
            if (State is State.Other)
            {
                Animations.Update(State, gameTime);
                if (!Animations.CurrentAnimation.IsFinished) return;
                State = State.Attacking;
                
            }

            if (!_isActivated) return;
            Animations.Update(State, gameTime);
            if (_loop|| !Animations.CurrentAnimation.IsFinished) return;
            IsFinished = true;


        }
        public override bool GetDamage(int i)
        {
            return false; //all traps except worm can't get damage
        }
    }
}
