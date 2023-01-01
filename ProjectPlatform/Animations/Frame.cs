using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Animations
{
    internal class Frame
    {
        public Texture2D Texture { get; }
        public Rectangle FrameRectangle { get; private set; }
        public Rectangle HitBox { get; private set; }
        public Frame(Rectangle frameRectangle,Texture2D texture, float scale)
        {
            FrameRectangle = frameRectangle;
            Texture = texture;
            HitBox = TextureUtil.SetPixelBasedHitBox(texture, frameRectangle);
        }
    }
}
