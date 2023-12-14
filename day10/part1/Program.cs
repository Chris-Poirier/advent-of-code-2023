using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Part1;

class Program
{
    static void Main(string[] args)
    {
        var contents = File.ReadAllLines(args[0]).ToList();
        var map = new Map(contents);
        //Console.WriteLine($"Map:\n{map}");
        var distanceMap = new DistanceMap(map);
        //Console.WriteLine($"Distance Map:\n{distanceMap}");
        var farthest = distanceMap.GetFarthest();
        Console.WriteLine($"Farthest: {farthest}");
    }

}
