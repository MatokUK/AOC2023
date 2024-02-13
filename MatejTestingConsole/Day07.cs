
namespace MatejTestingConsole
{

    enum WinningHand
    {
        FIVE_OF_KIND = 900,
        FOUR_OF_KINF = 800,
        FULL_HOUSE = 700,
        THREE_PAIRS = 600,
        TWO_PAIRS = 500,
        PAIR = 100,
    }

    class Hand: IComparable<Hand>
    {
        protected string cards;
        public readonly int pot;
        protected Dictionary<char, int> charFrequency = new();

        public JokerHand Joker
        {
            get
            {
                return new JokerHand(cards, pot);
            }
        }


        public Hand(string cards, int pot)
        { 
            this.cards = cards;
            this.pot = pot;

            foreach (char c in cards)
            {
                if (charFrequency.ContainsKey(c))
                {
                    charFrequency[c]++;
                }
                else
                {
                    charFrequency[c] = 1;
                }
            }
        }

        public virtual int Rank()
        {
            if (charFrequency.Count == 1)
            {
                return (int)WinningHand.FIVE_OF_KIND;
            }

            if (charFrequency.Any(pair => pair.Value == 4))
            {
                return (int)WinningHand.FOUR_OF_KINF;
            }

            if (charFrequency.Any(pair => pair.Value == 3) && charFrequency.Any(pair => pair.Value == 2))
            {
                return (int)WinningHand.FULL_HOUSE;
            }

            if (charFrequency.Count == 3 && charFrequency.Any(pair => pair.Value == 3))
            {
                return (int)WinningHand.THREE_PAIRS;
            }

            if (charFrequency.Count == 3 && charFrequency.Any(pair => pair.Value == 2))
            {
                return (int)WinningHand.TWO_PAIRS;
            }

            if (charFrequency.Count == 4)
            {
                return (int)WinningHand.PAIR;
            }

            return 0;
        }


        public int CompareTo(Hand? other)
        {
            if (this.Rank() == other.Rank())
            {
                return this.CompareTo(other.cards);
            }

            return this.Rank() > other.Rank() ? 1 : -1;
        }

        public int CompareTo(string ? other)
        {
            for (int i = 0; i < this.cards.Length; ++i)
            {
                int valA = CardScore(this.cards[i]);
                int valb = CardScore(other[i]);
                
                if (valA == valb)
                {
                    continue;
                }

                return valA > valb ? 1 : -1;
            }

            return 1;
        }

        protected virtual int CardScore(char value)
        {
            return value switch
            {
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ =>  value - '0',
            };
        }
    }

    class JokerHand : Hand
    {
        public JokerHand(string cards, int pot) : base(cards, pot)
        {
        }

        public override int Rank()
        {
            var baseRank = base.Rank();

            if (charFrequency.ContainsKey('J'))
            {
                switch (charFrequency['J'])
                {
                    case 5: // JJJJJ
                        return (int)WinningHand.FIVE_OF_KIND;

                    case 4: // JJJJX
                        return (int)WinningHand.FIVE_OF_KIND;

                    case 3: // JJJAA   | JJJAB
                        return charFrequency.Count == 2 ? (int)WinningHand.FIVE_OF_KIND : (int)WinningHand.FOUR_OF_KINF;

                    case 2: // JJAAA | JJAAB | JJABC
                        if (charFrequency.Count == 2)
                        {
                            return (int)WinningHand.FIVE_OF_KIND;
                        }
                        return charFrequency.Count == 3 ? (int)WinningHand.FOUR_OF_KINF : (int)WinningHand.THREE_PAIRS;


                    case 1: // JXXXX
                        if (charFrequency.Count == 2)
                        {
                            return (int)WinningHand.FIVE_OF_KIND;
                        }

                        if (charFrequency.Count == 3)
                        { //JAABB | //JAAAB
                            return charFrequency.ContainsValue(3) ? (int)WinningHand.FOUR_OF_KINF : (int)WinningHand.FULL_HOUSE;
                        }

                        if (charFrequency.Count == 4)
                        { //JAABC
                            return (int)WinningHand.THREE_PAIRS;
                        }

                        //JABCD
                        return (int)WinningHand.PAIR;
                }               
            }

            return baseRank;
        }

        protected override int CardScore(char value)
        {
            return value switch
            {
                'T' => 10,
                'J' => 0,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => value - '0',
            };
        }

    }

    public class Day07: AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            SortedList<Hand, string> hands = ReadHands(lines);            
            SortedList<JokerHand, string> jokerHands = new();
            long winningsA = 0;
            long winningsB = 0;
            int rank = 1;

            foreach (var hand in hands)
            {
                winningsA += hand.Key.pot * rank;
                rank++;
                jokerHands.Add(hand.Key.Joker, hand.Value);
            }

            rank = 1;
            foreach (var hand in jokerHands)
            {             
                winningsB += hand.Key.pot * rank;
                rank++;
            }

            return (winningsA, winningsB);
        }


        private SortedList<Hand, string> ReadHands(string[] Lines)
        {
            SortedList<Hand, string> Hands = new SortedList<Hand, string>();

            foreach (var Line in Lines)
            {
                var Parts = Line.Trim().Split(' ');
                int.TryParse(Parts[1], out int pot);

                Hands.Add(new Hand(Parts[0], pot), Parts[0]);
            }

            return Hands;
        }
    }
}
//Respectable
//Mel and Kim