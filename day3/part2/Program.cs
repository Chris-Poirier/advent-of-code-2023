using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Part2;

class Program
{
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines(args[0]).ToList();
        var grid = ParseGrid(lines);
        var schematic = new Schematic(grid);
        var gears = schematic.FindGears();
        var sum = 0;
        foreach (var gear in gears)
        {
            sum += gear.Ratio;
        }
        Console.WriteLine($"Sum of gear ratios: {sum}");
    }

    static List<List<char>> ParseGrid(List<string> lines)
    {
        var grid = new List<List<char>>();
        foreach (var line in lines)
        {
            grid.Add([.. line.ToCharArray()]);
        }
        return grid;
    }
}
