using System.Collections.Generic;

namespace Part1 {
    public class Hand {
        public enum HandType {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        public List<Card> Cards { get; set; }
        public long Bid { get; set; }
        public HandType Type { get; set; }
        public int Rank { get; set; } = -1;

        public Hand(string handString) {
            Cards = new List<Card>();
            var parts = handString.Split(" ");
            foreach (var card in parts[0]) {
                Cards.Add(new Card(card));
            }
            Bid = long.Parse(parts[1]);
            Type = GetHandType();
        }

        private HandType GetHandType() {
            var cardCounts = new Dictionary<char, int>();
            foreach (var card in Cards) {
                if (!cardCounts.ContainsKey(card.Value)) {
                    cardCounts.Add(card.Value, 0);
                }
                cardCounts[card.Value]++;
            }
            var cardCountCounts = new Dictionary<int, int>();
            foreach (var cardCount in cardCounts.Values) {
                if (!cardCountCounts.ContainsKey(cardCount)) {
                    cardCountCounts.Add(cardCount, 0);
                }
                cardCountCounts[cardCount]++;
            }
            if (cardCountCounts.ContainsKey(5)) {
                return HandType.FiveOfAKind;
            }
            if (cardCountCounts.ContainsKey(4)) {
                return HandType.FourOfAKind;
            }
            if (cardCountCounts.ContainsKey(3) && cardCountCounts.ContainsKey(2)) {
                return HandType.FullHouse;
            }
            if (cardCountCounts.ContainsKey(3)) {
                return HandType.ThreeOfAKind;
            }
            if (cardCountCounts.ContainsKey(2) && cardCountCounts[2] == 2) {
                return HandType.TwoPair;
            }
            if (cardCountCounts.ContainsKey(2)) {
                return HandType.OnePair;
            }
            return HandType.HighCard;
        }
    }
}