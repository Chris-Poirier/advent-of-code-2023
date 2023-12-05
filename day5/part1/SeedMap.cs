namespace Part1 {
    public class SeedMap {
        public long DestinationStart { get; set; }
        public long SourceStart { get; set; }
        public long RangeCount { get; set; }

        private SeedMap(long destinationStart, long sourceStart, long rangeCount) {
            DestinationStart = destinationStart;
            SourceStart = sourceStart;
            RangeCount = rangeCount;
        }

        public static SeedMap Parse(string mapString) {
            var mapParts = mapString.Split(" ");
            var destinationStart = long.Parse(mapParts[0]);
            var sourceStart = long.Parse(mapParts[1]);
            var rangeCount = long.Parse(mapParts[2]);
            return new SeedMap(destinationStart, sourceStart, rangeCount);
        }

        public long? GetDestination(long source) {
            if (source < SourceStart || source >= SourceStart + RangeCount) {
                return null;
            }
            return DestinationStart + (source - SourceStart);
        }
    }
}