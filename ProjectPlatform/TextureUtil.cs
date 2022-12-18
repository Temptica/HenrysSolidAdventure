using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Mapfolder;

namespace ProjectPlatform
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


    }
}
