using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Mapfolder;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPlatform
{
    internal static class OtterCollision
    {
        public static MapTile OtterGroundHit(Otter otter, List<MapTile> maptiles)
        {
            var MainTileFilter = maptiles.Where(tile => tile.Tile.Type != TileType.Air && tile.HitBox.Intersects(otter.HitBox)).ToList(); // list of tiles that intersect with otter (main hitbox based)
            if (MainTileFilter.Count == 0) return null;
            return MainTileFilter.OrderBy(tile => tile.HitBox.Bottom).FirstOrDefault(); // get the tile that is the 'lowest'
        }
        public static bool PixelBasedHit(Otter otter, Enemy enemy)
        {
            Color[,] otterPixels2D = GetCurrentPixels2D(otter);
            return false;

        }

        private static Color[] GetCurrentPixels(Otter otter)
        {
            Color[] otterPixels = new Color[200 * 200];
            otter.Texture.GetData(0, new Rectangle(otter.CurrentSpriteX, 0,200, 200), otterPixels, 0, otterPixels.Length);

            return otterPixels;
        }

        private static Color[,] GetCurrentPixels2D(Otter otter)
        {
            Color[] otterPixels = GetCurrentPixels(otter);
            //convert otterPixels to 2d array to from a Rectangle
            Color[,] otterPixels2D = new Color[otter.Texture.Width, otter.Texture.Height];
            for (int x = 0; x < 200; x++)
            {
                for (int y = 0; y < 200; y++)
                {
                    otterPixels2D[x, y] = otterPixels[x + y * 200];
                }
            }

            return otterPixels2D;
        }
    }
}