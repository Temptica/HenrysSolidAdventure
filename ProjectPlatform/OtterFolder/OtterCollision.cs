using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ProjectPlatform.EnemyFolder;
using ProjectPlatform.Mapfolder;

namespace ProjectPlatform.OtterFolder
{
    internal static class OtterCollision
    {
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
            var MainTileFilter = maptiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(otterHitBox) && tile.HitBox.Bottom >= otterHitBox.Top && tile.HitBox.Bottom - otterHitBox.Top < 10).ToList();
            if (MainTileFilter.Count == 0) return null;
            return MainTileFilter.OrderByDescending(tile => tile.HitBox.Bottom).First();
        }

        public static MapTile OtterLeftHit(Rectangle otterHitBox, List<MapTile> mapTiles)
        {

            //check every tile if teh enemy walks into it and ignores the ground tiles
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

        public static bool PixelBasedHit(OtterFolder.Otter otter, Enemy enemy)
        {
            Color[,] otterPixels2D = GetCurrentPixels2D(otter);
            Color[,] enemyPixels2D = GetCurrentPixels2D(enemy);
            //get the rectangle wherein both collide
            Rectangle collisionRectangle = Rectangle.Intersect(otter.HitBox, enemy.HitBox);
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
                    try
                    {
                        if (otterPixels2D[(int)Math.Abs(x - otter.Position.X), (int)Math.Abs(y - otter.Position.Y)].A !=
                            0 && enemyPixels2D[(int)Math.Abs(x - enemy.Position.X),
                                (int)Math.Abs(y - enemy.Position.Y - 1)].A != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
            return false;

        }

        private static Color[] GetCurrentPixels(OtterFolder.Otter otter)
        {
            Color[] pixels = new Color[otter.CurrentAnimation.CurrentFrame.FrameRectangle.Width * otter.CurrentAnimation.CurrentFrame.FrameRectangle.Height];
            otter.CurrentAnimation.Texture.GetData(0, otter.CurrentAnimation.CurrentFrame.FrameRectangle, pixels, 0, pixels.Length);

            return pixels;
        }
        private static Color[] GetCurrentPixels(Enemy enemy)
        {
            Color[] pixels = new Color[enemy.CurrentAnimation.CurrentFrame.FrameRectangle.Width * enemy.CurrentAnimation.CurrentFrame.FrameRectangle.Height];

            enemy.CurrentAnimation.Texture.GetData(0, enemy.CurrentAnimation.CurrentFrame.FrameRectangle, pixels, 0, pixels.Length);

            return pixels;
        }

        private static Color[,] GetCurrentPixels2D(OtterFolder.Otter otter)
        {
            Color[] otterPixels = GetCurrentPixels(otter);
            var width = otter.CurrentAnimation.CurrentFrame.FrameRectangle.Width;
            var height = otter.CurrentAnimation.CurrentFrame.FrameRectangle.Height;
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
        private static Color[,] GetCurrentPixels2D(Enemy enemy)
        {
            Color[] otterPixels = GetCurrentPixels(enemy);
            //convert otterPixels to 2d array to from a Rectangle
            var width = enemy.CurrentAnimation.CurrentFrame.FrameRectangle.Width;
            var height = enemy.CurrentAnimation.CurrentFrame.FrameRectangle.Height;
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
    }
}