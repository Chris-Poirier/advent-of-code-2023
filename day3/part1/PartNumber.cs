using System.Collections.Generic;

namespace Part1 {
    public class PartNumber {
        public int Number {get; set;}
        public int X {get; set;}
        public int Y {get; set;}
        public int Width {get; set;}
        public PartNumber(int number, int x, int y, int width) {
            Number = number;
            X = x;
            Y = y;
            Width = width;
        }

        public bool IsActualPart(List<Symbol> symbols) {
            for(var i=X; i<X+Width; i++) {
                var symbol = symbols.Find(s => s.IsAdjacentTo(i, Y));
                if(symbol != null)
                    return true;
            }
            return false;
        }

        public override string ToString() {
            return $"PartNumber({Number}, {X}, {Y}, {Width})";
        }
    }
}