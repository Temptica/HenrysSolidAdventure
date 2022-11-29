using Microsoft.Xna.Framework;
using ProjectPlatform.Mapfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.PathFinding
{
    internal class PathTile
    {
        
        public MapTile MapTile { get; set; }
        public Vector2 Position => MapTile.HitBox.Center.ToVector2(); //object shall always navigate to the center of the tile
public float Cost { get; set; }
        public float Distance { get; set; }
        public float TotalCost => Cost + Distance;
        public PathTile Parent { get; set; }
        public PathTile(MapTile mapTile)
        {
            MapTile = mapTile;            
        }
        public void SetDistance(PathTile target)
        {
            Distance = Math.Abs(MapTile.Position.X - target.MapTile.Position.X) + Math.Abs(MapTile.Position.Y - target.MapTile.Position.Y);
        }
        
        

    }
}
