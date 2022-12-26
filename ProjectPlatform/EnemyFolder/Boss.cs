using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Animations;
using ProjectPlatform.Graphics;

namespace ProjectPlatform.EnemyFolder
{
    internal class Boss : Enemy
    {//https://luizmelo.itch.io/evil-wizard-2
        public static Texture2D Texture { get; set; }
        public Boss(Vector2 position)
        {
            Position = position;
            Animations = new List<Animation>();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Sprites spriteBatch)
        {
            throw new NotImplementedException();
        }

        public virtual void Move(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
