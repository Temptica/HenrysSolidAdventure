using System;
using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Traps
{
    internal class Worm:Trap
    {

        public static Dictionary<TrapTier, Texture2D> Textures { get; set; }
        public static Dictionary<TrapTier, Texture2D> SpawnTextures { get; set; }
        private float _timer;
        private float _removeTimer;
        private const float RemoveTime = 3000f;

        public Worm(Vector2 position, TrapTier tier, bool direction, int wormHp = 0) : base(position, tier, direction)
        {
            var size = tier switch
            {
                TrapTier.One => TextureSizeWidth,
                TrapTier.Two => 96,
                TrapTier.Three => 124,
                _ => TextureSizeWidth
            };

            Animations = new AnimationList<Animation>()
            {
                new(SpawnTextures[tier],State.Other,SpawnTextures[tier].Width/size,12),
                new(Textures[tier],State.Attacking, Textures[tier].Width / size, 8)
            };
            Damage = 3 + 2 * (int)tier;
            if (wormHp == 0)
            {
                Health = BaseHp = 100 + 50 * (int)tier;
            }
            else
            {
                Health = BaseHp = wormHp;
            }

            Loop = false;
            IsActivated = true;
            _timer = 0;
        }
        public override void Update(GameTime gameTime)
        {
            
            if (State is State.Other)
            {
                Animations.Update(State, gameTime);
                if (!Animations.CurrentAnimation.IsFinished) return;
                State = State.Attacking;
                _timer = 0;
                Animations.Update(State, gameTime);
            }

            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
                    if (_timer <= 1000) return; //stay in first frame till 3 seconds passed
            if (!Animations.CurrentAnimation.IsFinished)
            {
                Animations.Update(State, gameTime);
                return;
            }
            if (_removeTimer >= RemoveTime) IsFinished = true;
            _removeTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        public override void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            var effect = !IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Animations.Draw(sprites, Position, effect, 1f);

        }
        public override bool CheckDamage()
        {
            if (Animations.CurrentAnimation.State != State.Attacking) return false;
            return Tier switch
            {
                TrapTier.One => Animations.CurrentAnimation.CurrentFrameIndex is > 3 and < 7,
                TrapTier.Two => Animations.CurrentAnimation.CurrentFrameIndex is > 4 and < 8,
                TrapTier.Three => Animations.CurrentAnimation.CurrentFrameIndex is > 1 and < 16,
                _ => false
            };
        }

        public override bool GetDamage(int i)
        {
            if (Animations.CurrentAnimation.State != State.Attacking) return false;
            switch (Tier)
            {
                case TrapTier.One when Animations.CurrentAnimation.CurrentFrameIndex is >3 and <10:
                    Health -= i;
                    break;
                case TrapTier.Two when Animations.CurrentAnimation.CurrentFrameIndex is > 4 and < 11:
                    Health -= i;
                    break;
                case TrapTier.Three when Animations.CurrentAnimation.CurrentFrameIndex is > 1 and < 20:
                    Health -= i;
                    break;
            }
            return Health > 0;
        }

    }
}
