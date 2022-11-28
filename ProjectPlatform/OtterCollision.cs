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
            var sorted = MainTileFilter.OrderByDescending(tile => tile.HitBox.Top).ToList();
            if (MainTileFilter.TrueForAll(tile => tile.Tile.Type is TileType.Flat or TileType.Air)) return sorted.First().Position.Y;//no slopes
            float height = float.MaxValue;
            foreach (var tile in sorted)
            {
                switch (tile.Tile.Type)
                {
                    case TileType.UphillLow: //on the right
                        var onTileDistance = Util.Clamp(otterHitBox.Right - tile.HitBox.Left, 0, tile.HitBox.Width);
                        int upslope = onTileDistance / 2;
                        //uphill is 2x for 1y staring from the bottom of the block
                        float newHeight = tile.HitBox.Bottom - upslope;
                        if (height > newHeight) height = tile.HitBox.Bottom - upslope;

                        break;
                    case TileType.UpHillHigh://on the right
                        //uphill is 2x for 1y staring from the middle height of the block
                        onTileDistance = Util.Clamp(otterHitBox.Right - tile.HitBox.Left, 0, tile.HitBox.Width);
                        upslope = onTileDistance / 2;
                        newHeight = tile.HitBox.Bottom - upslope - tile.Tile.Rectangle.Height / 2f;
                        if (height > newHeight) height = newHeight;
                        break;
                    case TileType.DownhillHigh: //on the left
                        //downhill is 2x for 1y staring from the top height of the block downwards to the middle
                        onTileDistance = Util.Clamp(tile.HitBox.Right - otterHitBox.Left , 0, tile.HitBox.Width);
                        upslope = onTileDistance / 2;
                        newHeight = tile.HitBox.Bottom - upslope - tile.Tile.Rectangle.Height / 2f;
                        if (height > newHeight) height = newHeight;

                        break;
                    case TileType.DownHillLow: //On the left
                        //downhill is 2x for 1y staring from the middle of the block downwards to the bottom
                        onTileDistance = Util.Clamp(tile.HitBox.Right - otterHitBox.Left, 0, tile.HitBox.Width);
                        upslope = onTileDistance / 2;
                        newHeight = tile.HitBox.Bottom - upslope;
                        if (height > newHeight) height = newHeight;
                        break;
                    case TileType.Flat:
                        if(height> tile.HitBox.Top) height = tile.HitBox.Top;
                        break;
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
            Otter.Texture.GetData(0, new Rectangle(otter.CurrentSpriteX, 0,200, 200), otterPixels, 0, otterPixels.Length);

            return otterPixels;
        }

        private static Color[,] GetCurrentPixels2D(Otter otter)
        {
            Color[] otterPixels = GetCurrentPixels(otter);
            //convert otterPixels to 2d array to from a Rectangle
            Color[,] otterPixels2D = new Color[Otter.Texture.Width, Otter.Texture.Height];
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