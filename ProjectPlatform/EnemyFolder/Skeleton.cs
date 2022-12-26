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
                new Animation(Textures[OtterFolder.State.Idle], OtterFolder.State.Idle,11, Textures[OtterFolder.State.Idle].Width/11, Textures[OtterFolder.State.Idle].Height, 0, 0,7),
                new Animation(Textures[OtterFolder.State.Walking],OtterFolder.State.Walking, 13, Textures[OtterFolder.State.Walking].Width/13, Textures[OtterFolder.State.Walking].Height, 0, 0,6),
                new Animation(Textures[OtterFolder.State.Attacking], OtterFolder.State.Attacking,18, Textures[OtterFolder.State.Attacking].Width/18, Textures[OtterFolder.State.Attacking].Height, 0, 0,10),
                new Animation(Textures[OtterFolder.State.Hit], OtterFolder.State.Hit,8, Textures[OtterFolder.State.Hit].Width/8, Textures[OtterFolder.State.Hit].Height, 0, 0,5),
                new Animation(Textures[OtterFolder.State.Dead], OtterFolder.State.Dead,15, Textures[OtterFolder.State.Dead].Width/15, Textures[OtterFolder.State.Dead].Height, 0, 0,5),
                new Animation(Textures[OtterFolder.State.Other], OtterFolder.State.Other,4, Textures[OtterFolder.State.Other].Width/4, Textures[OtterFolder.State.Other].Height, 0, 0,7)//when skeleton detects Otter
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
                fixedYPosition + (Textures[OtterFolder.State.Idle].Height - Textures[State].Height));//makes position correct, even tho texture is higher
            if (State is OtterFolder.State.Walking)
            {

                Move(gameTime);

            }
            base.Update(gameTime);
        }

        private void SetState()
        {
            if (State is OtterFolder.State.Dead && CurrentAnimation.IsFinished)
            {
                Remove = true;
                return;
            }
            if (IsDead) State = OtterFolder.State.Dead;
            else if (IsHit) State = OtterFolder.State.Hit;
            if (State is OtterFolder.State.Dead || IsDead) return;
            if ((State is OtterFolder.State.Hit && !CurrentAnimation.IsFinished)||(!CurrentAnimation.IsFinished && State == OtterFolder.State.Attacking))
            {
                CanAttack = false;
                if (State is OtterFolder.State.Attacking && CurrentAnimation.CurrentFrameIndex >= 9 && CurrentAnimation.CurrentFrameIndex <= 6) CanDamage = false;//after 8th frame, skeleton can't damage otter as it lifts up his weapon
                return;
            };
            if (State is OtterFolder.State.Hit or OtterFolder.State.Attacking && CurrentAnimation.IsFinished)
            {
                IsHit = false;
                CanAttack = true;
                State = OtterFolder.State.Idle;
            }
            IsAttacking = CheckAttack();

            if (IsAttacking)
            {
                if (State == OtterFolder.State.Attacking) return;
                CanDamage = true;//first time attacking animation starts
                State = OtterFolder.State.Attacking;
            }
            else if (IsWalking) State = OtterFolder.State.Walking;
            else State = OtterFolder.State.Idle;
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
