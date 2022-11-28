using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform.Animations
{
    internal class Frame
    {
        public Rectangle FrameRectangle { get; private set; }
        public Rectangle HitBox { get; private set; }
        public Frame(Rectangle frameRectangle,Texture2D texture, float scale)
        {
            FrameRectangle = frameRectangle;
            SetPixelBasedHitBox(texture);
        }
        private void SetPixelBasedHitBox(Texture2D texture)
        {
            //get the lowest pixel in teh rectangle
            var color = TextureUtil.GetCurrentPixels2D(texture, FrameRectangle);
            int mostLeftPixel = 0;
            
            //find left most pixel
            for (int x = 0; x < FrameRectangle.Width; x++)
            {
                for (int y = 0; y < FrameRectangle.Height; y++)
                {
                    if (color[x, y].A == 0) continue;
                    mostLeftPixel = x;
                    break;
                }
                if (mostLeftPixel != 0) break;
            }
            //find right most pixel
            int mostRightPixel = 0;
            for (int x = FrameRectangle.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < FrameRectangle.Height; y++)
                {
                    if (color[x, y].A == 0) continue;
                    mostRightPixel = x;
                    break;
                }
                if (mostRightPixel != 0) break;
            }
            //find highest pixel
            int highestPixel = 0;
            for (int y = 0; y < FrameRectangle.Height; y++)
            {
                for (int x = 0; x < FrameRectangle.Width; x++)
                {
                    if (color[x, y].A == 0) continue;
                    highestPixel = y;
                    break;
                }
                if (highestPixel != 0) break;
            }
            //find lowest pixel
            int lowestPixel = 0;
            for (int y = FrameRectangle.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < FrameRectangle.Width; x++)
                {
                    if (color[x, y].A == 0) continue;
                    lowestPixel = y;
                    break;
                }
                if (lowestPixel != 0) break;
            }
            //set hitbox
            HitBox = new Rectangle(mostLeftPixel, (int)(highestPixel), (int)((mostRightPixel - mostLeftPixel)), (int)((lowestPixel - highestPixel)));
        }
    }
}
