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
        protected float _animationDelay;
        protected float _timer;
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
                Animations.Update(State, gameTime);//update just once
            }

            if (!_isActivated || (!_loop && IsFinished)) return;
            if (IsFinished && _animationDelay > 0 && _timer <= _animationDelay)
            {
                _timer += (float)gameTime.ElapsedGameTime.Milliseconds;
                CanDamage = false;
                return;
            }
            if(_timer >= _animationDelay)
            {
                IsFinished = false;
                CanDamage = true;
            }
            Animations.Update(State, gameTime);
            if (!Animations.CurrentAnimation.IsFinished) return;
            IsFinished = true;
            _timer = 0;


        }
        public override bool GetDamage(int i)
        {
            return false; //all traps except worm can't get damage
        }
    }
}
