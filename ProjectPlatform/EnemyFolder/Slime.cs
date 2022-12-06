using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Graphics;

namespace ProjectPlatform.EnemyFolder
{
    internal class Slime:RoamingEnemy
    {//slimes are dumb and won't track
        public static Texture2D Texture;
        public Slime() : base()
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Sprites spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Move(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
