﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Interface;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.EnemyFolder
{
    internal abstract class Enemy : IAnimatable, IGameObject
    {
        protected Enemy()
        {
            CanAttack = true;
        }

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
        public bool Remove { get; set; }
        public AnimationList<Animation> Animations { get; set; }
        public Animation CurrentAnimation=> Animations.CurrentAnimation;
        public virtual Rectangle HitBox
        {
            get => GetHitBox();
            set => throw new NotImplementedException();
        }
        public Vector2 Position { get; set; }
        internal State State { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if((State == State.Attacking && CurrentAnimation.IsFinished) || State is not State.Attacking)
            {
                CanAttack = true;
            }
            
        }
        public abstract void Draw(Sprites spriteBatch);
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