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
        public static float OtterGroundHit(Rectangle otterHitBox, List<MapTile> maptiles)
        {            
            var MainTileFilter = maptiles.Where(tile => tile.Tile.Type != TileType.Air && tile.HitBox.Intersects(otterHitBox) && ((tile.HitBox.Top<= otterHitBox.Bottom && otterHitBox.Bottom - tile.HitBox.Top <50)||tile.Tile.Type != TileType.Flat)).ToList(); // list of tiles that intersect with otter (main hitbox based)
            if (MainTileFilter.Count == 0) return -1;
            var sorted = MainTileFilter.MinBy(tile => tile.HitBox.Top);
            if (MainTileFilter.TrueForAll(tile => tile.Tile.Type is TileType.Flat or TileType.Air)) return sorted.Position.Y;//no slopes
            float height = 0;
            if (MainTileFilter[0].Tile.Type is not TileType.Flat)
            {
                var HighestTile = MainTileFilter[0];
                switch (HighestTile.Tile.Type)
                {
                    case TileType.UphillLow: //on the right
                        var onTileDistance = otterHitBox.Right - HighestTile.HitBox.Left;
                        //uphill is 2x for 1y staring from the bottom of the block
                        height = (float)(HighestTile.HitBox.Bottom - Math.Ceiling(onTileDistance / 2f) - HighestTile.Tile.Rectangle.Height / 2);

                        break;
                    case TileType.UpHillHigh://on the right
                        //uphill is 2x for 1y staring from the middle height of the block
                        onTileDistance = otterHitBox.Right - HighestTile.HitBox.Left;
                        height = (float)(HighestTile.HitBox.Bottom - Math.Ceiling(onTileDistance / 2f) - HighestTile.Tile.Rectangle.Height/2);
                        break;
                    case TileType.DownhillHigh: //on the left
                        //downhill is 2x for 1y staring from the middle height of the block
                        onTileDistance = HighestTile.HitBox.Left - otterHitBox.Right;
                        height = (float)(HighestTile.HitBox.Bottom - Math.Ceiling(onTileDistance / 2f) - 50);
                        break;
                    case TileType.DownHillLow: //On the left
                        //downhill is 2x for 1y staring from the bottom of the block
                        onTileDistance = HighestTile.HitBox.Left - otterHitBox.Right;
                        height = (float)(HighestTile.HitBox.Bottom - Math.Ceiling(onTileDistance / 2f));
                        break;
                }
                
            }
            //check if a normal tile has teh same or a higher height           
            if (MainTileFilter.Count == 1) return height;
            for (int i = 1; i < MainTileFilter.Count; i++)
            {
                if (MainTileFilter[i].HitBox.Top < height)
                {
                    height = MainTileFilter[i].HitBox.Top;
                }
            }
            return height;
        }

        public static MapTile OtterTopHit(Rectangle otterHitBox, List<MapTile> maptiles)
        {            
            var MainTileFilter = maptiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(otterHitBox) && tile.HitBox.Bottom >= otterHitBox.Top && tile.HitBox.Bottom - otterHitBox.Top < 10).ToList();
            if (MainTileFilter.Count == 0) return null;
            return MainTileFilter.OrderByDescending(tile => tile.HitBox.Bottom).First();
        }
        
        public static MapTile OtterLeftHit(Rectangle otterHitBox, List<MapTile> mapTiles)
        {
            
            //check every tile if teh otter walks into it and ignores the ground tiles
            var mainTileFilter = mapTiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(otterHitBox) && tile.HitBox.Right >= otterHitBox.Left && tile.HitBox.Right - otterHitBox.Left < 20).ToList();
            if (mainTileFilter.Count == 0) return null;
            return mainTileFilter.OrderByDescending(tile => tile.HitBox.Right).First();
        }

        public static MapTile OtterRightHit(Rectangle otterHitBox, List<MapTile> mapTiles)
        {
            //checks every time if intersects with oterHitbox
            var mainTileFilter = mapTiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(otterHitBox) && tile.HitBox.Left <= otterHitBox.Right && otterHitBox.Right - tile.HitBox.Left < 20).ToList();
            if (mainTileFilter.Count == 0) return null;
            return mainTileFilter.OrderBy(tile => tile.HitBox.Left).First();
            
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