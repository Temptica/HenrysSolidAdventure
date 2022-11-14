using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.Mapfolder
{
    internal class MapReaderObject
    {
            public int height { get; set; }
            public bool infinite { get; set; }
            public Layer[] layers { get; set; }
            public int nextlayerid { get; set; }
            public int nextobjectid { get; set; }
            public string orientation { get; set; }
            public string renderorder { get; set; }
            public string tiledversion { get; set; }
            public int tileheight { get; set; }
            public int tilewidth { get; set; }
            public string type { get; set; }
            public string version { get; set; }
            public int width { get; set; }       

    }
    public class Layer
    {
        public int id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public float offsetx { get; set; }
        public int offsety { get; set; }
        public int opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int[] data { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
}
