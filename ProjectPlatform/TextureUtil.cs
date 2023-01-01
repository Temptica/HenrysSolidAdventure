﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure
{
    public static class TextureUtil
    {
        public static Color[] GetCurrentPixels(Texture2D texture, Rectangle rectangle)
        {
            Color[] Pixels = new Color[rectangle.Width * rectangle.Height];
            texture.GetData(0, rectangle, Pixels, 0, Pixels.Length);
            return Pixels;
        }
        public static Color[,] GetCurrentPixels2D(Texture2D texture, Rectangle rectangle)
        {
            Color[] Pixels = GetCurrentPixels(texture, rectangle);
            Color[,] Pixels2D = new Color[rectangle.Width, rectangle.Height];
            for (int x = 0; x < rectangle.Width; x++)
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    Pixels2D[x, y] = Pixels[x + y * rectangle.Width];
                }
            }
            return Pixels2D;
        }
        public static Rectangle SetPixelBasedHitBox(Texture2D texture, Rectangle rectangle)
        {
            //get the lowest pixel in teh rectangle
            var color = GetCurrentPixels2D(texture, rectangle);
            int mostLeftPixel = 0;

            //find left most pixel
            for (int x = 0; x < rectangle.Width; x++)
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    if (color[x, y].A == 0) continue;
                    mostLeftPixel = x;
                    break;
                }
                if (mostLeftPixel != 0) break;
            }
            //find right most pixel
            int mostRightPixel = 0;
            for (int x = rectangle.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    if (color[x, y].A == 0) continue;
                    mostRightPixel = x;
                    break;
                }
                if (mostRightPixel != 0) break;
            }
            //find highest pixel
            int highestPixel = 0;
            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    if (color[x, y].A == 0) continue;
                    highestPixel = y;
                    break;
                }
                if (highestPixel != 0) break;
            }
            //find lowest pixel
            int lowestPixel = 0;
            for (int y = rectangle.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    if (color[x, y].A == 0) continue;
                    lowestPixel = y;
                    break;
                }
                if (lowestPixel != 0) break;
            }
            //set hitbox
            return new Rectangle(mostLeftPixel, (int)(highestPixel), (int)((mostRightPixel - mostLeftPixel)), (int)((lowestPixel - highestPixel)));
        }

    }
}
