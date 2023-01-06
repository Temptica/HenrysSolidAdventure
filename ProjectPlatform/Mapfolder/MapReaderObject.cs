namespace HenrySolidAdventure.MapFolder
{
    internal class MapReaderObject //fields getting read in via the monogame extended son reader, generated with teh Tiles application.
    {
        public Layer[] Layers { get; set; }
    }

    public class Layer
    {
        public int[] Data { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public Object[] Objects { get; set; }
    }

    public class Object
    {
        [Newtonsoft.Json.JsonProperty("class")]
        public string Class { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }

}
