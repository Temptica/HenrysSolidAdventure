using System;
using System.Collections.Generic;
using System.Linq;
using HenrySolidAdventure.Characters;
using HenrySolidAdventure.Characters.Enemies;
using HenrySolidAdventure.Characters.Enemies.Roaming;
using HenrySolidAdventure.Characters.Enemies.Tracking;
using HenrySolidAdventure.Characters.HeroFolder;
using HenrySolidAdventure.Characters.Traps;
using HenrySolidAdventure.Controller;
using HenrySolidAdventure.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HenrySolidAdventure.MapFolder
{
    internal static class MapLoader
    {
        private static float _mapOffset;
        public static int MapId = 1;
        private static readonly int MapCount = 5;
        private static ContentManager _content;
        public static void LoadMap(int screenheight, ContentManager content = null)
        {
            if (content != null)
            {
                _content = content;
            }
            //if unable to load, make sure to add the extended content pipeline builder dll in content.mgcb file under properties => references
            //this is required to load in Json files via the content builder.
            MapReaderObject mapFromFile = null;
            try
            {
                mapFromFile = _content.Load<MapReaderObject>($"Level/Level{MapId}");
                if (mapFromFile is null) throw new ArgumentNullException("file is empty or incorrect");
            }
            catch (ArgumentNullException)
            {
                return;
            }
            
            
            var map = Map.Instance;
            map.Unload();
            map.Enemies = new List<Enemy>();
            map.FrontMap = GenerateTileLayer(mapFromFile.Layers.First(layer => layer.Name == "ForegroundTiles"), screenheight);
            map.BackMap = GenerateTileLayer(mapFromFile.Layers.First(layer => layer.Name == "BackgroundTiles"), screenheight);
            map.Coins = GenerateCoins(mapFromFile.Layers.First(layer => layer.Name == "Coins").Objects);
            map.Decorations = new();
            GenerateDecorations(mapFromFile.Layers.First(layer => layer.Name == "Decoration").Objects, map.Decorations);
            GenerateDecorations(mapFromFile.Layers.First(layer => layer.Name == "Nature").Objects, map.Decorations);
            map.Shop = GenerateShop(mapFromFile.Layers.First(layer => layer.Name == "Shop").Objects.FirstOrDefault());
            var spawn = mapFromFile.Layers.First(layer => layer.Name == "Otter & enemies").Objects;
            SetSpawns(spawn, map);
            DiscordRichPresence.Instance.UpdateLevel();
        }
        
        private static void SetSpawns(Object[] spawns, Map map) //load in spawn+enemies+traps
        {
            if (spawns == null) throw new ArgumentNullException(nameof(spawns));
            foreach (var spawn in spawns)
            {
                switch (spawn.Class)
                {
                    case "Spawn":
                        map.Spawn = new Vector2(spawn.X, spawn.Y + _mapOffset - Hero.Instance.HitBox.Height);
                        break;
                    case "Bat":
                        map.Enemies.Add(new Bat(new Vector2(spawn.X, spawn.Y + _mapOffset-Bat.Texture.Height)));
                        break;
                    case "Skeleton":
                        map.Enemies.Add(new Skeleton(new Vector2(spawn.X, spawn.Y + _mapOffset - Skeleton.Textures[State.Idle].Height)));
                        break;
                    case "Slime":
                        map.Enemies.Add(new Slime(new Vector2(spawn.X, spawn.Y + _mapOffset - Slime.Texture.Height/3f)));
                        break;
                    case "Portal":
                        map.Portal = new Portal(new Vector2(spawn.X, spawn.Y + _mapOffset - Portal.Texture.Height));
                        break;
                    case "Boss" when !map.Enemies.Any(e => e is Boss):
                        map.Enemies.Add(new Boss(new Vector2(spawn.X, spawn.Y + _mapOffset - Boss.Texture.Height/8f))); //if boss exists, do not add
                        break;
                    case "Fire1":
                        map.Enemies.Add(new Fire(new Vector2(spawn.X, spawn.Y + _mapOffset),TrapTier.One));
                        break;
                    case "Fire2":
                        map.Enemies.Add(new Fire(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Two));
                        break;
                    case "Fire3":
                        map.Enemies.Add(new Fire(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Three));
                        break;
                    case "Saw1":
                        map.Enemies.Add(new Saw(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.One));
                        break;
                    case "Saw2":
                        map.Enemies.Add(new Saw(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Two));
                        break;
                    case "Saw3":
                        map.Enemies.Add(new Saw(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Three));
                        break;
                    case "Lightning1":
                        map.Enemies.Add(new Lightning(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.One));
                        break;
                    case "Lightning2":
                        map.Enemies.Add(new Lightning(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Two));
                        break;
                    case "Lightning3":
                        map.Enemies.Add(new Lightning(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Three));
                        break;
                    case "Ceiling1":
                        map.Enemies.Add(new Ceiling(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.One));
                        break;
                    case "Ceiling2":
                        map.Enemies.Add(new Ceiling(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Two));
                        break;
                    case "Ceiling3":
                        map.Enemies.Add(new Ceiling(new Vector2(spawn.X, spawn.Y + _mapOffset), TrapTier.Three));
                        break;
                }
            }
        }

        private static void GenerateDecorations(IEnumerable<Object> objects, ICollection<Decoration> decorations)
        {
            foreach (var obj in objects)
            {
                var texture = Map.DecorationTextures.GetValueOrDefault(obj.Class);
                decorations.Add(new Decoration(texture, new Vector2(obj.X,obj.Y+ _mapOffset - texture.Height), 1));
            }
        }

        private static Store GenerateShop(Object store)
        {
            if (store is null) return null;
            return new Store(new Vector2(store.X, store.Y+ _mapOffset - Store.Texture.Height));
        }

        private static List<MapTile> GenerateTileLayer(Layer layer, int height)
        {
            List<MapTile> map = new();

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    var data = layer.Data[x + y * layer.Height];
                    if (data == 0)
                    {
                        data = 5; //make air
                    }
                    var tile = Map.TileSet[data-1];
                    map.Add(new MapTile(tile, new Vector2(x * tile.Rectangle.Width, (y * tile.Rectangle.Height)), new Vector2(x, y)));
                }
            }
            
            var mapHeight = map.Max(tile => tile.Position.Y) + Map.TileSet[0].Rectangle.Height;
            _mapOffset = height - mapHeight;
            foreach (var tile in map)
            {
                tile.Position += new Vector2(0, _mapOffset);
            }
            return map;
            
        }        
        private static List<Coin> GenerateCoins(Object[] coins)
        {
            return coins.Select(coin => new Coin(new Vector2(coin.X-4, coin.Y + _mapOffset -28))).ToList();
        }
        public static void LoadNextMap(int screenHeight)
        {
            if (MapId == MapCount) return;
            MapId++;
            LoadMap(screenHeight);
        }
        public static void SetMapId(int mapId)
        {
            MapId = mapId;
        }
        public static void ReloadEnemies()
        {
            var remove = new List<Enemy>();
            var boss = Map.Instance.Enemies.First(enemy => enemy is Boss);

            Map.Instance.Enemies = new List<Enemy>()
            {
                boss
            };
            var mapFromFile = _content.Load<MapReaderObject>($"Level/Level{MapId}");
            SetSpawns(mapFromFile.Layers.First(layer => layer.Name == "Otter & enemies").Objects, Map.Instance);
        }
    }
}
