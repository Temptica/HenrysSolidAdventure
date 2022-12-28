using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OtterlyAdventure.Mapfolder
{
    public enum TileType
    {
        Flat, UphillLow, UpHillHigh, DownhillHigh, DownHillLow, Air
    }
    public class Tile
    {
        public const float TileSize = 24;
        public Texture2D Texture { get; }
        public Rectangle Rectangle { get; }
        public TileType Type { get; }

        public Tile(Texture2D texture, Rectangle rectangle)
        {
            Texture = texture;
            Rectangle = rectangle;
            Type = CalculateTileType();
        }

        public TileType CalculateTileType()
        {
            int size = Rectangle.Width * Rectangle.Height;
            Color[] buffer = new Color[size];
            Texture.GetData(0, Rectangle, buffer, 0, size);
            List<List<Color>> colors = new List<List<Color>>();
            for (int i = 0; i < Rectangle.Height; i++)
            {
                colors.Add(new List<Color>());
                for (int j = 0; j < Rectangle.Width; j++)
                {
                    colors[i].Add(buffer[i * Rectangle.Width + j]);
                }
            }

            int leftHeight = -1;
            int rightHeight = -1;
            for (int i = 0; i < colors.Count; i++) //checks every pixel if transparent or not.
            {
                if (colors[i][5] != Color.Transparent && leftHeight == -1)//if the 5th pixel from left is not transparent, set value
                {
                    leftHeight = i;
                }

                if (colors[i][^5] != Color.Transparent && rightHeight == -1)//if the 5th pixel from right is not transparent, set value
                {
                    rightHeight = i;
                }
                //5th pixel because the first or last pixels may sometimes be transparent. taking the 5th will guarantee a pixel to be found on the Y axis
                if (leftHeight != -1 && rightHeight != -1)
                {
                    break;
                }
            }

            if (leftHeight == -1 && rightHeight == -1) return TileType.Air;
            //if left height = rightHeight or a margin of 2, return flat, if X is lower, return uphill, else downhill
            if (leftHeight == rightHeight || Math.Abs(leftHeight - rightHeight) <= 5)//margin of 5 for variation of height on flat tiles
            {
                return TileType.Flat;
            }
            if (leftHeight > rightHeight) //height is from top to bottom so invert logic 
            {
                return leftHeight > Rectangle.Height / 2 ? TileType.UphillLow : TileType.UpHillHigh;
            }
            return rightHeight < Rectangle.Height / 2 ? TileType.DownhillHigh : TileType.DownHillLow;
        }
    }
}
