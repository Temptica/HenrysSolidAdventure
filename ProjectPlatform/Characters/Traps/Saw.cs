using System;
using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Traps
{
    internal class Saw:Trap
    {
        public static Dictionary<TrapTier, Texture2D> Textures { get; set; }
        public static Dictionary<TrapTier, Texture2D> SpawnTextures { get; set; }
        public override bool CheckDamage()
        {
            if (Animations.CurrentAnimation.State != State.Attacking) return false;
            return true; //every frame does damage
        }

        public Saw(Vector2 position, TrapTier tier, bool direction) : base(position, tier, direction)
        {
            Animations = new AnimationList<Animation>()
            {
                new(SpawnTextures[tier],State.Other,SpawnTextures[tier].Width/TextureSizeWidth,6),
                new(Textures[tier],State.Attacking, Textures[tier].Width / TextureSizeWidth, 6)
            };
        }
    }
}
