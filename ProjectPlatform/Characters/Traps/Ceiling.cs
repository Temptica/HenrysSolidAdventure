using System;
using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Traps
{
    internal class Ceiling:Trap
    {
        public static Dictionary<TrapTier, Texture2D> Textures { get; set; }
        public static Dictionary<TrapTier, Texture2D> SpawnTextures { get; set; }

        public override bool CheckDamage()
        {
            if (Animations.CurrentAnimation.State != State.Attacking) return false;
            return true; //every frame does damage
        }

        public Ceiling(Vector2 position, TrapTier tier, bool direction = false) : base(position, tier, direction)
        {
            var width = TextureSizeWidth;
            switch (Tier)
            {
                case TrapTier.One:
                    width = 64;
                    break;
                case TrapTier.Two:
                case TrapTier.Three:
                    width = 96;
                    break;
            }
            Animations = new AnimationList<Animation>()
            {
                new(SpawnTextures[tier], State.Other,SpawnTextures[tier].Width/width,8),
                new(Textures[tier], State.Attacking,Textures[tier].Width / width, 10)
            };
            _loop = false;
            _isActivated = false;
            Position = new Vector2(position.X, position.Y - Textures[tier].Height);
            _animationDelay = 500;
        }
        public override void Update(GameTime gameTime)
        {
            if (Hero.Instance.HitBox.Intersects(new Rectangle(HitBox.X, HitBox.Y, HitBox.Width, Textures[Tier].Height)))
            {
                _isActivated = true;
                _loop = true;
            }
            else _loop = false;
            base.Update(gameTime);
        }
    }
}
