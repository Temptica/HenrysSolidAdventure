using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.MapFolder
{
    internal class Decoration
    {
        internal Texture2D Texture;
        internal Vector2 Position;
        private readonly float _scale;
        private readonly SpriteEffects _effect;
        public Decoration(Texture2D texture, Vector2 position, float scale, SpriteEffects effect = SpriteEffects.None)
        {
            Texture = texture;
            Position = position;
            _scale = scale;
            _effect = effect;
        }
        public void Draw(Sprites spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, _scale, _effect, 1f);
        }
    }
}
