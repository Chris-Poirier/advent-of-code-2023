using System.Collections.Generic;
using System.Linq;

namespace Part2 {
    public class Symbol {
        public char Character {get; set;}
        public int X {get; set;}
        public int Y {get; set;}
        public Symbol(char character, int x, int y) {
            Character = character;
            X = x;
            Y = y;
        }

        public bool IsAdjacentTo(int x, int y) {
            return
                (x == X - 1 && (y == Y || y == Y - 1 || y == Y + 1)) ||
                (x == X + 1 && (y == Y || y == Y - 1 || y == Y + 1)) ||
                (x == X && (y == Y - 1 || y == Y + 1));
        }

        public Gear ToGear(List<PartNumber> partNumbers) {
            var adjacentParts = partNumbers.Where(p => {
                for(int i=p.X; i < p.X + p.Width; i++) {
                    if (IsAdjacentTo(i, p.Y)) {
                        return true;
                    }
                }
                return false;
            }).ToList();
            if (adjacentParts.Count == 2) {
                var ratio = adjacentParts[0].Number * adjacentParts[1].Number;
                return new Gear(X, Y, ratio);
            }
            return null;
        }

        public override string ToString() {
            return $"Symbol({Character}, {X}, {Y})";
        }
    }
}