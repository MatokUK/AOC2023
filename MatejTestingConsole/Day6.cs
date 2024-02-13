using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MatejTestingConsole.Day2Class;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Timers;
using System.Diagnostics;

namespace MatejTestingConsole
{
    public class Day6 : AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            Dictionary<int, int> Races = ReadRaces(lines[0], lines[1]);
            long product = 1;
            
            foreach (var Race in Races)
            {
                product *= WayOfWins(Race.Key, Race.Value);
            }

            long.TryParse(string.Join("", Races.Keys), out var TotalTime);
            long.TryParse(string.Join("", Races.Values), out var TotalDistance);

            return (product, WayOfWins(TotalTime, TotalDistance));
        }

        private long WayOfWins<T>(T Time, T Distace)
        {
            long ways = 0;

            for (dynamic s = 1; s < Time; ++s)
            {
                var RemaingingTime = Time - s;

                if (RemaingingTime * s > Distace)
                {
                    ways++;
                }

            }

            return ways;
        }

        private Dictionary<int, int> ReadRaces(string Time, string Distance)
        {
            List<int> Times = ReadLine(Time);
            List<int> Distances =ReadLine(Distance);

            return Times.Zip(Distances, (time, distance) => new { Time = time, Distance = distance })
                                                      .ToDictionary(pair => pair.Time, pair => pair.Distance);
        }

        private List<int> ReadLine(string Line)
        {
            List<int> Values = new List<int>();

            var idx = Line.IndexOf(':');

            string[] parts = Regex.Split(Line.Substring(idx + 2).Trim(), "\\s+",
                                  RegexOptions.IgnoreCase);


            foreach (var part in parts)
            {
                int.TryParse(part, out var Value);

                Values.Add(Value);
            }

            return Values;
        }
    }
}
