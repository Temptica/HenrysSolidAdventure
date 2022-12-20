using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.Graphics
{
    internal class Text
    {
        public Vector2 Position { get; set; }
        string TextString { get; set; }
        Color Color { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
        SpriteFont Font { get; set; }

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
