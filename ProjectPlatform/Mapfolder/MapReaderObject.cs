using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtterlyAdventure.Mapfolder
{
    internal class MapReaderObject
    {
        public Layer[] layers { get; set; }
    }

    public class Layer
    {
        public int[] data { get; set; }
        public int height { get; set; }
        public string name { get; set; }
        public int width { get; set; }
        public Object[] objects { get; set; }
    }

    public class Object
    {
        [Newtonsoft.Json.JsonProperty("class")]
        public string _class { get; set; }
        public float x { get; set; }
        public float y { get; set; }
    }

}
