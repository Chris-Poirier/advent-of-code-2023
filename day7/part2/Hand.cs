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

        private Dictionary<char, int> ApplyWildcards(Dictionary<char, int> cardCounts) { 
            if(!cardCounts.ContainsKey('J')) {
                return cardCounts;
            }
            var wildCount = cardCounts['J'];
            var possibleHands = new List<Dictionary<char, int>>();

            // Generate all possible hands by replacing J cards with every other possible value
            for (int i = 0; i <= wildCount; i++)
            {
                var currentHand = new Dictionary<char, int>(cardCounts);
                currentHand['J'] -= i;

                foreach (var card in cardCounts.Keys)
                {
                    if (card != 'J')
                    {
                        currentHand[card] += i;
                        possibleHands.Add(new Dictionary<char, int>(currentHand));
                        currentHand[card] -= i;
                    }
                }
            }

            // Find the highest value hand among all possible hands
            Dictionary<char, int> highestHand = null;

            foreach (var hand in possibleHands)
            {
                var handType = GetHandTypeFromCounts(hand);
                if (highestHand == null || CompareHandTypes(handType, GetHandTypeFromCounts(highestHand)) > 0)
                {
                    highestHand = hand;
                }
            }

            return highestHand ?? cardCounts;
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
                return CompareHandTypes(Type, other.Type);
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

        public static int CompareHandTypes(HandType type1, HandType type2) {
            var type1Val = Enum.GetValues(typeof(HandType)).Cast<HandType>().ToList().IndexOf(type1);
            var type2Val = Enum.GetValues(typeof(HandType)).Cast<HandType>().ToList().IndexOf(type2);
            return type1Val.CompareTo(type2Val);
        }

        public override string ToString() {
            return $"{Type} {Cards[0]} {Cards[1]} {Cards[2]} {Cards[3]} {Cards[4]}";
        }
    }
}