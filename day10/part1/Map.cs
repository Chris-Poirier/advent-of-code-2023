using System.Collections.Generic;
using System.Linq;

namespace Part1 {
    public class Map {
        public List<List<MapPoint>> Points { get; set; }

        public Map(List<string> mapStrings) {
            Points = new List<List<MapPoint>>();
            for (var y = 0; y < mapStrings.Count; y++) {
                var mapRow = new List<MapPoint>();
                for (var x = 0; x < mapStrings[y].Length; x++) {
                    mapRow.Add(new MapPoint(mapStrings[y][x]));
                }
                Points.Add(mapRow);
            }
        }

        public (MapPoint, int, int) GetStart() {
            for(var y = 0; y < Points.Count; y++) {
                for(var x = 0; x < Points[y].Count; x++) {
                    if (Points[y][x].Type == MapPoint.PointType.Start) {
                        return (Points[y][x], x, y);
                    }
                }
            }
            return (null, -1, -1);
        }

        public MapPoint this[int x, int y] {
            get {
                if (y >= 0 && y < Points.Count && x >= 0 && x < Points[y].Count) {
                    return Points[y][x];
                }
                return null;
            }
        }

        public override string ToString() {
            var mapString = "";
            foreach (var mapRow in Points) {
                foreach (var mapPoint in mapRow) {
                    mapString += mapPoint.ToString().PadRight(13);
                }
                mapString += "\n";
            }
            return mapString;
        }
    }
}