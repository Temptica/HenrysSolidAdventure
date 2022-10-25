using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform
{
    internal interface IMoveable
    {
        public void Update(GameTime gameTime);
        public void MoveLeft();
        public void MoveRight();
        public void Jump();
        public bool OnGround();
        public void Draw(SpriteBatch spriteBatch);
    }
}
