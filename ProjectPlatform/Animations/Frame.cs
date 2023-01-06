using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Animations
{
    internal class Frame //one single frame in an animation
    {
        public Texture2D Texture { get; }
        public Rectangle FrameRectangle { get; private set; }
        public Rectangle HitBox { get; private set; }
        public Frame(Rectangle frameRectangle,Texture2D texture, float scale)
        {
            FrameRectangle = frameRectangle;
            Texture = texture;
            HitBox = TextureUtil.SetPixelBasedHitBox(texture, frameRectangle); //generate a pixel-based hitbox by itself. no hardcoding needed per animation!
        }
    }
}
