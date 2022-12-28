using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OtterlyAdventure.Mapfolder;

namespace OtterlyAdventure.PathFinding
{
    internal class Pathfinding
    {
        public List<PathPoint> Points;
        private IGameObject _target;
        private PathPoint _currentPoint;
        private PathPoint _destinationPoint;

        public bool Initialised { get; private set; }
        public Pathfinding() //between object
        {
        }

        public void Initialise(IGameObject seeker, Vector2 destination)
        {
            Unload();
            if (Points is null)
            {
                BuildMap();
            }
            _currentPoint = new PathPoint(seeker.Position.ToPoint());
            _destinationPoint = new PathPoint(destination.ToPoint());
            _currentPoint.SetDistance(_destinationPoint);
            _currentPoint.Cost = 0;
            _currentPoint.NeighborPoints = GetReachablePoints(_currentPoint);
            _destinationPoint.NeighborPoints = GetReachablePoints(_destinationPoint);
            Initialised = true;

            SetCost();
        }

        public void Initialise(IGameObject seeker, IGameObject target)
        {
            Initialise(seeker, target.Position);
            _target = target;
        }
        private List<PathPoint> GetReachablePoints(PathPoint currentPoint)
        {
            //add all surrounding points in the points list
            var reachablePoints = new List<PathPoint>();
            var x = currentPoint.Position.X;
            var y = currentPoint.Position.Y;
            reachablePoints.AddRange(Points.Where(point =>
                Vector2.Distance(point.Position.ToVector2(), currentPoint.Position.ToVector2()) <= 10f).ToList());
            return reachablePoints;
        }

        public void Unload()
        {
            Initialised = false;
        }
        private void BuildMap()
        {
            var mapTiles = Map.Instance.FrontMap;
            
            if (Points is null)
            {
                Points = new List<PathPoint>();
                var ScreenWitdh = mapTiles.Max(tile => tile.HitBox.Right);
                var ScreenHeight = mapTiles.Max(tile => tile.HitBox.Bottom);
                List<Thread> threads = new List<Thread>();
                for (int x = 0; x < ScreenWitdh; x +=5)
                {
                    threads.Add( new Thread(() => MakePoints(ScreenHeight, x, mapTiles)));
                }
                //run threads and wait untill the task is fully done, let 10 theards work at the same time
                foreach (var thread in threads)
                {
                    thread.Start();
                }
                while (threads.Any(thread => thread.IsAlive))
                {
                    Thread.Sleep(1);
                }
            }
            //find neighbor points within a 1 point radius
            foreach (var point in Points)
            {
                point.NeighborPoints = GetReachablePoints(point);
            }

        }
        private void MakePoints(int ScreenHeight, int x, List<MapTile> mapTiles)
        {
            for (int y = 0; y < ScreenHeight; y += 5)
            {
                var point = new PathPoint(new Point(x, y));
                if (mapTiles.Any(tile => tile.HitBox.Contains(point.Position) && tile.Tile.Type == TileType.Air))
                {
                    Points.Add(point);
                }
            }
        }

        public void Update()
        {
            if (_currentPoint != _destinationPoint)
            {
                var nextPoints = Points.Where(point => Vector2.Distance(point.Position.ToVector2(), _currentPoint.Position.ToVector2()) <= 5);
                foreach (var point in nextPoints)
                {
                    //if the cost of the current point + the distance to the next point is less than the cost of the next point
                    if (_currentPoint.Cost + Vector2.Distance(point.Position.ToVector2(), _currentPoint.Position.ToVector2()) < point.Cost)
                    {
                        //set the cost of the next point to the cost of the current point + the distance to the next point
                        point.Cost = _currentPoint.Cost + Vector2.Distance(point.Position.ToVector2(), _currentPoint.Position.ToVector2());
                        //set the parent of the next point to the current point
                        //point.Parent = _currentPoint;
                    }
                }
                //set the current point to the point with the lowest total cost
                _currentPoint = Points.OrderBy(point => point.TotalCost).First();
            }
            //if the current point is the destination point
            else
            {
                if (_target.Position != _currentPoint.Position.ToVector2())
                {
                    _target.Position = _currentPoint.Position.ToVector2();
                }
                else
                {
                    //if (_currentPoint.Parent != _currentPoint)
                    //{
                    //    _currentPoint = _currentPoint.Parent;
                    //}
                }
            }
        }
        
        public List<PathPoint> GetPath()
        {
            var visited = new List<PathPoint>();
            var path = new List<PathPoint>();
            var current = _destinationPoint;
            while (current != _currentPoint)
            {
                visited.Add(current);
                var next = current.NeighborPoints.OrderBy(point => point.TotalCost).First();
                if (visited.Contains(next))
                {
                    next = current.NeighborPoints.OrderBy(point => point.TotalCost).First();
                }
                current = next;
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
        private void SetCost()
        {
            foreach (var point in Points)
            {
                point.Cost = Vector2.Distance(point.Position.ToVector2(), _currentPoint.Position.ToVector2());
            }
        }
    }
}
