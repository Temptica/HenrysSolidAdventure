using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform.Mapfolder
{
    //TODO: Make map scaled to screen with fixed raster of 50X50 for example
    //TODO: Read from a string (from folder)
    //
    public class Map
    {
        private static Map uMap; //unique key
        public static List<Tile> TileSet { get; private set; }
        internal List<MapTile> FrontMap { get; set; }
        internal List<MapTile> BackMap { get; set; }
        internal float Scale { get; private set; }

        private Map()
        {

        }
        public static Map GetInstance() => uMap ??= new Map(); //Singleton

        public void Initialise(ContentManager content, float screenWidth)
        {
            var tileSheet = content.Load<Texture2D>("Tiles/oak_woods_tileset_fixed");
            const int tileSize = 24;//from the pack https://brullov.itch.io/oak-woods
            const float mapRaster = 50f; //have a raster of 50 tiles wide
            Scale = screenWidth / (mapRaster * tileSize);
            var currentHeight = 0;
            var currentWidth = 0;

            TileSet = new List<Tile>();
            while (currentHeight + tileSize <= tileSheet.Height && currentWidth + tileSize <= tileSheet.Width)
            {
                TileSet.Add(new Tile(tileSheet, new Rectangle(currentWidth, currentHeight, tileSize, tileSize)));
                currentWidth += tileSize;
                if (!(currentWidth >= tileSheet.Width)) continue;
                currentWidth = 0;
                currentHeight += tileSize;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            BackMap.ForEach(mapTile => mapTile.Draw(spriteBatch));
            FrontMap.ForEach(mapTile => mapTile.Draw(spriteBatch));
            

        }
        public Tile GetTile(int i)
        {
            if (TileSet is null) throw new NullReferenceException("Map is not initialized");
            if (TileSet.Count < i) throw new IndexOutOfRangeException($"{i} is greater than {TileSet.Count}");
            return TileSet[i];
        }

        private static bool IsFullyTransparent(Texture2D texture, Rectangle r) //from https://stackoverflow.com/questions/8372041/how-to-check-if-a-texture2d-is-transparent
        {
            int size = r.Width * r.Height;
            Color[] buffer = new Color[size];
            texture.GetData(0, r, buffer, 0, size);
            return buffer.All(c => c == Color.Transparent);
        }

    }
}
