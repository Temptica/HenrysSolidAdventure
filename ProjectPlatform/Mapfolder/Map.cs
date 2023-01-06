using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HenrySolidAdventure.Characters.Enemies;
using HenrySolidAdventure.Graphics;
using HenrySolidAdventure.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HenrySolidAdventure.MapFolder
{
    //TODO: Make map scaled to screen with fixed raster of 50X50 for example
    //TODO: Read from a string (from folder)
    //
    public class Map
    {
        private static Map _uMap; //unique key
        public static List<Tile> TileSet { get; private set; }
        public static Dictionary<string, Texture2D> DecorationTextures { get; private set; }
        internal List<MapTile> FrontMap { get; set; }
        internal List<MapTile> BackMap { get; set; }
        internal List<Coin> Coins { get; set; }
        internal List<Decoration> Decorations { get; set; }
        //internal float Scale { get; private set; }
        internal Store Shop { get; set; }
        internal Vector2 Spawn { get; set; }
        internal Rectangle ScreenRectangle { get; private set; }
        internal List<Enemy> Enemies { get; set; }
        internal Portal Portal { get; set; }


        private Map()
        {

        }
        public static Map Instance => _uMap ??= new Map(); //Singleton

        public void Initialise(ContentManager content, Screen screen)
        {
            if(TileSet is null || TileSet.Count == 0)
            {
                var tileSheet = content.Load<Texture2D>("Tiles/oak_woods_tileset_fixed");

                const int tileSize = 24;//from the pack https://brullov.itch.io/oak-woods
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
            ScreenRectangle = new Rectangle(0, 0, screen.Width, screen.Height);
            if (DecorationTextures is not null && DecorationTextures.Count != 0) return;
            DecorationTextures = new Dictionary<string, Texture2D>
            {
                { "fence_1", content.Load<Texture2D>("Decoration/fence_1") },
                { "fence_2", content.Load<Texture2D>("Decoration/fence_2") },
                { "grass_1", content.Load<Texture2D>("Decoration/grass_1") },
                { "grass_2", content.Load<Texture2D>("Decoration/grass_2") },
                { "grass_3", content.Load<Texture2D>("Decoration/grass_3") },
                { "lamp", content.Load<Texture2D>("Decoration/lamp") },
                { "rock_1", content.Load<Texture2D>("Decoration/rock_1") },
                { "rock_2", content.Load<Texture2D>("Decoration/rock_2") },
                { "rock_3", content.Load<Texture2D>("Decoration/rock_3") },
                { "sign", content.Load<Texture2D>("Decoration/sign") }
            };
            Coin.Texture = content.Load<Texture2D>("Items/Coin");
            Store.Texture = content.Load<Texture2D>("Decoration/shop_anim");
            Portal.Texture = content.Load<Texture2D>("Items/PortalRings2");
            EnemyInitialiser.LoadTextures(content);

        }
        public void Draw(Sprites sprites, SpriteBatch spriteBatch)
        {
            BackMap?.ForEach(mapTile => mapTile.Draw(sprites));
            FrontMap.ForEach(mapTile => mapTile.Draw(sprites));
            Coins?.ForEach(coin =>coin.Draw(sprites));
            Decorations?.ForEach(deco => deco.Draw(sprites));
            Enemies?.ForEach(enemy => enemy.Draw(sprites, spriteBatch));
            Portal?.Draw(sprites);
            Shop?.Draw(sprites, spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            Coins?.ForEach(coin => coin.Update(gameTime));
            var coinToDestroy = Coins?.Where(coin => coin.Destroy).FirstOrDefault();
            if (coinToDestroy != null) Coins?.Remove(coinToDestroy);
            Shop?.Update(gameTime);
            try
            {
                Enemies?.ForEach(enemy => enemy.Update(gameTime));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            var remove = Enemies?.Where(enemy => enemy.Remove).ToList();
            if (remove != null)
            {
                foreach (var enemy in remove)
                {
                    Enemies.Remove(enemy);
                }
            }
            if (Portal is null) return;
            if (Portal.Update(gameTime))
            {
                MapLoader.LoadNextMap(ScreenRectangle.Height);
            }
        }
        
        internal void Unload()
        {
            FrontMap = null;
            BackMap = null;
            Coins  = null;
            Decorations  = null;
            Shop = null;
            Spawn = Vector2.One;
            Enemies = null;
            Portal = null;
        }
    }
}
