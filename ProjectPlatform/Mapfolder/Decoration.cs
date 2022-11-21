using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.Mapfolder
{
    internal class Decoration
    {
        Texture2D Texture;
        Vector2 Position;
        float Scale;
        SpriteEffects Effect;
        public Decoration(Texture2D texture, Vector2 position, float scale, SpriteEffects effect = SpriteEffects.None)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            Effect = effect;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, Effect, 1f);
        }
    }
}
