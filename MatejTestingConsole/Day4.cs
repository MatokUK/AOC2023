using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MatejTestingConsole.Day2Class;
using System.Globalization;

namespace MatejTestingConsole
{
    public class Card
    {
        public readonly int Id;
        private List<int> Numbers = new List<int>();
        private List<int> Winning = new List<int>();

        public Card(string line)
        {
            var idx = line.IndexOf(':');
            var parts = line.Substring(idx + 2).Split(" | ");

            int.TryParse(line.Substring(0, idx).Trim().Substring(4), out Id);

            foreach (string numString in parts[0].Split(' '))
            {
                if (int.TryParse(numString, out int number))
                {
                    Numbers.Add(number);
                }
            }

            foreach (string numString in parts[1].Split(' '))
            {
                if (int.TryParse(numString, out int number))
                {
                    Winning.Add(number);
                }
            }
        }

        public int getPoints()
        {
            int points = 0;
            foreach (int i in Numbers)
            {
                if (Winning.Contains(i))
                {
                    points = points == 0 ? 1 : points * 2;
                }
            }

            return points;
        }

        public int getNextWinningCards()
        {
            int cards = 0;
            foreach (int i in Numbers)
            {
                if (Winning.Contains(i))
                {
                    cards++;
                }
            }

            return cards;
        }
    }


    public class Day4 : AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            List<Card> cards = readSchema(lines);
            int points = 0;

            foreach (Card card in cards)
            {
                points += card.getPoints();
            }


            SortedDictionary<int, int> copies = new SortedDictionary<int, int> ();
            foreach (Card card in cards)
            {
                var WonCopies = card.getNextWinningCards();


                for (int i = card.Id+1; i <= card.Id + WonCopies; ++i)
                {
                    if (!copies.ContainsKey(i))
                    {
                        copies[i] = 0;
                    }

                    copies[i] += 1 + copies.GetValueOrDefault(card.Id, 0);
                }
            }

            return (points, cards.Count + copies.Values.Sum());
        }

        private List<Card> readSchema(string[] lines)
        {
            List <Card> cards = new List<Card>();

            foreach (var line in lines)
            {
                cards.Add(new Card(line));
            }

            return cards;
        }
    }
}
