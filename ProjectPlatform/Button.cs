using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform
{
    internal enum TextAlignment{Left, Center, Right}
    internal class Button //make ButtonList class soon ?
    {
        public string Name { get; }
        public Texture2D Texture { get; }
        public Vector2 Position { get; }
        public Rectangle HitBox { get; }
        public bool IsActive { get; private set; }
        public GameState? ActivationState { get; set; }
        public float Scale { get; set; }
        

        public Button(string name,Texture2D texture, Vector2 position, GameState? activationState = null, float scale = 1f)
        {
            Name = name;
            Texture = texture;
            Position = position;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            ActivationState = activationState;
            IsActive = true;
            Scale = scale;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,null, Color.White, 0f, Vector2.One, Scale, SpriteEffects.None, 0f );
        }

        public bool CheckHit(Vector2 cursor)
        {
            //Check if cursor is within Hitbox
            return HitBox.Contains(cursor);

        }

        public void UpdateActive(bool? isActive = null)
        {
            if (isActive is null)
            {
                IsActive=!IsActive;
                return;
            }
            IsActive = (bool)isActive;
        }
        public void UpdateActive(GameState activeState)
        {
            if (ActivationState is not null)
            {
                IsActive = ActivationState == activeState;
            }
        }

    }
}
