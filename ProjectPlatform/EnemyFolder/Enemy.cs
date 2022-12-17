using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPlatform.Interface;

namespace ProjectPlatform.EnemyFolder
{
    internal abstract class Enemy : IAnimatable, IGameObject
    {
        public static List<Enemy> Enemies;
        internal Enemy()
        {
            Enemies ??= new List<Enemy>();
            Enemies.Add(this);
        }

        internal bool IsFacingLeft;
        internal bool IsAttacking;
        internal bool IsWalking;
        internal bool IsIdle;
        internal bool IsDead;
        internal bool IsHit;
        internal bool CanAttack;
        public int BaseHp { get; set; }
        public int Damage { get; set; }
        public float CurrentHp { get; set; }
        public float Speed { get; set; }
        public bool Remove { get; set; }
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation
        {
            get
            {
                try
                {
                    return Animations.First(ani => ani.State == State);
                }
                catch (Exception)
                {
                    return Animations[0];
                }
            }
        }
        public virtual Rectangle HitBox
        {
            get => GetHitBox();
            set => throw new NotImplementedException();
        }
        public Vector2 Position { get; set; }
        internal State State { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if(State == State.Attacking && CurrentAnimation.IsFinished)
            {
                CanAttack = true;
            }
            
        }
        public abstract void Draw(Sprites spriteBatch);
        public abstract void Move(GameTime gameTime);
        
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
            if (CanAttack) {
                CanAttack = false;
                return Damage; 
            }
            return 0;
        }
        private Rectangle GetHitBox()
        {
            if (IsFacingLeft)
            {//invert hitbox
                //return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X * Scale- CurrentAnimation.CurrentFrame.HitBox.Width * Scale)
                return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X), (int)(Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y), (int)(CurrentAnimation.CurrentFrame.HitBox.Width), (int)(CurrentAnimation.CurrentFrame.HitBox.Height));

            }
            return new((int)(Position.X + CurrentAnimation.CurrentFrame.HitBox.X), (int)(Position.Y + CurrentAnimation.CurrentFrame.HitBox.Y), (int)(CurrentAnimation.CurrentFrame.HitBox.Width), (int)(CurrentAnimation.CurrentFrame.HitBox.Height));
        }
    }
}
