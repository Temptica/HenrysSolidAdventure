﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Graphics;
using ProjectPlatform.Shop;

namespace ProjectPlatform.Mapfolder
{
    //TODO: Make map scaled to screen with fixed raster of 50X50 for example
    //TODO: Read from a string (from folder)
    //
    public class Map
    {
        private static Map uMap; //unique key
        public static List<Tile> TileSet { get; private set; }
        public static Dictionary<string, Texture2D> DecorationTextures { get; private set; }
        internal List<MapTile> FrontMap { get; set; }
        internal List<MapTile> BackMap { get; set; }
        internal List<Coin> Coins { get; set; }
        internal List<Decoration> Decorations { get; set; }
        //internal float Scale { get; private set; }
        internal Store Shop { get; set; }
        internal Vector2 Spawn { get; set; }
        

        private Map()
        {

        }
        public static Map Instance => uMap ??= new Map(); //Singleton

        public void Initialise(ContentManager content)
        {
            if(TileSet is null || TileSet.Count == 0)
            {
                var tileSheet = content.Load<Texture2D>("Tiles/oak_woods_tileset_fixed");

                const int tileSize = 24;//from the pack https://brullov.itch.io/oak-woods
                var currentHeight = 0;
                var currentWidth = 0;
                //Scale = 1;
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

            if (DecorationTextures is not null && DecorationTextures.Count != 0) return;
            DecorationTextures = new Dictionary<string, Texture2D>();
            DecorationTextures.Add("fence_1", content.Load<Texture2D>("Decoration/fence_1"));
            DecorationTextures.Add("fence_2", content.Load<Texture2D>("Decoration/fence_2"));
            DecorationTextures.Add("grass_1", content.Load<Texture2D>("Decoration/grass_1"));
            DecorationTextures.Add("grass_2", content.Load<Texture2D>("Decoration/grass_2"));
            DecorationTextures.Add("grass_3", content.Load<Texture2D>("Decoration/grass_3"));
            DecorationTextures.Add("lamp", content.Load<Texture2D>("Decoration/lamp"));
            DecorationTextures.Add("rock_1", content.Load<Texture2D>("Decoration/rock_1"));
            DecorationTextures.Add("rock_2", content.Load<Texture2D>("Decoration/rock_2"));
            DecorationTextures.Add("rock_3", content.Load<Texture2D>("Decoration/rock_3"));
            DecorationTextures.Add("sign", content.Load<Texture2D>("Decoration/sign"));

        }
        public void Draw(Sprites spriteBatch)
        {
            BackMap?.ForEach(mapTile => mapTile.Draw(spriteBatch));
            FrontMap.ForEach(mapTile => mapTile.Draw(spriteBatch));
            Coins?.ForEach(coin =>coin.Draw(spriteBatch));
            Shop?.Draw(spriteBatch);
            Decorations?.ForEach(deco => deco.Draw(spriteBatch));
        }
        public void Update(GameTime gameTime)
        {
            Coins?.ForEach(coin => coin.Update(gameTime));
            var coinToDestroy = Coins?.Where(coin => coin.Destroy).FirstOrDefault();
            if (coinToDestroy != null) Coins?.Remove(coinToDestroy);
            Shop?.Update(gameTime);
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
        
        internal void Unload()
        {
            FrontMap??= null;
            BackMap??= null;
            Coins ??= null;
            Decorations ??= null;
            Shop ??= null;
            Spawn = Vector2.One;
        }
    }
}
