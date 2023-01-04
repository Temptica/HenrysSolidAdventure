using System;
using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Traps
{
    internal class Fire:Trap
    {
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

        public Fire(Vector2 position, TrapTier tier, bool direction) : base(position, tier, direction)
        {
            Animations = new AnimationList<Animation>()
            {
                new(SpawnTextures[tier],State.Other,SpawnTextures[tier].Width/TextureSizeWidth,6),
                new(Textures[tier],State.Attacking, Textures[tier].Width / TextureSizeWidth, 6)
            };
        }
    }
}
