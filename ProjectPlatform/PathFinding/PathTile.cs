using Microsoft.Xna.Framework;
using ProjectPlatform.Mapfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.PathFinding
{
    internal class PathPoint
    {
        public Point Position { get; }
        public float Cost { get; set; }
        public float Distance { get; set; }
        public float TotalCost => Cost + Distance;
        public List<PathPoint> NeighborPoints { get; set; }
        public PathPoint(Point position)
        {
            Position = position;      
        }
        public void SetDistance(PathPoint target)
        {
            Distance = Vector2.Distance(Position.ToVector2(), target.Position.ToVector2());
        }
    }
}
