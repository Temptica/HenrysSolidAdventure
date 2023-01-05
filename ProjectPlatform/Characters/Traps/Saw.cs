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

        public Saw(Vector2 position, TrapTier tier, bool direction = false) : base(position, tier, direction)
        {
            if (tier is TrapTier.Three)
            {
                Animations = new AnimationList<Animation>()
                {
                    new(SpawnTextures[tier],State.Other,SpawnTextures[tier].Width/96,10),
                    new(Textures[tier],State.Attacking, Textures[tier].Width / 96, 10)
                };
            }
            else
            {
                Animations = new AnimationList<Animation>()
                {
                    new(SpawnTextures[tier],State.Other,SpawnTextures[tier].Width/64,10),
                    new(Textures[tier],State.Attacking, Textures[tier].Width / 64, 10)
                };
            }
            
            _isActivated = true;
            _loop = true;
            Position = new Vector2(position.X, position.Y - Textures[tier].Height);
            Damage = 4;
        }
    }
}
