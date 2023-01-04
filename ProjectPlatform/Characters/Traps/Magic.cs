using System;
using System.Collections.Generic;
using HenrySolidAdventure.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters.Traps
{
    internal class Magic:Trap
    {
        private float _size;
        public Magic(Vector2 position, TrapTier tier, bool isFacingLeft) : base(position,tier,isFacingLeft)
        {
            var size = TextureSizeWidth;
            switch (tier)
            {
                case TrapTier.One:
                    break;
                case TrapTier.Two:
                    size = 96;
                    break;
                case TrapTier.Three:
                    size = 144;
                    break;
            }

            var fps = 12;
            Animations = tier is TrapTier.Three
                ? new AnimationList<Animation>()//tier 3
                {
                    new(SpawnTextures[tier], State.Other, SpawnTextures[tier].Width / 96, fps * 2),
                    new(Textures[tier], State.Attacking, (Textures[tier].Width / size) * 2, size,
                        Textures[tier].Height / 2, 0, 0, fps)
                }
                : new AnimationList<Animation>() //tier 1 and 2
                {
                    new(SpawnTextures[tier], State.Other, SpawnTextures[tier].Width / size, fps * 2),
                    new(Textures[tier], State.Attacking, Textures[tier].Width / size, fps)
                };
            _isActivated = true;
            _loop = false;
            //put the position so the bottom of the trap is at the position as well as the position half to teh left
            Position = new Vector2(position.X - (size / 2), position.Y - (Textures[tier].Height / 2));
            Damage = 4 + (int)tier * 3;
            _size = size;
        }

        public static Dictionary<TrapTier, Texture2D> Textures { get; set; }
        public static Dictionary<TrapTier, Texture2D> SpawnTextures { get; set; }
        public override bool CheckDamage()
        {
            if (Animations.CurrentAnimation.State != State.Attacking) return false;
            switch (Tier)
            {
                case TrapTier.One:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 11 and < 17;
                case TrapTier.Two:
                    return Animations.CurrentAnimation.CurrentFrameIndex is >15 and < 24;
                case TrapTier.Three:
                    return Animations.CurrentAnimation.CurrentFrameIndex is > 8 and < 24;
                default: return false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (State is not State.Attacking && Vector2.Distance(Hero.Instance.HitBox.Center.ToVector2(),HitBox.Center.ToVector2()) > 30)
            {
                if (Hero.Instance.HitBox.X <= HitBox.X) //move near the hero
                {
                    Position += new Vector2(-1, 0);
                }
                else
                {
                    Position += new Vector2(1, 0);

                }
                //do teh same heightweise
                if (Hero.Instance.HitBox.Y <= HitBox.Y)
                {
                    Position += new Vector2(0, -1);
                }
                else
                {
                    Position += new Vector2(0, 1);
                }
            }

            base.Update(gameTime);
        }

    }
}
