using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace OtterlyAdventure.PathFinding
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
