using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPlatform.Interface;

namespace ProjectPlatform.EnemyFolder
{
    internal abstract class Enemy:IAnimatable
    {
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation { get => Animations.First(animation => animation.State == State); }
        public Rectangle HitBox { get; set; }
        public Vector2 Position { get; set; }
        internal State State { get; set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(Sprites spriteBatch);
    }
}
