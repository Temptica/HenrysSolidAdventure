using ProjectPlatform.Interface;
using ProjectPlatform.Mapfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlatform.PathFinding
{
    internal class Pathfinding
    {
        List<PathTile> pathTiles;
        IGameObject _target;
        PathTile _currentTile;
        PathTile _destinationTile;
        
        public Pathfinding(List<MapTile> mapTiles, IGameObject seaker, IGameObject target)
        {
            _target = target;
            foreach (var mapTile in mapTiles)
            {
                pathTiles.Add(new PathTile(mapTile));
            }
            PathTile _currentTile = pathTiles.First(tile => tile.MapTile.HitBox.Contains(seaker.HitBox.Center));
            PathTile _destinationTile = pathTiles.First(tile => tile.MapTile.HitBox.Contains(target.HitBox.Center));
            _currentTile.SetDistance(_destinationTile);//shortest path without collision
            _currentTile.Cost = 0;
            _currentTile.Parent = null;
            List<PathTile> openList = new();
            List<PathTile> closedList = new();
            openList.Add(_currentTile);
            while (openList.Count > 0)
            {
                PathTile currentTile = openList.OrderBy(tile => tile.TotalCost).First();
                if (currentTile == _destinationTile)
                {
                    break;
                }
                openList.Remove(currentTile);
                closedList.Add(currentTile);
                foreach (var neighbour in GetNeighbours(currentTile))
                {
                    if (closedList.Contains(neighbour))
                    {
                        continue;
                    }
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                    float newCost = currentTile.Cost + 1;
                    if (newCost < neighbour.Cost)
                    {
                        neighbour.Cost = newCost;
                        neighbour.Parent = currentTile;
                        neighbour.SetDistance(_destinationTile);
                    }
                }
            }
        }
        private List<PathTile> GetNeighbours(PathTile currentTile)
        {
            List<PathTile> neighbours = new();
            foreach (var tile in pathTiles)
            {
                if (tile.MapTile.HitBox.Intersects(currentTile.MapTile.HitBox))
                {
                    neighbours.Add(tile);
                }
            }
            return neighbours;
        }
        public List<PathTile> GetPath()
        {
            List<PathTile> path = new();
            PathTile currentTile = _destinationTile;
            while (currentTile != null)
            {
                path.Add(currentTile);
                currentTile = currentTile.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}
