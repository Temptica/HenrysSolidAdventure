using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Graphics
{
    internal class Button: Clickable //make ButtonList class soon ?
    {
        public static SpriteFont Font;
        public string Name { get; }
        public Texture2D Texture { get; }
        public Vector2 Position { get; }
        public override Rectangle HitBox { get; protected set; }
        public bool IsActive { get; private set; }
        public GameState? ActivationState { get; set; }
        public Text Text { get; set; }


        public Button(string name, Texture2D texture, Vector2 position, string text = null, GameState? activationState = null)
        {
            Name = name;
            Texture = texture;
            Position = position;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            ActivationState = activationState;
            IsActive = true;
            if (text != null)
            {
                //center text in button
                var length = Font.MeasureString(text).Length();
                //text height
                var height = Font.MeasureString(text).Y;
                var halfWidth = Texture.Width / 2f;
                var halfHeight = Texture.Height / 2f;
                
                
                var scale = 1f;
                if (length > Texture.Width - 40)
                {
                    scale = (Texture.Width - 40) / length;
                    //move text to the right
                    length *= scale;
                    height *= scale;
                }
                var textPosition = new Vector2((Position.X + halfWidth - length / 2) + 15, (Position.Y + halfHeight - height / 2) + 8);

                Text = new Text(textPosition, text, Color.Black, scale, 0f, Font);
                //rescale text if too long with 20 pixels margin left and right
                
            }
            else Text = null;
        }
        public void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            sprites.Draw(Texture, Position, null, Color.White, 0f, Vector2.One, 1f, SpriteEffects.None, 0f);
            if (Text != null)
            {
                Text.Draw(spriteBatch);
            }
        }

    }
}
