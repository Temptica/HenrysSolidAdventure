using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.Characters
{
    public static class TextureUtil
    {
        public static Color[] GetCurrentPixels(Texture2D texture, Rectangle rectangle)
        {
            var pixels = new Color[rectangle.Width * rectangle.Height];
            texture.GetData(0, rectangle, pixels, 0, pixels.Length);
            return pixels;
        }
        public static Color[,] GetCurrentPixels2D(Texture2D texture, Rectangle rectangle)
        {
            var pixels = GetCurrentPixels(texture, rectangle);
            var pixels2D = new Color[rectangle.Width, rectangle.Height];
            for (var x = 0; x < rectangle.Width; x++)
            {
                for (var y = 0; y < rectangle.Height; y++)
                {
                    pixels2D[x, y] = pixels[x + y * rectangle.Width];
                }
            }
            return pixels2D;
        }
        public static Rectangle SetPixelBasedHitBox(Texture2D texture, Rectangle rectangle)
        {
            //get the lowest pixel in teh rectangle
            var color = GetCurrentPixels2D(texture, rectangle);
            var mostLeftPixel = 0;

            //find left most pixel
            for (var x = 0; x < rectangle.Width; x++)
            {
                for (var y = 0; y < rectangle.Height; y++)
                {
                    if (color[x, y].A == 0) continue;
                    mostLeftPixel = x;
                    break;
                }
                if (mostLeftPixel != 0) break;
            }
            //find right most pixel
            var mostRightPixel = 0;
            for (var x = rectangle.Width - 1; x >= 0; x--)
            {
                for (var y = 0; y < rectangle.Height; y++)
                {
                    if (color[x, y].A == 0) continue;
                    mostRightPixel = x;
                    break;
                }
                if (mostRightPixel != 0) break;
            }
            //find highest pixel
            var highestPixel = 0;
            for (var y = 0; y < rectangle.Height; y++)
            {
                for (var x = 0; x < rectangle.Width; x++)
                {
                    if (color[x, y].A == 0) continue;
                    highestPixel = y;
                    break;
                }
                if (highestPixel != 0) break;
            }
            //find lowest pixel
            var lowestPixel = 0;
            for (var y = rectangle.Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < rectangle.Width; x++)
                {
                    if (color[x, y].A == 0) continue;
                    lowestPixel = y;
                    break;
                }
                if (lowestPixel != 0) break;
            }
            //set hitBox
            return new Rectangle(mostLeftPixel, highestPixel, mostRightPixel - mostLeftPixel, lowestPixel - highestPixel);
        }

    }
}
