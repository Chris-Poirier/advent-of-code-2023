namespace Part1 {
    public class MapPoint {
        public enum PointType {
            Start,
            Ground,
            NorthSouth,
            EastWest,
            NorthWest,
            NorthEast,
            SouthWest,
            SouthEast
        }
        public PointType Type { get; set; }

        public MapPoint(char type) {
            Type = GetPointType(type);
        }

        private PointType GetPointType(char type) {
            switch (type) {
                case '.':
                    return PointType.Ground;
                case 'S':
                    return PointType.Start;
                case '|':
                    return PointType.NorthSouth;
                case '-':
                    return PointType.EastWest;
                case 'J':
                    return PointType.NorthWest;
                case 'L':
                    return PointType.NorthEast;
                case '7':
                    return PointType.SouthWest;
                case 'F':
                    return PointType.SouthEast;
                default:
                    throw new System.Exception($"Unknown point type: {type}");
            }
        }

        public override string ToString() {
            return $"({Type})";
        }
    }
}