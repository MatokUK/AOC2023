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
using System.Drawing;

namespace MatejTestingConsole
{
    public class Day10: AdventSolution
    {

        sealed class PathCell
        {
            public readonly (int x, int y) pos;
            public readonly (int x, int y) prev;
            public readonly int length;

            public PathCell((int, int) pos, (int, int) prev, int length)
            {
                this.pos = pos;
                this.prev = prev;
                this.length = length;
            }
        }

        sealed class Grid
        {
            private int rows;
            private int cols;
            private char[,] map;

            private List<PathCell> openPositons = new();
            private Dictionary<(int x, int y), int> visited = new();


            public long Max
            {
                get
                {
                    return visited.Last().Value;
                }
            }
                            
            public Grid(char[,] map, int rows, int cols)
            {
                this.map = map;
                this.rows = rows;
                this.cols = cols;            
            }

            public void Evaluate()
            {
                // part I:
                (int startX, int startY) = getStart();
                var startingMoves = getNeighboards(startX, startY);

                foreach (var item in startingMoves)
                {
                    openPositons.Add(new PathCell(item, (startX, startY), 1));
                }

                do
                {
                    var path = openPositons.First();
                    openPositons.Remove(openPositons.First());

                    if (visited.ContainsKey(path.pos))
                    {
                        continue;
                    }
                    visited.Add(path.pos, path.length);   

                    var n = getNextMove(path.pos.x, path.pos.y, path.prev);

                    if (!visited.ContainsKey(n))
                    {
                        openPositons.Add(new PathCell(n, path.pos, path.length + 1));
                    }
                } while (openPositons.Count > 0);

                Console.WriteLine(this.ToString());
                // part II:

                var dots = ReadUnvisitedCells();

                foreach (var dot in dots)
                {
                    Console.WriteLine(dot);
                }

            }

            private (int, int) getStart() 
            { 
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (map[r, c] == 'S')
                        {
                            return (r, c);
                        }
                    }
                }

                return (0, 0);
            }

            private List<(int, int)> getNeighboards(int r, int c, (int x, int y)? prev = null)
            {
                List<(int, int)> pos = new List<(int, int)>();

                if (r > 0 && (map[r - 1, c] == '|' || map[r - 1, c] == '7' || map[r - 1, c] == 'J'))
                {
                    pos.Add((r - 1, c));
                }

                if (map[r + 1, c] == '|' || map[r + 1, c] == 'L' || map[r + 1, c] == 'F')
                {
                    pos.Add((r + 1, c));
                }

                if (c > 0 &&( map[r, c - 1] == '-' || map[r, c - 1] == 'L' || map[r, c - 1] == 'F'))
                {
                    pos.Add((r, c - 1));
                }


                if (map[r, c + 1] == '-' || map[r, c + 1] == '7' || map[r, c + 1] == 'J')
                {
                    pos.Add((r, c + 1));
                }

                if (prev != null)
                {
                    pos = pos.Where(x => x != prev).ToList();
                }
                
                return pos.Where(x => map[x.Item1, x.Item2] != '.').ToList();
            }

            private (int, int) getNextMove(int r, int c, (int r, int c) prev)
            {
                return map[r, c] switch
                {
                    '|' => (prev.r + 1 == r ? r + 1 : r - 1, c),
                    '-' => (r, prev.c + 1 == c ? c + 1 : c - 1),
                    'L' => prev.c == c ? (r, c + 1) : (r - 1, c),
                    'J' => prev.c == c ? (r, c - 1) : (r - 1, c),
                    'F' => prev.c == c ? (r, c + 1) : (r + 1, c),
                    _ => prev.c == c ? (r, c - 1) : (r + 1, c),
                };                
            }

            private List<(int, int)> ReadUnvisitedCells()
            {
                List<(int x, int y)> dots = new();

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (!visited.ContainsKey((r, c)))
                        {
                            dots.Add((r, c));
                        }
                    }
                }

                // filter out border dots:
                return dots.Where(coord => coord.x > 0 && coord.y > 0 && coord.x < rows - 1 && coord.y < cols - 1).ToList();

            }

            public override string ToString()
            {
                string o = "";

                for (int r = 0; r < rows; r++)
                {
                    o += r.ToString().PadLeft(4) + ": ";
                    for (int c = 0; c < cols; c++)
                    {

                        if (visited.ContainsKey((r, c)))
                        {
                            var color = GetGradientColor(visited[(r, c)], 5000);
                            string setColorCode = $"\u001b[38;5;{(int)color}m";

                            // ANSI escape code for resetting color
                            string resetColorCode = "\u001b[0m";
                            o += setColorCode + (visited[(r, c)] % 10).ToString() + resetColorCode;
                        }
                        else
                        {
                            o += map[r, c];
                        }                        
                    }
                    o += "\n";
                }

                return o;
            }
        }

        static ConsoleColor GetGradientColor(int step, int totalSteps)
        {
            int startR = 0;
            int startG = 0;
            int startB = 255; // Blue

            int endR = 0;
            int endG = 0;
            int endB = 0; // Darkest Blue

            int currentR = (int)Math.Round(startR + (endR - startR) * (double)step / totalSteps);
            int currentG = (int)Math.Round(startG + (endG - startG) * (double)step / totalSteps);
            int currentB = (int)Math.Round(startB + (endB - startB) * (double)step / totalSteps);

            ConsoleColor interpolatedColor = (ConsoleColor)((currentR << 4) | currentG << 2 | currentB);

            return interpolatedColor;
        }

        public (long, long) Execute(string[] lines)
        {
            char[,] map = ReadMap(lines);
            long sum2 = 0;

            Grid grid = new Grid(map, lines.Length, lines[0].Length);
            grid.Evaluate();


            return (grid.Max, sum2);
        }


        private char[,] ReadMap(string[] lines)
        {
            char[,] map = new char[lines.Length, lines[0].Length];

            for (int r = 0; r < lines.Length; ++r)
            {
                for (int c = 0; c < lines[r].Length; ++c)
                {
                    map[r, c] = lines[r][c];
                }
            }

            return map;
        }
    }
}
