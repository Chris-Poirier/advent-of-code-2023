using System.Collections.Generic;

namespace Part1 {
    public class Card {
        private readonly List<char> CardRank = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];

        public char Value { get; set; }
        public int Rank => CardRank.IndexOf(Value);

        public Card(char card) {
            Value = card;
        }
    }
}