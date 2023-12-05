using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Part1;

class Program
{
    static void Main(string[] args)
    {
        var contents = File.ReadAllText(args[0]);
        var contentSections = contents.Split("\r\n\r\n");

        var seeds = GetSeedList(contentSections[0]);
        var seedToSoil = SeedMapCollection.Parse(contentSections[1]);
        var soilToFertilizer = SeedMapCollection.Parse(contentSections[2]);
        var fertilizerToWater = SeedMapCollection.Parse(contentSections[3]);
        var waterToLight = SeedMapCollection.Parse(contentSections[4]);
        var lightToTemperature = SeedMapCollection.Parse(contentSections[5]);
        var temperatureToHumidity = SeedMapCollection.Parse(contentSections[6]);
        var humidityToLocation = SeedMapCollection.Parse(contentSections[7]);

        var locations = new List<long>();
        foreach (var seed in seeds)
        {
            var soil = seedToSoil.GetDestination(seed);
            var fertilizer = soilToFertilizer.GetDestination(soil);
            var water = fertilizerToWater.GetDestination(fertilizer);
            var light = waterToLight.GetDestination(water);
            var temperature = lightToTemperature.GetDestination(light);
            var humidity = temperatureToHumidity.GetDestination(temperature);
            var location = humidityToLocation.GetDestination(humidity);
            locations.Add(location);
            //Console.WriteLine($"Seed: {seed}, Location: {location}");
        }

        var minLocation = locations.Min();
        Console.WriteLine($"Closest location number: {minLocation}");
    }

    static List<long> GetSeedList(string seedString)
    {
        var seeds = new List<long>();
        foreach (var seed in seedString.Substring(7).Split(" "))
        {
            seeds.Add(long.Parse(seed));
        }
        //Console.WriteLine($"Seeds: {string.Join(", ", seeds)}");
        return seeds;
    }
}
