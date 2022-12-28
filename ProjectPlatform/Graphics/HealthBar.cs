using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OtterlyAdventure.Graphics
{
    internal class HealthBar
    {
        public static Texture2D Texture;
        public static Texture2D BarTexture;
        public static Vector2 BarPosition;
        private float hpPercentage;
        private Vector2 _position;
        private float _scale;
        public HealthBar(Vector2 Position, float scale = 1f)
        {
            _position = Position;
            _scale = scale;
            //bar starts at 21:13 and goes to 76:16 and is in same texture
            BarPosition = new Vector2(21, 13);
            hpPercentage = 100;
        }
        public void Draw(Sprites spriteBatch)
        {//Texture2D background, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float layerDepth
            spriteBatch.Draw(Texture, _position,null, Color.White,0, Vector2.One,_scale,SpriteEffects.None,0f);
            spriteBatch.Draw(BarTexture, null, Vector2.One, _position+BarPosition*_scale, 0, new Vector2(hpPercentage/100, 1)*_scale, Color.White);
        }

        public void SetHealth(float hpPercentage)
        {
            this.hpPercentage = hpPercentage;
        }
    }
}
