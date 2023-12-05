namespace Part2 {
    public class Seed {
        public long StartValue { get; set; }
        public long Length { get; set; }

        public Seed(long startValue, long length) {
            StartValue = startValue;
            Length = length;
        }
    }
}