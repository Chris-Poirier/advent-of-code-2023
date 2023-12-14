using System.Collections.Generic;

namespace Part1 {
    public class DistanceMap {
        public List<List<int>> Distances { get; set; }

        public DistanceMap(Map map) {
            Distances = new List<List<int>>();
            InitDistances(map);
            GetDistances(map);
        }

        private void InitDistances(Map map) {
            for(var y = 0; y < map.Points.Count; y++) {
                var row = new List<int>();
                for(var x = 0; x < map.Points[y].Count; x++) {
                    row.Add(-1);
                }
                Distances.Add(row);
            }
        }

        public void GetDistances(Map map) {
            var (start, x0, y0) = map.GetStart();
            if(start == null) {
                throw new System.Exception("No start found");
            }
            Distances[y0][x0] = 0;

            var connections = new List<(int, int)>();
            if(x0 > 0) {
                if(map[x0 - 1, y0].Type == MapPoint.PointType.EastWest 
                    || map[x0 - 1, y0].Type == MapPoint.PointType.NorthEast 
                    || map[x0 - 1, y0].Type == MapPoint.PointType.SouthEast) {
                    Distances[y0][x0 - 1] = 1;
                    connections.Add((x0 - 1, y0));
                }
            }
            if(x0 < map.Points[y0].Count - 1) {
                if(map[x0 + 1, y0].Type == MapPoint.PointType.EastWest 
                    || map[x0 + 1, y0].Type == MapPoint.PointType.NorthWest 
                    || map[x0 + 1, y0].Type == MapPoint.PointType.SouthWest) {
                    Distances[y0][x0 + 1] = 1;
                    connections.Add((x0 + 1, y0));
                }
            }
            if(y0 > 0) {
                if(map[x0, y0 - 1].Type == MapPoint.PointType.NorthSouth 
                    || map[x0, y0 - 1].Type == MapPoint.PointType.SouthEast 
                    || map[x0, y0 - 1].Type == MapPoint.PointType.SouthWest) {
                    Distances[y0 - 1][x0] = 1;
                    connections.Add((x0, y0 - 1));
                }
            }
            if(y0 < map.Points.Count - 1) {
                if(map[x0, y0 + 1].Type == MapPoint.PointType.NorthSouth 
                    || map[x0, y0 + 1].Type == MapPoint.PointType.NorthEast 
                    || map[x0, y0 + 1].Type == MapPoint.PointType.NorthWest) {
                    Distances[y0 + 1][x0] = 1;
                    connections.Add((x0, y0 + 1));
                }
            }

            FollowConnections(map, connections, 2);
        }

        private void FollowConnections(Map map, List<(int, int)> connections, int steps) {
            var newConnections = new List<(int, int)>();
            foreach(var (x, y) in connections) {
                if(map[x, y].Type == MapPoint.PointType.EastWest 
                    || map[x, y].Type == MapPoint.PointType.NorthWest 
                    || map[x, y].Type == MapPoint.PointType.SouthWest) {
                    if(x > 0 && Distances[y][x - 1] == -1) {
                        Distances[y][x - 1] = steps;
                        newConnections.Add((x - 1, y));
                    }
                }
                if(map[x, y].Type == MapPoint.PointType.EastWest 
                    || map[x, y].Type == MapPoint.PointType.NorthEast 
                    || map[x, y].Type == MapPoint.PointType.SouthEast) {
                    if(x < map.Points[y].Count - 1 && Distances[y][x + 1] == -1) {
                        Distances[y][x + 1] = steps;
                        newConnections.Add((x + 1, y));
                    }
                }
                if(map[x, y].Type == MapPoint.PointType.NorthSouth 
                    || map[x, y].Type == MapPoint.PointType.NorthEast 
                    || map[x, y].Type == MapPoint.PointType.NorthWest) {
                    if(y > 0 && Distances[y - 1][x] == -1) {
                        Distances[y - 1][x] = steps;
                        newConnections.Add((x, y - 1));
                    }
                }
                if(map[x, y].Type == MapPoint.PointType.NorthSouth 
                    || map[x, y].Type == MapPoint.PointType.SouthEast 
                    || map[x, y].Type == MapPoint.PointType.SouthWest) {
                    if(y < map.Points.Count - 1 && Distances[y + 1][x] == -1) {
                        Distances[y + 1][x] = steps;
                        newConnections.Add((x, y + 1));
                    }
                }
            }
            if(newConnections.Count > 0) {
                FollowConnections(map, newConnections, steps + 1);
            }
        }

        public int GetFarthest() {
            var farthest = 0;
            foreach(var distanceRow in Distances) {
                foreach(var distance in distanceRow) {
                    if(distance > farthest) {
                        farthest = distance;
                    }
                }
            }
            return farthest;
        }

        public override string ToString() {
            var distanceString = "";
            foreach (var distanceRow in Distances) {
                foreach (var distance in distanceRow) {
                    distanceString += $"{distance}".PadRight(3);
                }
                distanceString += "\n";
            }
            return distanceString;
        }
    }
}