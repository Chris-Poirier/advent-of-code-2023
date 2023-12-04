using System.Collections.Generic;

namespace Part1{ 
    public class Schematic {
        public List<PartNumber> PartNumbers {get; private set;}
        public List<Symbol> Symbols {get; private set;}
        private int Width {get; set;}
        private int Height {get; set;}

        public Schematic(List<List<char>> grid) {
            PartNumbers = [];
            Symbols = [];
            InitFromGrid(grid);
        }

        private void InitFromGrid(List<List<char>> grid) {
            Width = grid[0].Count;
            Height = grid.Count;
            for (int y = 0; y < Height; y++) {
                var row = grid[y];
                for (int x = 0; x < Width; x++) {
                    var character = row[x];
                    if (char.IsDigit(character)) {
                        var numChars = (List<char>)[];
                        numChars.Add(character);
                        var width = 1;
                        while (x + width < Width && char.IsDigit(row[x + width])) {
                            width++;
                            numChars.Add(row[x + width - 1]);
                        }
                        var number = int.Parse(string.Join("", numChars));
                        var partNumber = new PartNumber(number, x, y, width);
                        PartNumbers.Add(partNumber);
                        x += width - 1;
                    } else if (character != '.') {
                        var symbol = new Symbol(character, x, y);
                        Symbols.Add(symbol);
                    }
                }
            }
        }

        public List<PartNumber> FindActualParts() {
            var actualParts = (List<PartNumber>)[];
            foreach (var partNumber in PartNumbers) {
                if(partNumber.IsActualPart(Symbols))
                    actualParts.Add(partNumber);
            }
            return actualParts;
        }

        public override string ToString() {
            var str = "Schematic:\n";
            str += $"  Width: {Width}\n";
            str += $"  Height: {Height}\n";
            str += $"  PartNumbers:\n";
            foreach (var partNumber in PartNumbers) {
                str += $"    {partNumber}\n";
            }
            str += $"  Symbols:\n";
            foreach (var symbol in Symbols) {
                str += $"    {symbol}\n";
            }
            return str;
        }
    }
}