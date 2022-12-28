using System;
using Microsoft.Xna.Framework;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Characters
{
    internal class Character
    {
        public AnimationList<Animation> Animations { get; set; }
        public virtual Rectangle HitBox => GetHitBox();
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
        internal Rectangle GetHitBox()
        {
            var x = Position.X + Animations.CurrentAnimation.CurrentFrame.HitBox.X;
            var y = Position.Y + Animations.CurrentAnimation.CurrentFrame.HitBox.Y;
            var width = Animations.CurrentAnimation.CurrentFrame.HitBox.Width;
            var height = Animations.CurrentAnimation.CurrentFrame.HitBox.Height;
            if (IsFacingLeft)
            {
                //invert hitbox so it is correct when flipped
                x = Position.X + Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Width - Animations.CurrentAnimation.CurrentFrame.HitBox.Width;
            }
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }
    }
}
