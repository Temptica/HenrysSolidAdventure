using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using OtterlyAdventure.Characters;
using OtterlyAdventure.Mapfolder;

namespace OtterlyAdventure.OtterFolder
{
    internal static class CollisionHelper
    {
        public static Rectangle LastHit = new(1,1,1,1);
        public static float OtterGroundHit(Rectangle otterHitBox, List<MapTile> maptiles)
        {
            var MainTileFilter = maptiles.Where(tile => tile.Tile.Type != TileType.Air && tile.HitBox.Intersects(otterHitBox) && (tile.HitBox.Top <= otterHitBox.Bottom && otterHitBox.Bottom - tile.HitBox.Top < 50 || tile.Tile.Type != TileType.Flat)).ToList(); // list of tiles that intersect with enemy (main hitbox based)
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
                        onTileDistance = Util.Clamp(tile.HitBox.Right - otterHitBox.Left, 0, tile.HitBox.Width);
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
                        if (height > tile.HitBox.Top) height = tile.HitBox.Top;
                        break;
                }
            }
            if (otterHitBox.Bottom < height) return -1;
            return height;
        }

        public static MapTile OtterTopHit(Rectangle otterHitBox, List<MapTile> maptiles)
        {
            otterHitBox = new Rectangle(otterHitBox.X, otterHitBox.Y, otterHitBox.Width, otterHitBox.Height);
            var MainTileFilter = maptiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(otterHitBox) && tile.HitBox.Bottom - otterHitBox.Top < 10).ToList();
            if (MainTileFilter.Count == 0) return null;
            return MainTileFilter.OrderByDescending(tile => tile.HitBox.Bottom).First();
        }

        public static MapTile OtterLeftHit(Rectangle otterHitBox, List<MapTile> mapTiles)
        {
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
            var otterPixels2D = GetCurrentPixels2D(otter);
            var enemyPixels2D = GetCurrentPixels2D(enemy);
            //get the rectangle wherein both collide
            var collisionRectangle = Rectangle.Intersect(otter.HitBox, enemy.HitBox);
            if (collisionRectangle.Width == 0 || collisionRectangle.Height == 0) return false;
            //get the start and end points of the collision rectangle
            int startX = collisionRectangle.Left;
            int startY = collisionRectangle.Top;
            int endX = startX + collisionRectangle.Width;
            int endY = startY + collisionRectangle.Height;
            //check every pixel in the collision rectangle
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    //if both pixels are not transparent, there is a collision
                    var otterx = x - otter.HitBox.Left;
                    var ottery = y - otter.HitBox.Top;
                    var enemyx = x - enemy.HitBox.Left;
                    var enemyy = y - enemy.HitBox.Top;
                    try
                    {
                        if (otterPixels2D[otterx,ottery].A != 0 && enemyPixels2D[enemyx,enemyy].A != 0)
                        {
                            LastHit = collisionRectangle;
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return false;

        }

        private static Color[] GetCurrentPixels(Character animatable)
        {
            
            Color[] pixels = new Color[animatable.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Width * animatable.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Height];
            animatable.Animations.CurrentAnimation.Texture.GetData(0, animatable.Animations.CurrentAnimation.CurrentFrame.FrameRectangle, pixels, 0, pixels.Length);

            return pixels;
        }

        private static Color[,] GetCurrentPixels2D(Character animatable)
        {
            Color[] otterPixels = GetCurrentPixels(animatable);
            var width = animatable.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Width;
            var height = animatable.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Height;
            //convert otterPixels to 2d array to from a Rectangle
            Color[,] otterPixels2D = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    otterPixels2D[x, y] = otterPixels[x + y * width];
                }
            }

            return otterPixels2D;
        }
        public static bool LeavingLeftMapBorder(Rectangle otter, int x)
        {
            return otter.Left < x;
        }

        public static bool LeavingRightMapBorder(Rectangle otter, int x)
        {
            return otter.Right > x;
        }
        public static bool LeavingBottomMapBorder(Rectangle otter, int y)
        {
            return otter.Bottom > y;
        }

    }
}