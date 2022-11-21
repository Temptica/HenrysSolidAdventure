using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.Mapfolder
{
    internal class MapReaderObject
    {
        public int compressionlevel { get; set; }
        public int height { get; set; }
        public bool infinite { get; set; }
        public Layer[] layers { get; set; }
        public int nextlayerid { get; set; }
        public int nextobjectid { get; set; }
        public string orientation { get; set; }
        public string renderorder { get; set; }
        public string tiledversion { get; set; }
        public int tileheight { get; set; }
        public Tileset[] tilesets { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public string version { get; set; }
        public int width { get; set; }
    }

    public class Layer
    {
        public int[] data { get; set; }
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string draworder { get; set; }
        public Object[] objects { get; set; }
    }

    public class Object
    {
        public string _class { get; set; }
        public long gid { get; set; }
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int rotation { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public float x { get; set; }
        public float y { get; set; }
    }

    public class Tileset
    {
        public int firstgid { get; set; }
        public string source { get; set; }
    }

}
