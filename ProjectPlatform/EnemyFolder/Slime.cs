using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;
using OtterlyAdventure.Interface;
using OtterlyAdventure.OtterFolder;

namespace OtterlyAdventure.EnemyFolder
{
    internal class Slime:RoamingEnemy
    {//slimes are dumb and won't track
        public static Texture2D Texture;
        public Slime(Vector2 position)
        {
            Position = position;
            var frameWidth = Texture.Width / 8;
            var frameHeight = Texture.Height/3;
            Animations = new List<Animation>
            {
                new(Texture, State.Idle, 4, frameWidth, frameHeight, 0, 0, 8),
                new(Texture, State.Walking, 4, frameWidth, frameHeight, 0, frameWidth * 4, 8),
                new(Texture, State.Attacking, 5, frameWidth, frameHeight, frameHeight, 0, 8),
                new(Texture, State.Hit, 4, frameWidth, frameHeight, frameHeight, frameWidth * 5, 8),
                new(Texture, State.Dead, 4, frameWidth, frameHeight, frameHeight * 2, frameWidth, 8)
            };
            CurrentHp = BaseHp = 6;
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

        public override void Draw(Sprites spriteBatch)
        {//texture is flipped compared to other enemies
            CurrentAnimation.Draw(spriteBatch, Position, !IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
        }


    }
}
