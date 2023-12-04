namespace Part1 {
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

        public override string ToString() {
            return $"Symbol({Character}, {X}, {Y})";
        }
    }
}