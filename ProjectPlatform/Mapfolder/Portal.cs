using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Animations;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Mapfolder
{
    internal class Portal
    {
        public Vector2 Position { get; private set; }
        public static Texture2D Texture;
        public Rectangle HitBox { get; private set; }
        private BasicAnimation _basicAnimation;
        public Portal(Vector2 position)
        {
            Position = position;
            _basicAnimation = new(Texture, "portal", 4f);
        }

        public void Update(GameTime gameTime)
        {
            _basicAnimation.Update(gameTime);
        }
        public void Draw(Sprites sprites)
        {
            _basicAnimation.Draw(sprites,Position,SpriteEffects.None,1f);

        }
    }
}
