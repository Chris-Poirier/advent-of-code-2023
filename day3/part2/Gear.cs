namespace Part2 {
    public class Gear {
        public int X {get; set;}
        public int Y {get; set;}
        public int Ratio {get; set;}
        public Gear(int x, int y, int ratio) {
            X = x;
            Y = y;
            Ratio = ratio;
        }

        public override string ToString() {
            return $"Gear({X}, {Y}, {Ratio})";
        }
    }
}