using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Graphics;
using System.Collections.Generic;
using OtterlyAdventure.Animations;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.EnemyFolder
{
    internal class Skeleton:RoamingEnemy
    {//somewhat smart, will track when enemies are on the same platform
        public static Dictionary<State, Texture2D> Textures;//list as some of the spritesheets are bigger than others due to the big "sword" making it very difficult having them on one sprite
        private float fixedYPosition;
        public Skeleton(Vector2 position)
        {
            Position = position;
            fixedYPosition = Position.Y;
            Animations = new()
            {//https://jesse-m.itch.io/skeleton-pack
                new Animation(Textures[State.Idle], State.Idle,11, Textures[State.Idle].Width/11, Textures[State.Idle].Height, 0, 0,7),
                new Animation(Textures[State.Walking],State.Walking, 13, Textures[State.Walking].Width/13, Textures[State.Walking].Height, 0, 0,6),
                new Animation(Textures[State.Attacking], State.Attacking,18, Textures[State.Attacking].Width/18, Textures[State.Attacking].Height, 0, 0,10),
                new Animation(Textures[State.Hit], State.Hit,8, Textures[State.Hit].Width/8, Textures[State.Hit].Height, 0, 0,5),
                new Animation(Textures[State.Dead], State.Dead,15, Textures[State.Dead].Width/15, Textures[State.Dead].Height, 0, 0,5),
                new Animation(Textures[State.Other], State.Other,4, Textures[State.Other].Width/4, Textures[State.Other].Height, 0, 0,7)//when skeleton detects Otter
            };
            CurrentHp = BaseHp = 14;
            Damage = 5;
            Speed = 8f;
            CanAttack = true;
            IsWalking = true;
            DefineWalkablePath();

        }
        public override void Update(GameTime gameTime)
        {
            SetState();
            
            Position = new Vector2(Position.X,
                fixedYPosition + (Textures[State.Idle].Height - Textures[State].Height));//makes position correct, even tho texture is higher
            
            base.Update(gameTime);
        }

        internal override void SetState()
        {            
            
            IsAttacking = CheckAttack();

            if (IsAttacking)
            {
                if (State == State.Attacking) return;
                CanDamage = true;//first time attacking animation starts
                State = State.Attacking;
            }
            base.SetState();
            
        }

        public void CheckDamage()
        {
            if (State is State.Attacking && CurrentAnimation.CurrentFrameIndex >= 9 && CurrentAnimation.CurrentFrameIndex <= 6) CanDamage = false;//after 8th frame, skeleton can't damage otter as it lifts up his weapon
        }
        public bool CheckAttack()
        {
            if (!CanAttack) return false;
            var hitBox = Otter.Instance.HitBox;
            if (hitBox.Top >= HitBox.Bottom || hitBox.Bottom <= HitBox.Top) return false;
            return hitBox.Center.X < HitBox.Center.X
                ? hitBox.Right > HitBox.Left - 30
                : hitBox.Left < HitBox.Right + 30; //if otter is left, then check left distance is 30 or less, otherwise do opposite
        }
    }
}
