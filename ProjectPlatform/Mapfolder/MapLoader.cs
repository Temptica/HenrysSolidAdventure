using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPlatform.Mapfolder
{
    internal static class MapLoader
    {
        //"D:\ap\22-23\ProjGameDev\Map\Level1.json"
        public static void LoadMap(string location, int screenheight)
        {
            var mapFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<MapReaderObject>(System.IO.File.ReadAllText(location));
            var map = Map.GetInstance();
            map.FrontMap =  GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "ForegroundTiles"), screenheight);
            map.BackMap = GenerateTileLayer(mapFromFile.layers.First(layer => layer.name == "BackgroundTiles"), screenheight);
            
        }

        private static List<MapTile> GenerateTileLayer(Layer layer, int height)
        {
            List<MapTile> map = new();
            var mapScale = Map.GetInstance().Scale;
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
    }
}
