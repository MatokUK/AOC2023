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
using System.Reflection;
using Microsoft.VisualBasic;
using System.Buffers;

namespace MatejTestingConsole
{
    public class Day15: AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            string[] sequnce = ReadSequence(lines[0]);

            long part1 = 0;
            long part2 = 0;


            var s = "rn=1";

            foreach (string str in sequnce) {
                part1 += hash(str);
            }

            return (part1, part2);
        }

        private int hash(string s)
        {
            return s.Select((c) => (byte)c).Aggregate(0, (int acc, byte num) => ((acc + num) * 17) % 256);
        }

        private string[] ReadSequence(string input)
        {
            return input.Split(",");
        }

        
    }
}
