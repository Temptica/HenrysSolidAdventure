using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Characters
{
    internal class Character
    {
        public AnimationList<Animation> Animations { get; set; }
        public virtual Rectangle HitBox => GetHitBox(Position);
        internal bool IsFacingLeft;
        internal bool IsAttacking;
        internal bool IsWalking;
        internal bool IsDead;
        internal bool IsHit;
        internal bool CanAttack;
        internal bool CanDamage;
        public int BaseHp { get; set; }
        public int Damage { get; set; }
        public float CurrentHp { get; set; }
        public float Speed { get; set; }
        protected Vector2 Velocity;
        public virtual void Update(GameTime gameTime){}

        public virtual void Draw(Sprites sprites){}

        public Vector2 Position { get; set; }
        internal Rectangle GetHitBox(Vector2 position)
        {
            var x = position.X + Animations.CurrentAnimation.CurrentFrame.HitBox.X;
            var y = position.Y + Animations.CurrentAnimation.CurrentFrame.HitBox.Y;
            var width = Animations.CurrentAnimation.CurrentFrame.HitBox.Width;
            var height = Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
            if (IsFacingLeft)
            {
                // mirror the origin of your sprite.
                x = position.X + Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Width -
                    Animations.CurrentAnimation.CurrentFrame.HitBox.Right;
            }
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        internal Vector2 GetPosition(Rectangle hitBox)
        {
            //convert back to position
            var x = hitBox.X - Animations.CurrentAnimation.CurrentFrame.HitBox.X;
            var y = hitBox.Y - Animations.CurrentAnimation.CurrentFrame.HitBox.Y;
            if (IsFacingLeft)
            {
                // mirror the origin of your sprite.
                x = hitBox.X + Animations.CurrentAnimation.CurrentFrame.HitBox.Right -
                    Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Width;
            }
            return new Vector2(x, y);


        }
        
    }
}
