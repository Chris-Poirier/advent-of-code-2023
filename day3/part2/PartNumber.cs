using System.Collections.Generic;

namespace Part2 {
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

        public override string ToString() {
            return $"PartNumber({Number}, {X}, {Y}, {Width})";
        }
    }
}