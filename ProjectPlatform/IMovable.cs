using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform
{
    internal interface IMovable
    {
        Vector2 Velocity { get; set; }
        Vector2 Position { get; set; }
        Texture2D Texture { get; set; }
        Rectangle Rectangle { get; set; }
        int MoveSpeed { get; set; }
        int Health { get; set; }
        int Damage { get; set; }
        int AttackRange { get; set; }

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);


    }
}
