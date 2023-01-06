using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Enemies.Roaming
{
    internal class Slime : RoamingEnemy
    {//slimes are dumb and won't track enemies. (perhaps make them run onto slopes?)
        public static Texture2D Texture;
        public Slime(Vector2 position)
        {
            Position = position;
            var frameWidth = Texture.Width / 8;
            var frameHeight = Texture.Height / 3;
            Animations = new AnimationList<Animation>
            {
                new(Texture, State.Idle, 4, frameWidth, frameHeight, 0, 0, 6),
                new(Texture, State.Walking, 4, frameWidth, frameHeight, 0, frameWidth * 4, 8),
                new(Texture, State.Attacking, 5, frameWidth, frameHeight, frameHeight, 0, 4),
                new(Texture, State.Hit, 4, frameWidth, frameHeight, frameHeight, frameWidth * 5, 4),
                new(Texture, State.Dead, 4, frameWidth, frameHeight, frameHeight * 2, frameWidth, 4)
            };
            Health = BaseHp = 6;
            Damage = 3;
            IsWalking = true;
            CanAttack = true;
            Speed = 16f;
            DefineWalkablePath();
        }
        public override void Update(GameTime gameTime)
        {
            SetState();
            base.Update(gameTime);
        }

        public override void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {//texture is flipped compared to other enemies
            Animations.Draw(sprites, Position, !IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);

        }
        public override bool CheckDamage()
        {
            return State is State.Attacking && CurrentAnimation.CurrentFrameIndex > 0 && CurrentAnimation.CurrentFrameIndex < 4;
            //before 7th grame, it's lifting up it's weapon after 9th frame, skeleton can't damage otter as it lifts up his weapon
        }
        public override bool CheckAttack()
        {
            if (!CanAttack || Hero.Instance.IsInvisible) return false;
            var hitBox = Hero.Instance.HitBox;
            if (hitBox.Top >= HitBox.Bottom || hitBox.Bottom <= HitBox.Top) return false;
            if (hitBox.Center.X < HitBox.Center.X)
            {
                if (hitBox.Right > HitBox.Left - 35)
                {
                    IsFacingLeft = true;
                    if (hitBox.Right > HitBox.Left + 2)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (hitBox.Left < HitBox.Right + 35)
                {
                    IsFacingLeft = false;
                    if (hitBox.Left < HitBox.Right - 2)
                    {
                        return true;
                    }
                }

            }

            return false;
        }
        public override bool GetDamage(int i)
        {
            AudioController.Instance.PlayEffect(SoundEffects.SlimeHit);
            return base.GetDamage(i);
        }


    }
}
