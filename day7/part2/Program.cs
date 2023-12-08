using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Part2;

class Program
{
    static void Main(string[] args)
    {
        var contents = File.ReadAllLines(args[0]);
        var hands = new List<Hand>();
        foreach (var line in contents)
        {
           hands.Add(new Hand(line));
        }
        RankHands(hands);
        var winnnings = hands.Sum(h => h.Bid * h.Rank);
        Console.WriteLine($"Winnings: {winnnings}");
    }

    static void RankHands(List<Hand> hands)
    {
        var curRank = hands.Count;
        var curType = Hand.HandType.FiveOfAKind;
        curRank = RankHandsOfType(hands, curRank, curType);
        curType = Hand.HandType.FourOfAKind;
        curRank = RankHandsOfType(hands, curRank, curType);
        curType = Hand.HandType.FullHouse;
        curRank = RankHandsOfType(hands, curRank, curType);
        curType = Hand.HandType.ThreeOfAKind;
        curRank = RankHandsOfType(hands, curRank, curType);
        curType = Hand.HandType.TwoPair;
        curRank = RankHandsOfType(hands, curRank, curType);
        curType = Hand.HandType.OnePair;
        curRank = RankHandsOfType(hands, curRank, curType);
        curType = Hand.HandType.HighCard;
        curRank = RankHandsOfType(hands, curRank, curType);
    }

    static int RankHandsOfType(List<Hand> hands, int curRank, Hand.HandType curType)
    {
        var handsOfType = hands.Where(h => h.Type == curType).ToList();
        if (handsOfType.Count == 0)
        {
            return curRank;
        }
        handsOfType.Sort((h1, h2) => h1.CompareTo(h2));
        foreach (var hand in handsOfType)
        {
            hand.Rank = curRank;
            curRank--;
        }
        return curRank;
    }
}
