using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.MapFolder
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

    }
}
