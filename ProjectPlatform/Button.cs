using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectPlatform
{
    internal class Button //make ButtonList class soon ?
    {
        public string Name { get; }
        public Texture2D Texture { get; }
        public Vector2 Position { get; }
        public Rectangle HitBox { get; }
        public bool IsActive { get; private set; }
        public GameState? ActivationState { get; set; }
        

        public Button(string name,Texture2D texture, Vector2 position, GameState? activationState = null)
        {
            Name = name;
            Texture = texture;
            Position = position;
            HitBox = new Rectangle((int)(Position.X), (int)(Position.Y),Texture.Width, Texture.Height);
            ActivationState = activationState;
            IsActive = true;
        }
        public void Draw(Sprites spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,null, Color.White, 0f, Vector2.One, 1f, SpriteEffects.None, 0f );
        }

        public int CheckHit(Vector2 cursor, bool click) //-1 = no hit / 0 hit no click / 1 hit and click
        {
            //Check if cursor is within Hitbox
            if (!IsActive) return -1;
            
            if((HitBox).Contains(cursor))
            {
                Mouse.SetCursor(MouseCursor.Hand);
                return click ? 1 : 0;
            }
            return -1;
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
