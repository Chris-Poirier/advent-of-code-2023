using System.Collections.Generic;
using System.Linq;

namespace Part2 {
    public class SeedMapCollection {
        public List<SeedMap> SeedMaps { get; set; }

        public SeedMapCollection() {
            SeedMaps = new List<SeedMap>();
        }
        private SeedMapCollection(List<SeedMap> seedMaps) {
            SeedMaps = seedMaps;
        }

        public static SeedMapCollection Parse(string seedMapString) {
            var seedMaps = new List<SeedMap>();
            var seedMapStrings = seedMapString.Split("\r\n");
            foreach (var seedMap in seedMapStrings.Skip(1)) {
                seedMaps.Add(SeedMap.Parse(seedMap));
            }
            return new SeedMapCollection(seedMaps);
        }

        public long GetDestination(long source) {
            foreach (var seedMap in SeedMaps) {
                var destination = seedMap.GetDestination(source);
                if (destination.HasValue) {
                    return destination.Value;
                }
            }
            return source;
        }
    }
}