using System.Collections.Generic;

namespace Part2 {
    public class Card {
        private readonly List<char> CardRank = ['J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'];

        public char Value { get; set; }
        public int Rank => CardRank.IndexOf(Value);

        public Card(char card) {
            Value = card;
        }

        public override string ToString() {
            return Value.ToString();
        }
    }
}