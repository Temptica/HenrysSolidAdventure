using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OtterlyAdventure.Animations
{
    internal class Frame
    {
        public Rectangle FrameRectangle { get; private set; }
        public Rectangle HitBox { get; private set; }
        public Frame(Rectangle frameRectangle,Texture2D texture, float scale)
        {
            FrameRectangle = frameRectangle;
            HitBox = TextureUtil.SetPixelBasedHitBox(texture, frameRectangle);
        }
    }
}
