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
        private float _fadeTimer;
        private readonly float _fadeTime = 3000f;
        public Worm(Vector2 position, TrapTier tier, bool direction, int wormHp = 0) : base(position, tier, direction)
        {
            var size = TextureSizeWidth;
            switch (tier)
            {
                case TrapTier.One:
                    break;
                case TrapTier.Two:
                    
                case TrapTier.Three:
                    size = 96;
                    break;
            }

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

            _loop = false;
            _isActivated = true;
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
            if (_fadeTimer >= 3000) IsFinished = true;
            _fadeTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        public override void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            var effect = !IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Animations.Draw(sprites, Position, effect, 1f);
            //if fading (_timer >0) then make it fade
            if (_fadeTimer > 0)
            {
                var fade = _fadeTimer / _fadeTime;
                Animations.Draw(sprites, Position, effect,1f,0f, Color.Lerp(Color.Transparent,Color.White, _fadeTimer / _fadeTime));
            }

        }
        public override bool CheckDamage()
        {
            if (Animations.CurrentAnimation.State != State.Attacking) return false;
            switch (Tier)
            {
                case TrapTier.One:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 3 and < 7;
                case TrapTier.Two:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 4 and < 8;
                case TrapTier.Three:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 1 and < 16;
                default: return false;
            }
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
