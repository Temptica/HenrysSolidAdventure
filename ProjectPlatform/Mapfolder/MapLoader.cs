using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.Mapfolder
{
    internal static class MapLoader
    {
        //"D:\ap\22-23\ProjGameDev\Map\Level1.json"
        public static void LoadMap(string location)
        {
            var mapFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<MapReaderObject>(System.IO.File.ReadAllText(@"D:\ap\22-23\ProjGameDev\Map\Level1.json"));
            var map = Map.GetInstance();
            var frontMapdata = mapFromFile.layers.Where(layer => layer.name == "ForegroundTiles").First().data;
        }
    }
}
