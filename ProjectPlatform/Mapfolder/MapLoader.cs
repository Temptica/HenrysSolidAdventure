using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OtterlyAdventure.EnemyFolder;
using OtterlyAdventure.OtterFolder;
using OtterlyAdventure.Shop;

namespace OtterlyAdventure.Mapfolder
{
    internal static class MapLoader
    {
        //"D:\ap\22-23\ProjGameDev\Map\Level1.json"
        private static float mapOffset;
        private static int MapID = 1;
        private static int mapCount = 3;
        static ContentManager _content;
        public static void LoadMap(int screenheight, ContentManager content = null)
        {
            if (content != null)
            {
                _content = content;
            }
            
            MapReaderObject mapFromFile = null;
            try
            {
                mapFromFile = _content.Load<MapReaderObject>($"Level/Level{MapID}");//ProjectPlatform.Mapfolder.MapReaderObject, ProjectPlatform
                if (mapFromFile is null) throw new ArgumentNullException("file is empty or incorrect");
            }
            catch (ArgumentNullException)
            {
                return;
            }
            
            
            var map = Map.Instance;
            map.Unload();
            map.FrontMap =  GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "ForegroundTiles"), screenheight);
            map.BackMap = GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "BackgroundTiles"), screenheight);
            map.Coins = GenerateCoins(mapFromFile.layers.First(layer => layer.name == "Coins").objects);
            map.Decorations = new();
            GenerateDecorations(mapFromFile.layers.First(layer => layer.name == "Decoration").objects, map.Decorations);
            GenerateDecorations(mapFromFile.layers.First(layer => layer.name == "Nature").objects, map.Decorations);
            map.Shop = GenerateShop(mapFromFile.layers.First(layer => layer.name == "Shop").objects.First());
            var spawn = mapFromFile.layers.First(layer => layer.name == "Otter & enemies").objects;
            SetSpawns(spawn, map);
        }

        private static void SetSpawns(Object[] spawns, Map map)
        {
            map.Enemies = new List<Enemy>();
            foreach (var spawn in spawns)
            {
                if(spawn._class == "Spawn") map.Spawn = new Vector2(spawn.x, spawn.y + mapOffset - Otter.Texture.Height);
                if (spawn._class == "Bat") map.Enemies.Add(new Bat(new Vector2(spawn.x, spawn.y + mapOffset-Bat.Texture.Height)));
                if (spawn._class == "Skeleton") map.Enemies.Add(new Skeleton(new Vector2(spawn.x, spawn.y + mapOffset - Skeleton.Textures[State.Idle].Height)));
                if (spawn._class == "Slime") map.Enemies.Add(new Slime(new Vector2(spawn.x, spawn.y + mapOffset - Slime.Texture.Height/3f)));
            }           
        }

        private static void GenerateDecorations(IEnumerable<Object> objects, ICollection<Decoration> decorations)
        {
            foreach (var obj in objects)
            {
                var texture = Map.DecorationTextures.GetValueOrDefault(obj._class);
                decorations.Add(new Decoration(texture, new Vector2(obj.x,obj.y+ mapOffset - texture.Height), 1));
            }
        }

        private static Store GenerateShop(Object store)
        {
            return new Store(new Vector2(store.x, store.y+ mapOffset - Store.Texture.Height));
        }

        private static List<MapTile> GenerateTileLayer(Layer layer, int height)
        {
            List<MapTile> map = new();

            for (int y = 0; y < layer.height; y++)
            {
                for (int x = 0; x < layer.width; x++)
                {
                    var data = layer.data[x + y * layer.height];
                    if (data == 0)
                    {
                        data = 5; //make air
                    }
                    var tile = Map.TileSet[data-1];
                    map.Add(new MapTile(tile, new Vector2(x * tile.Rectangle.Width, (y * tile.Rectangle.Height)), new Vector2(x, y)));
                }
            }
            
            var mapHeight = map.Max(tile => tile.Position.Y) + Map.TileSet[0].Rectangle.Height;
            mapOffset = height - mapHeight;
            foreach (var tile in map)
            {
                tile.Position += new Vector2(0, mapOffset);
            }
            return map;
            
        }        
        private static List<Coin> GenerateCoins(Object[] coins)
        {
            return coins.Select(coin => new Coin(new Vector2(coin.x-4, coin.y + mapOffset -28))).ToList();
        }

        public static void LoadPreviousMap(int screenHeight)
        {
            if (MapID == 1) return;
            MapID--;
            LoadMap(screenHeight);
        }

        public static void LoadNextMap(int screenHeight)
        {
            if (MapID == mapCount) return;
            MapID++;
            LoadMap(screenHeight);
        }
    }
}
