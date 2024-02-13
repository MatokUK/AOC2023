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
    public class Day11: AdventSolution
    {
        class Universe
        {
            private int rows;
            private int cols;
            private char[,] map;

            private List<(int, int)> galaxies = new List<(int, int)> ();
            private List<int> emptyRows = new List<int>();
            private List<int> emptyCols = new List<int>();

            public Universe(char[,] map, int rows, int cols)
            {
                this.map = map;
                this.rows = rows;
                this.cols = cols;

                galaxies = ReadGalaxies();
                emptyRows = ReadEmptyRows();
                emptyCols = ReadEmptyCols();
            }

            private List<(int, int)> ReadGalaxies() {
                List<(int, int)> galaxies = new List<(int, int)>();

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (map[r, c] == '#')
                        {
                            galaxies.Add((r, c));
                        }
                    }
                }

                return galaxies;
            }

            private List<int> ReadEmptyRows()
            {
                List<int> empty = new List<int>();
                
                for (int r = 0; r < rows; r++)
                {
                    bool isEmpty = true;
                    for (int c = 0; c < cols; c++)
                    {
                        if (map[r, c] != '.')
                        {
                            isEmpty = false;
                        }
                    }

                    if (isEmpty)
                    {
                        empty.Add(r);
                    }
                }
             
                return empty;
            }

            private List<int> ReadEmptyCols()
            {
                List<int> empty = new List<int>();

                for (int c = 0; c < cols; c++)
                {
                    bool isEmpty = true;
                    for (int r = 0; r < rows; r++)
                    {
                        if (map[r, c] != '.')
                        {
                            isEmpty = false;
                        }
                    }

                    if (isEmpty)
                    {
                        empty.Add(c);
                    }
                }

                return empty;
            }

            public long getShortestPaths(int expand)
            {
                long sum = 0;

                for (int i = 0; i < galaxies.Count; i++)
                {
                    for (int j = i + 1; j < galaxies.Count; j++)
                    {
                        sum += getShortestPath(galaxies[i], galaxies[j], expand); 
                    }
                }

                return sum;
            }


            private long getShortestPath((int, int) g1, (int, int) g2, int expand)
            {

                int rowDistance = Math.Abs(g1.Item1 - g2.Item1);
                int colDistance = Math.Abs(g1.Item2 - g2.Item2);

                /*   Console.WriteLine($"getting {g1} - {g2} R: {rowDistance} C: {colDistance}");
                   Console.WriteLine("rows " + getExpandedRows(g1.Item1, g2.Item1));
                   Console.WriteLine("cols " + getExpandedCols(g1.Item2, g2.Item2));
                   Console.WriteLine($"RESUOLT {rowDistance + getExpandedRows(g1.Item1, g2.Item1) + colDistance + getExpandedCols(g1.Item2, g2.Item2)}");
                */
                return rowDistance + getExpandedRows(g1.Item1, g2.Item1, expand) + colDistance + getExpandedCols(g1.Item2, g2.Item2, expand);
            }

            private int getExpandedRows(int r1, int r2, int expandValue)
            {
                int expanded = 0;

                int low = r1 < r2 ? r1 : r2;
                int high = r1 > r2 ? r1 : r2;

                for (int r =low; r <= high; r++)
                {
                    if (emptyRows.Contains(r))
                    {
                        expanded += expandValue;
                    }
                }

                return expanded;
            }

            private int getExpandedCols(int c1, int c2, int expandValue)
            {
                int expanded = 0;

                int low = c1 < c2 ? c1 : c2;
                int high = c1 > c2 ? c1 : c2;

                for (int c = low; c <= high; c++)
                {
                    if (emptyCols.Contains(c))
                    {
                        expanded += expandValue;
                    }
                }

                return expanded;
            }
        }
        public (long, long) Execute(string[] lines)

        {
            char[,] map = ReadMap(lines);

            Universe universe = new Universe(map, lines.Length, lines[0].Length);

            return (universe.getShortestPaths(1), universe.getShortestPaths(1000000 - 1));
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
