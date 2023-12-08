using System.Collections.Generic;
using System;
using System.Linq;

namespace Part2 {
    public class Hand : IComparable<Hand> {
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
        public Hand(List<Card> cards, long bid) {
            Cards = cards;
            Bid = bid;
            Type = GetHandType();
        }

        private HandType GetHandType() {
            var cardCounts = GetCardCounts();
            cardCounts = ApplyWildcards(cardCounts);
            return GetHandTypeFromCounts(cardCounts);
        }

        private Dictionary<char, int> GetCardCounts() {
            var cardCounts = new Dictionary<char, int>();
            foreach (var card in Cards) {
                if (!cardCounts.ContainsKey(card.Value)) {
                    cardCounts.Add(card.Value, 0);
                }
                cardCounts[card.Value]++;
            }
            return cardCounts;
        }

        private Dictionary<char, int> ApplyWildcards(Dictionary<char, int> cardCounts) {
            if (cardCounts.ContainsKey('J')) {
                var wildCount = cardCounts['J'];
                cardCounts.Remove('J');

                // Check for five of a kind
                var fiveKey = cardCounts.Keys.FirstOrDefault(c => cardCounts[c]+wildCount == 5);
                if (fiveKey != default(char)) {
                    cardCounts[fiveKey] = 5;
                    return cardCounts;
                }
                // Check for four of a kind
                var fourKey = cardCounts.Keys.FirstOrDefault(c => cardCounts[c]+wildCount == 4);
                if (fourKey != default(char)) {
                    cardCounts[fourKey] = 4;
                    return cardCounts;
                }

                /*//Console.WriteLine($"Hand: {this}");
                Hand wildHand = null;
                foreach (var card in cardCounts.Keys) {
                    var newCards = Cards.Select(c => new Card(c.Value)).ToList();
                    foreach (var wildCard in newCards.Where(c => c.Value == 'J')) {
                        wildCard.Value = card;
                    }
                    var newHand = new Hand(newCards, Bid);
                    if (wildHand == null || newHand.CompareTo(wildHand) > 0) {
                        wildHand = newHand;
                    }
                }
                //Console.WriteLine($"Wild Hand: {wildHand}");
                return wildHand != null ? wildHand.GetCardCounts() : cardCounts;*/
            }
            return cardCounts;
        }

        private HandType GetHandTypeFromCounts(Dictionary<char, int> cardCounts) {
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

        public int CompareTo(Hand other) {
            if (Type != other.Type) {
                var myTypeVal = Enum.GetValues(typeof(HandType)).Cast<HandType>().ToList().IndexOf(Type);
                var otherTypeVal = Enum.GetValues(typeof(HandType)).Cast<HandType>().ToList().IndexOf(other.Type);
                return myTypeVal.CompareTo(otherTypeVal);
            }
            for (var i = 0; i < Cards.Count; i++) {
                if (Cards[i].Rank > other.Cards[i].Rank) {
                    return -1;
                }
                if (Cards[i].Rank < other.Cards[i].Rank) {
                    return 1;
                }
            }
            return 0;
        }

        public override string ToString() {
            return $"{Type} {Cards[0]} {Cards[1]} {Cards[2]} {Cards[3]} {Cards[4]}";
        }
    }
}