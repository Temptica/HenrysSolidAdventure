using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics
{
    internal class Text
    {
        public Vector2 Position { get; set; }
        private string TextString { get; set; }
        public Color Color { get; set; }
        private float Scale { get; set; }
        private float Rotation { get; set; }
        private SpriteFont Font { get; set; }

        public Text(Vector2 position, string textString, Color color, float scale, float rotation, SpriteFont font)
        {
            Position = position;
            TextString = textString;
            Color = color;
            Scale = scale;
            Rotation = rotation;
            Font = font;
        }
        public void Draw(SpriteBatch sprite)
        {
            sprite.DrawString(Font, TextString, Position, Color, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
