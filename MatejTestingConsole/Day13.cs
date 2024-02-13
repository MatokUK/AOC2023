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

namespace MatejTestingConsole
{
    public class Day13: AdventSolution
    {
        class Pattern
        {
            private List<string> rows = new();


            public void AddLine(string line)
            {
                rows.Add(line);
            }

            public int RowReflection(int smudge = 0)
            {
                return CalculateReflection(new List<string>(rows), smudge);
            }

            public int ColReflection(int smudge = 0)
            {
                return CalculateReflection(new List<string>(Rotate(rows)), smudge);
            }

            private int CalculateReflection(List<string> mirror, int smudge = 0)
            {
                List<string> reflection = new List<string>();
                var max = mirror.Count - 1;

                for (int r = 0; r < max; ++r)
                {
                    reflection.Add(mirror.First());
                    mirror.RemoveAt(0);

                    if (IsRowReflected(new List<string>(reflection), new List<string>(mirror), smudge))
                    {
                        return r + 1;
                    }

                }

                return -1;
            }

            private bool IsRowReflected(List<string> a, List<string> b, int smudge)
            {
                int diffs = 0;

                while (a.Count > 0 && b.Count > 0)
                {
                    var rowA = a.Last();
                    var rowB = b.First();

                    a.RemoveAt(a.Count - 1);
                    b.RemoveAt(0);

                    diffs += RowsDiffs(rowA, rowB);


                    if (diffs > smudge)
                    {
                        return false;
                    }
                }

                return diffs == smudge;
            }

            private int RowsDiffs(string a, string b)
            {
                return a.Zip(b, (c1, c2) => c1 != c2 ? 1 : 0).Sum();
            }

            private List<string> Rotate(List<string> arr)
            {
                List<string> rotated = new();
                string column = "";

                for (int idx = 0;  idx < arr[0].Length; ++idx) 
                {
                    foreach (var row in arr)
                    {
                        column += row[idx];
                    }

                    rotated.Add(column);
                    column = "";
                }


                return rotated;
            }

            public string ToString()
            {
                string output = "";

                foreach (var row in rows)
                {
                    output += row.ToString() + "\n";
                }

                return output;
            }
        }

        public (long, long) Execute(string[] lines)
        {
            long part1 = 0;
            long part2 = 0;
            List<Pattern> patterns = ReadPatterns(lines);

            foreach (var pattern in patterns)
            {
                var rows =  pattern.RowReflection();
                var cols = pattern.ColReflection();
                if (rows != -1)
                {
                    part1 += rows * 100;
                } 
                else
                {                                       
                    part1 += cols;
                }


                rows = pattern.RowReflection(1);
                cols = pattern.ColReflection(1);

                if (rows != -1)
                {
                    part2 += rows * 100;
                }
                else
                {
                    part2 += cols;
                }
            }

            return (part1, part2);
        }

        private List<Pattern> ReadPatterns(string[] lines)
        {
            List<Pattern> patterns = new List<Pattern>();
            Pattern pattern = new();

            foreach (var line in lines)
            {

                if (line.Length == 0)
                {
                    patterns.Add(pattern);
                    pattern = new();
                }
                else
                {
                    pattern.AddLine(line);
                }            
            }

            patterns.Add(pattern);

            return patterns;
        }
    }
}
