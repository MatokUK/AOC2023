using System.Text.RegularExpressions;

namespace MatejTestingConsole.Day2Class
{
    class Game
    {
        public readonly Int32 idx;
        public readonly Int32 power;
        public readonly List<int> red = new List<int>();
        public readonly List<int> blue =  new List<int>();
        public readonly List<int> green = new List<int>();

        public Game(string game)
        {            
            idx = parseIdx(game);
            
            parseIterations(game);

            power = red.Max() * blue.Max() * green.Max();
        }


        private Int32 parseIdx(string game)
        {
            Regex rg = new Regex(@"Game ([0-9]+)");

            MatchCollection match = rg.Matches(game);

            return Int32.Parse(match.ElementAt(0).Groups[1].Value);
        }

        private void parseIterations(string game)
        {
            string[] iterations = Regex.Split(game, @";");
            foreach (string iteration in iterations)
            {
                this.red.Add(parseBalls(iteration, "red"));
                this.blue.Add(parseBalls(iteration, "blue"));
                this.green.Add(parseBalls(iteration, "green"));
            }
        }

        private Int32 parseBalls(string game, string color)
        {
            Regex rg = new Regex(@"([0-9]+) " + color);

            MatchCollection match = rg.Matches(game);
            if (match.Count == 0)
            {
                return 0;
            }

            return Int32.Parse(match.ElementAt(0).Groups[1].Value);
        }

        public bool isValid(int MaxRed, int MaxBlue, int MaxGreen)
        {
            foreach (int Count in red)  {
                if (Count > MaxRed)  {
                    return false;
                }
            }

            foreach (int Count in blue)  {
                if (Count > MaxBlue)  {
                    return false;
                }
            }

            foreach (int Count in green)
            {
                if (Count > MaxGreen)
                {
                    return false;
                }
            }


            return true;
        }


    }
}
