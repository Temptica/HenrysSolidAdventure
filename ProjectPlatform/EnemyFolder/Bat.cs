using Microsoft.Xna.Framework;
using ProjectPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;

namespace ProjectPlatform.EnemyFolder
{
    internal class Bat : TrackingEnemy
    {
        //flying tracking enemy. will start flying to you when you are 15 tiles away, regardless of walls. Once detected, it will keep tracking
        Vector2 _velocity;
        float _flyingSpeed;
        float _roamingSpeed;
        public Bat(Texture2D texture, Vector2 position)
        {
            _velocity = Vector2.Zero;
            Animations = new();
            Animations.Add(new Animation(texture, 4, 10));
        }
        public override void Update(GameTime gameTime)
        {
            CurrentAnimation.Update(gameTime);
        }
        public override void Draw(Sprites spriteBatch)
        {
            CurrentAnimation.Draw(spriteBatch, Position, SpriteEffects.None, 1f);
        }

        
    }
}
