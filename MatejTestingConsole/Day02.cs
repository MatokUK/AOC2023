using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MatejTestingConsole.Day2Class;

namespace MatejTestingConsole
{
    public class Day02: AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            int sum1 = 0;
            int sum2 = 0;
            for (int i = 0; i < lines.Length; ++i)
            {
                Game game = new Game(lines[i]);

                if (game.isValid(12,14,13))
                {
                    sum1 += game.idx;
                }
               
                sum2 += game.power;
                
            }

            return (sum1, sum2);
        }
    }
}
