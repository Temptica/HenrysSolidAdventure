using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.GameScreens
{
    internal interface IGameScreen
    {
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch, Sprites sprites);
    }
}
