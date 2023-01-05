using System;
using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Characters.Traps;
using HenrySolidAdventure.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HenrySolidAdventure.Mapfolder
{
    internal static class MapLoader
    {
        //"D:\ap\22-23\ProjGameDev\Map\Level1.json"
        private static float mapOffset;
        private static int MapID = 1;
        private static readonly int mapCount = 5;
        private static ContentManager _content;
        public static void LoadMap(int screenheight, ContentManager content = null)
        {
            if (content != null)
            {
                _content = content;
            }
            
            MapReaderObject mapFromFile = null;
            try
            {
                mapFromFile = _content.Load<MapReaderObject>($"Level/Level{MapID}");//HenrySolidAdventure.Mapfolder.MapReader, HenrySolidAdventure
                if (mapFromFile is null) throw new ArgumentNullException("file is empty or incorrect");
            }
            catch (ArgumentNullException)
            {
                return;
            }
            
            
            var map = Map.Instance;
            map.Unload();
            map.Enemies = new List<Enemy>();
            map.FrontMap = GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "ForegroundTiles"), screenheight);
            map.BackMap = GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "BackgroundTiles"), screenheight);
            map.Coins = GenerateCoins(mapFromFile.layers.First(layer => layer.name == "Coins").objects);
            map.Decorations = new();
            GenerateDecorations(mapFromFile.layers.First(layer => layer.name == "Decoration").objects, map.Decorations);
            GenerateDecorations(mapFromFile.layers.First(layer => layer.name == "Nature").objects, map.Decorations);
            var shop = GenerateShop(mapFromFile.layers.First(layer => layer.name == "Shop").objects.FirstOrDefault());
            map.Shop = shop == default ? null : shop;
            var spawn = mapFromFile.layers.First(layer => layer.name == "Otter & enemies").objects;
            SetSpawns(spawn, map);
        }
        
        private static void SetSpawns(Object[] spawns, Map map)
        {
            foreach (var spawn in spawns)
            {
                if(spawn._class == "Spawn") map.Spawn = new Vector2(spawn.x, spawn.y + mapOffset - Hero.Instance.HitBox.Height);
                if (spawn._class == "Bat") map.Enemies.Add(new Bat(new Vector2(spawn.x, spawn.y + mapOffset-Bat.Texture.Height)));
                if (spawn._class == "Skeleton") map.Enemies.Add(new Skeleton(new Vector2(spawn.x, spawn.y + mapOffset - Skeleton.Textures[State.Idle].Height)));
                if (spawn._class == "Slime") map.Enemies.Add(new Slime(new Vector2(spawn.x, spawn.y + mapOffset - Slime.Texture.Height/3f)));
                if (spawn._class == "Portal") map.Portal = new Portal(new Vector2(spawn.x, spawn.y + mapOffset - Portal.Texture.Height));
                if (spawn._class == "Boss" && !map.Enemies.Any(e => e is Boss)) map.Enemies.Add(new Boss(new Vector2(spawn.x, spawn.y + mapOffset - Boss.Texture.Height/8f))); //if boss exists, do not add
                if (spawn._class == "Fire1") map.Enemies.Add(new Fire(new Vector2(spawn.x, spawn.y + mapOffset),TrapTier.One));
                if (spawn._class == "Fire2") map.Enemies.Add(new Fire(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Two));
                if (spawn._class == "Fire3") map.Enemies.Add(new Fire(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Three));
                if (spawn._class == "Saw1") map.Enemies.Add(new Saw(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.One));
                if (spawn._class == "Saw2") map.Enemies.Add(new Saw(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Two));
                if (spawn._class == "Saw3") map.Enemies.Add(new Saw(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Three));
                if (spawn._class == "Lightning1") map.Enemies.Add(new Lightning(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.One));
                if (spawn._class == "Lightning2") map.Enemies.Add(new Lightning(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Two));
                if (spawn._class == "Lightning3") map.Enemies.Add(new Lightning(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Three));
                if (spawn._class == "Ceiling1") map.Enemies.Add(new Ceiling(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.One));
                if (spawn._class == "Ceiling2") map.Enemies.Add(new Ceiling(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Two));
                if (spawn._class == "Ceiling3") map.Enemies.Add(new Ceiling(new Vector2(spawn.x, spawn.y + mapOffset), TrapTier.Three));
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
            if (store is null) return null;
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

        public static void SetMapId(int mapId)
        {
            MapID = mapId;
        }

        public static void ReloadEnemies()
        {
            var remove = new List<Enemy>();
            var boss = Map.Instance.Enemies.First(enemy => enemy is Boss);

            Map.Instance.Enemies = new List<Enemy>()
            {
                boss
            };
            MapReaderObject mapFromFile = _content.Load<MapReaderObject>($"Level/Level{MapID}");
            SetSpawns(mapFromFile.layers.First(layer => layer.name == "Otter & enemies").objects, Map.Instance);
        }
    }
}
