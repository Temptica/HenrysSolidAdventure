using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.EnemyFolder
{
    internal abstract class TrackingEnemy : Enemy
    {//bat
        public abstract override void Draw(Sprites spriteBatch);

        public abstract override void Update(GameTime gameTime);
    }
}
