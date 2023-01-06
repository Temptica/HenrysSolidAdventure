using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Traps
{
    internal class Lightning:Trap
    {
        public static Dictionary<TrapTier, Texture2D> Textures { get; set; }
        public static Dictionary<TrapTier, Texture2D> SpawnTextures { get; set; }
        public Lightning(Vector2 position, TrapTier tier, bool direction = false) : base(position, tier, direction)
        {
            Animations = new AnimationList<Animation>
            {
                new(SpawnTextures[tier],State.Other,SpawnTextures[tier].Width/(TextureSizeWidth*3),10),
                new(Textures[tier],State.Attacking, Textures[tier].Width / (TextureSizeWidth*3), 10)
            };
            IsActivated = true;
            Loop = true;
            AnimationDelay = 400f;
            Position = new Vector2(position.X, position.Y - Textures[tier].Height);

            switch (tier)
            {
                case TrapTier.One:
                case TrapTier.Two:
                    Damage = 3;
                    break;
                case TrapTier.Three:
                    Damage = 5;
                    break;
            }
        }
        
        public override bool CheckDamage()
        {
            switch (Tier)
            {
                case TrapTier.One:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 2 and < 9;
                case TrapTier.Two:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 2 and <11;
                case TrapTier.Three:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 2 and < 18;
            }
            return false;

        }
    }
}
