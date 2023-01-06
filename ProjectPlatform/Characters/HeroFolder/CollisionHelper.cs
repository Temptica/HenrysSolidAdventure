using System;
using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Characters.Enemies;
using HenrySolidAdventure.MapFolder;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Characters.HeroFolder
{
    internal static class CollisionHelper
    {
        public static float GroundHit(Rectangle hitBox, List<MapTile> maptiles, bool isLookingLeft = false)
        {
            hitBox.Width /= 2;
            if (!isLookingLeft) hitBox.X += hitBox.Width;

            var filter1 = maptiles.Where(tile => tile.Tile.Type != TileType.Air && tile.HitBox.Intersects(hitBox))
                .ToList();
            var mainTileFilter = maptiles.Where(tile => tile.Tile.Type != TileType.Air && tile.HitBox.Intersects(hitBox) && (tile.HitBox.Top <= hitBox.Bottom && hitBox.Bottom - tile.HitBox.Top < 50 || tile.Tile.Type != TileType.Flat)).ToList(); // list of tiles that intersect with enemy (main hitbox based)
            if (mainTileFilter.Count == 0) return -1;
            var sorted = mainTileFilter.OrderByDescending(tile => tile.HitBox.Top).ToList();
            if (mainTileFilter.TrueForAll(tile => tile.Tile.Type is TileType.Flat or TileType.Air)) return sorted.First().Position.Y;//no slopes
            float height = float.MaxValue;
            foreach (var tile in sorted)
            {
                switch (tile.Tile.Type)
                {
                    case TileType.UphillLow: //on the right
                        var onTileDistance = Util.Clamp(hitBox.Right - tile.HitBox.Left, 0, tile.HitBox.Width);
                        int upslope = onTileDistance / 2;
                        //uphill is 2x for 1y staring from the bottom of the block
                        float newHeight = tile.HitBox.Bottom - upslope;
                        if (height > newHeight) height = tile.HitBox.Bottom - upslope;

                        break;
                    case TileType.UpHillHigh://on the right
                        //uphill is 2x for 1y staring from the middle height of the block
                        onTileDistance = Util.Clamp(hitBox.Right - tile.HitBox.Left, 0, tile.HitBox.Width);
                        upslope = onTileDistance / 2;
                        newHeight = tile.HitBox.Bottom - upslope - tile.Tile.Rectangle.Height / 2f;
                        if (height > newHeight) height = newHeight;
                        break;
                    case TileType.DownhillHigh: //on the left
                        //downhill is 2x for 1y staring from the top height of the block downwards to the middle
                        onTileDistance = Util.Clamp(tile.HitBox.Right - hitBox.Left, 0, tile.HitBox.Width);
                        upslope = onTileDistance / 2;
                        newHeight = tile.HitBox.Bottom - upslope - tile.Tile.Rectangle.Height / 2f;
                        if (height > newHeight) height = newHeight;

                        break;
                    case TileType.DownHillLow: //On the left
                        //downhill is 2x for 1y staring from the middle of the block downwards to the bottom
                        onTileDistance = Util.Clamp(tile.HitBox.Right - hitBox.Left, 0, tile.HitBox.Width);
                        upslope = onTileDistance / 2;
                        newHeight = tile.HitBox.Bottom - upslope;
                        if (height > newHeight) height = newHeight;
                        break;
                    case TileType.Flat:
                        if (height > tile.HitBox.Top) height = tile.HitBox.Top;
                        break;
                }
            }
            if (hitBox.Bottom < height) return -1;
            return height;
        }

        public static MapTile TopHit(Rectangle hitBox, List<MapTile> maptiles, bool isLookingLeft = false)
        {
            //if looking left, only take left half of hitbox, else right half
            var tempHitBox = new Rectangle(hitBox.X, hitBox.Y - 5, hitBox.Width, hitBox.Height);
            tempHitBox.Width /= 2;

            if (!isLookingLeft)
            {
                tempHitBox.X += hitBox.Width / 2;
            }
            var mainTileFilter = maptiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(tempHitBox) && tile.HitBox.Bottom - tempHitBox.Top < 10).ToList();
            if (mainTileFilter.Count == 0) return null;
            return mainTileFilter.OrderByDescending(tile => tile.HitBox.Bottom).First();
        }

        public static MapTile LeftHit(Rectangle hitBox, List<MapTile> mapTiles)
        {
            var tempHitBox = new Rectangle(hitBox.X, hitBox.Y + 10, hitBox.Width, hitBox.Height - 15);

            var mainTileFilter = mapTiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(tempHitBox) && tile.HitBox.Right >= tempHitBox.Left && tile.HitBox.Right - tempHitBox.Left < 20).ToList();
            if (mainTileFilter.Count == 0) return null;
            return mainTileFilter.OrderByDescending(tile => tile.HitBox.Right).First();
        }

        public static MapTile RightHit(Rectangle hitBox, List<MapTile> mapTiles)
        {
            //checks every time if intersects with oterHitbox
            var tempHitBox = new Rectangle(hitBox.X, hitBox.Y + 10, hitBox.Width, hitBox.Height - 15);
            var mainTileFilter = mapTiles.Where(tile => tile.Tile.Type == TileType.Flat && tile.HitBox.Intersects(tempHitBox) && tile.HitBox.Left <= tempHitBox.Right && tempHitBox.Right - tile.HitBox.Left < 20).ToList();
            if (mainTileFilter.Count == 0) return null;
            return mainTileFilter.OrderBy(tile => tile.HitBox.Left).First();

        }

        public static bool PixelBasedHit(Hero hero, Enemy enemy)
        {
            var otterPixels2D = GetCurrentPixels2D(hero);
            var enemyPixels2D = GetCurrentPixels2D(enemy);

            //get the rectangle wherein both collide
            var collisionRectangle = Rectangle.Intersect(hero.HitBox, enemy.HitBox);
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
                    var heroX = (int)(x - hero.GetPosition(hero.HitBox).X);
                    var heroY = (int)(y - hero.GetPosition(hero.HitBox).Y);
                    var enemyX = (int)(x - enemy.GetPosition(enemy.HitBox).X);
                    var enemyY = (int)(y - enemy.GetPosition(enemy.HitBox).Y);
                    try
                    {
                        if (otterPixels2D[heroX, heroY].A != 0 && enemyPixels2D[enemyX, enemyY].A != 0)
                        {
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

        private static Color[] GetCurrentPixels(Character character)
        {

            Color[] pixels = new Color[character.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Width * character.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Height];
            character.Animations.CurrentAnimation.CurrentFrame.Texture.GetData(0, character.Animations.CurrentAnimation.CurrentFrame.FrameRectangle, pixels, 0, pixels.Length);
            //gets the pixels of the current frame in a single array
            return pixels;
        }

        private static Color[,] GetCurrentPixels2D(Character character)
        {
            Color[] otterPixels = GetCurrentPixels(character);
            var width = character.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Width;
            var height = character.Animations.CurrentAnimation.CurrentFrame.FrameRectangle.Height;
            //convert otterPixels to 2d array to form a Rectangle
            Color[,] otterPixels2D = new Color[width, height];
            if (character.IsFacingLeft)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        otterPixels2D[x, y] = otterPixels[width - x - 1 + y * width];
                    }
                }
            }
            else
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        otterPixels2D[x, y] = otterPixels[x + y * width];
                    }
                }

            }

            return otterPixels2D;
        }
        public static bool LeavingLeftMapBorder(Rectangle hitBox, int x)
        {
            return hitBox.Left < x;
        }

        public static bool LeavingRightMapBorder(Rectangle hitBox, int x)
        {
            return hitBox.Right > x;
        }
        public static bool LeavingBottomMapBorder(Rectangle hitBox, int y)
        {
            return hitBox.Bottom > y;
        }

    }
}