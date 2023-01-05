using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Mapfolder
{
    internal class Decoration
    {
        internal Texture2D Texture;
        internal Vector2 Position;
        private readonly float Scale;
        private readonly SpriteEffects Effect;
        public Decoration(Texture2D texture, Vector2 position, float scale, SpriteEffects effect = SpriteEffects.None)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            Effect = effect;
        }
        public void Draw(Sprites spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, Effect, 1f);
        }
    }
}
