using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Part1;

class Program
{
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines(args[0]).ToList();
        var grid = ParseGrid(lines);
        var schematic = new Schematic(grid);
        var actualParts = schematic.FindActualParts();
        var sum = 0;
        foreach (var partNumber in actualParts)
        {
            sum += partNumber.Number;
        }
        Console.WriteLine($"Sum of actual part numbers: {sum}");
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
