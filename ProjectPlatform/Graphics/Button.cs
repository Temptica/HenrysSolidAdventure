using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics
{
    internal class Button: IClickable //make ButtonList class soon ?
    {
        public string Name { get; }
        public Texture2D Texture { get; }
        public Vector2 Position { get; }
        public Rectangle HitBox { get; set; }
        public bool IsActive { get;}
        public Text Text { get; set; }
        public float Scale { get; }


        public Button(string name, Texture2D texture, Vector2 position, string text = null, float scale = 1f)
        {
            Name = name;
            Texture = texture;
            Position = position;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            Scale = scale;
            IsActive = true;
            if (text != null)
            {
                //center text in button
                var length = Game1.MainFont.MeasureString(text).Length();
                //text height
                var height = Game1.MainFont.MeasureString(text).Y;
                var halfWidth = Texture.Width / 2f ;
                var halfHeight = Texture.Height / 2f;

                var textScale = scale;
                if (length > Texture.Width - 40)
                {
                    textScale = (Texture.Width - 40) / length;
                    //move text to the right
                    length *= textScale*scale;
                    height *= textScale*scale;
                }
                var textPosition = new Vector2((Position.X + halfWidth - length / 2) + 15, (Position.Y + halfHeight - height / 2) + 8);

                Text = new Text(textPosition, text, Color.Black, textScale*scale, 0f, Game1.MainFont);
                //rescale text if too long with 20 pixels margin left and right
                
            }
            else Text = null;
        }
        public void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            sprites.Draw(Texture, Position, null, Color.White, 0f, Vector2.One, Scale, SpriteEffects.None, 0f);
            if (Text != null)
            {
                Text.Draw(spriteBatch);
            }
        }

    }
}
