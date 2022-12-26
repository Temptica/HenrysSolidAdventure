using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.GameScreens
{
    internal interface IGameScreen
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, Sprites sprites);
    }
}
