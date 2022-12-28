using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Interface
{
    internal interface IGameObject
    {        
        Vector2 Position { get; set; }
        Rectangle HitBox { get; set; }
        public void Update(GameTime gameTime);
        public void Draw(Sprites sprites);
        
        
    }
}
