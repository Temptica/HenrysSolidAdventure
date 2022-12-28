using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.Graphics;

namespace OtterlyAdventure.Mapfolder
{
    internal class MapTile
    {
        public Tile Tile { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 GridPosition { get; set; }
        public Rectangle HitBox => new((int)Position.X, (int)Position.Y, Tile.Rectangle.Width, Tile.Rectangle.Height);

        public MapTile(Tile tile, Vector2 position, Vector2 gridPosition)
        {
            Tile = tile;
            Position = position;
            GridPosition = gridPosition;
        }
        public void Draw(Sprites spriteBatch)
        {
            spriteBatch.Draw(Tile.Texture, Position, Tile.Rectangle, Color.White, 0f,
                Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        //check if send in rectangle hits the top of the tile with it's bottom
        public bool IsTopHit(Rectangle rectangle)
        {
            return rectangle.Bottom >= HitBox.Top && rectangle.Bottom <= HitBox.Bottom &&
                   rectangle.Right >= HitBox.Left && rectangle.Left <= HitBox.Right;
        }
        //do the same for the bottom of the tile and the top of the rectangle
        public bool IsBottomHit(Rectangle rectangle)
        {
            return rectangle.Top <= HitBox.Bottom && rectangle.Top >= HitBox.Top &&
                   rectangle.Right >= HitBox.Left && rectangle.Left <= HitBox.Right;
        }
        //same for left of tile and right of rectangle
        public bool IsLeftHit(Rectangle rectangle)
        {
            return rectangle.Right >= HitBox.Left && rectangle.Right <= HitBox.Right &&
                   rectangle.Bottom >= HitBox.Top && rectangle.Top <= HitBox.Bottom;
        }
        //same for right of tile and left of rectangle
        public bool IsRightHit(Rectangle rectangle)
        {
            return rectangle.Left <= HitBox.Right && rectangle.Left >= HitBox.Left &&
                   rectangle.Bottom >= HitBox.Top && rectangle.Top <= HitBox.Bottom;
        }
        public static MapTile GetClosestAirTile(Vector2 Position)
        {
            {
                var list = new List<MapTile>();
                foreach (var item in Map.Instance.FrontMap)
                {
                    if (item.Tile.Type is TileType.Air)
                    {
                        list.Add(item);
                    }
                }
                var closest = list.OrderBy(x => Vector2.Distance(Position, x.Position)).First();
                return closest;
            }
        }
    }
}
