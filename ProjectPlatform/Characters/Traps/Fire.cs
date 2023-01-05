using System;
using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Traps
{
    internal class Fire:Trap
    {
        private float _detectionRange;
        public static Dictionary<TrapTier, Texture2D> Textures { get; set; }
        public static Dictionary<TrapTier, Texture2D> SpawnTextures { get; set; }
        public override bool CheckDamage()
        {
            if (Animations.CurrentAnimation.State != State.Attacking) return false;
            switch (Tier)
            {
                case TrapTier.One:
                case TrapTier.Two:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 3 and < 6;
                case TrapTier.Three:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 3 and < 10;
                default: return false;
            }
        }

        public Fire(Vector2 position, TrapTier tier, bool direction = false) : base(position, tier, direction)
        {
            Animations = new AnimationList<Animation>()
            {
                new(SpawnTextures[tier],State.Other,SpawnTextures[tier].Width/TextureSizeWidth,10),
                new(Textures[tier],State.Attacking, Textures[tier].Width / TextureSizeWidth, 10)
            };
            _isActivated = false;
            _loop = false;
            Position = new Vector2(position.X, position.Y - Textures[tier].Height);
            if (tier is TrapTier.One)
            {
                _loop = true;
                _animationDelay = 150f;
                _timer = 101f;
                _detectionRange = 50;
            }
            else if(Tier is TrapTier.Three)
            {
                Damage = 5;
                _detectionRange = 500;
                
                _animationDelay = 200f;
            }
            else
            {
                _detectionRange = 500;
                _animationDelay = 300f;
            }
            Damage = 2;
            
        }
        public override void Update(GameTime gameTime)
        {

            if (Vector2.Distance(Hero.Instance.HitBox.Center.ToVector2(), HitBox.Center.ToVector2()) < 50)
            {
                _isActivated = true;
                _loop = true;
            }
            else _loop = false;
            base.Update(gameTime);
        }
    }
}
