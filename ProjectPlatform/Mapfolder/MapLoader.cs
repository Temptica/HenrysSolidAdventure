using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectPlatform.Shop;

namespace ProjectPlatform.Mapfolder
{
    internal static class MapLoader
    {
        //"D:\ap\22-23\ProjGameDev\Map\Level1.json"
        static float xScale;
        static float yScale;
        public static void LoadMap(string location, int screenheight)
        {
            var mapFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<MapReaderObject>(System.IO.File.ReadAllText(location));
            var map = Map.Instance;
            map.Unload();
            map.FrontMap =  GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "ForegroundTiles"), screenheight);
            map.BackMap = GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "BackgroundTiles"), screenheight);
            map.Coins = GenerateCoins(mapFromFile.layers.First(layer => layer.name == "Coins").objects, screenheight);
            map.Decorations = new();
            GenerateDecorations(mapFromFile.layers.First(layer => layer.name == "Decoration").objects, screenheight, map.Decorations);
            GenerateDecorations(mapFromFile.layers.First(layer => layer.name == "Nature").objects, screenheight, map.Decorations);
            map.Shop = GenerateShop(mapFromFile.layers.First(layer => layer.name == "Shop").objects.First(), screenheight);
        }

        private static void GenerateDecorations(Object[] objects, int screenheight, List<Decoration> decorations)
        {
            var mapHeight = Map.Instance.FrontMap.Max(tile => tile.Position.Y) + Map.Instance.Scale * Map.TileSet[0].Rectangle.Height;
            var mapOffset = screenheight - mapHeight;
            foreach (var obj in objects)
            {
                decorations.Add(new Decoration(Map.DecorationTextures.GetValueOrDefault(obj._class), (new Vector2(obj.x/24*xScale,(int)(obj.y/24))), Map.Instance.Scale));
            }
            //put decoration on the correct position (scaled by the map);
        }

        private static Store GenerateShop(Object store, int screenheight)
        {
            return new Store(new Vector2(store.x / 24 * xScale, store.y/24/yScale), Map.Instance.Scale);
        }

        private static List<MapTile> GenerateTileLayer(Layer layer, int height)
        {
            List<MapTile> map = new();
            var mapScale = Map.Instance.Scale;
            int[,] mapLayer = new int[layer.width, layer.height];
            
            for (int y = 0; y < layer.height; y++)
            {
                for (int x = 0; x < layer.width; x++)
                {
                    var data = layer.data[x + y * layer.height];
                    if (data == 0)
                    {
                        continue;
                    }
                    var tile = Map.TileSet[data-1];
                    xScale = tile.Rectangle.Width * mapScale;
                    yScale = tile.Rectangle.Height * mapScale;
                    map.Add(new MapTile(tile, new Vector2(x * tile.Rectangle.Width* mapScale, (y * tile.Rectangle.Height* mapScale))));
                }
            }
            
            var mapHeight = map.Max(tile => tile.Position.Y) + mapScale * Map.TileSet[0].Rectangle.Height;
            var mapOffset = height - mapHeight;
            foreach (var tile in map)
            {
                tile.Position += new Vector2(0, mapOffset);
            }
            return map;
            
        }        
        private static List<Coin> GenerateCoins(Object[] coins, int height)
        {
            var CoinList = coins.Select(coin => new Coin(new Vector2((int)coin.x / 24 * xScale, (int)coin.y))).ToList();
            //var mapHeight = Map.Instance().FrontMap.Max(tile => tile.Position.Y) + Map.Instance().Scale * Map.TileSet[0].Rectangle.Height;
            //var mapOffset = height - mapHeight;

            //foreach (var coin in CoinList)
            //{
            //    var coinPosition = new Vector2(coin.HitBox.X, coin.HitBox.Y);
            //    coinPosition += new Vector2(0, mapOffset);
            //    coin.HitBox = new Rectangle((int)(coinPosition.X/ Map.Instance().Scale), (int)(coinPosition.Y / Map.Instance().Scale), coin.HitBox.Width, coin.HitBox.Height);
            //}

            //scale the position to be on the correct place on the map
            return CoinList;
        }
    }
}
