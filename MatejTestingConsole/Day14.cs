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

namespace MatejTestingConsole
{
    public class Day14: AdventSolution
    {
        class Platform
        {
            private List<string> rows = new();

            public void AddLine(string line)
            {
                rows.Add(line);
            }

            public void TiltNorth()
            {
                for (int r = 0; r < rows.Count; r++)
                {
                    for (int c = 0; c < rows[0].Length; c++)
                    {
                        if (rows[r][c] == '.')
                        {
                            (int rockRow, int rockCol) = GetRockPositionDown(r, c);

                            if (rockRow != -1)
                            {
                                var xxx = rows[r].ToCharArray();
                                xxx[c] = 'O';
                                rows[r] = new string(xxx);

                                var yyy = rows[rockRow].ToCharArray();
                                yyy[c] = '.';
                                rows[rockRow] = new string(yyy);
                            }

                        }                        
                    }
                }
            }

            public void TiltWest()
            {
                for (int c = 0; c < rows[0].Length; c++)
                {
                    for (int r = 0; r < rows.Count; r++)
                    {                   
                        if (rows[r][c] == '.')
                        {
                            (int rockRow, int rockCol) = GetRockPositionRight(r, c);

                            if (rockCol != -1)
                            {
                                var xxx = rows[r].ToCharArray();
                                xxx[c] = 'O';
                                xxx[rockCol] = '.';
                              //  rows[r] = new string(xxx);

                                //var yyy = rows[rockRow].ToCharArray();
                               // yyy[c] = '.';
                                rows[rockRow] = new string(xxx);
                            }

                        }
                    }
                }
            }


            public void TiltEast()
            {
                for (int c = rows[0].Length -1; c >=0; --c)
                {
                    for (int r = 0; r < rows.Count; r++)
                    {
                        if (rows[r][c] == '.')
                        {
                            (int rockRow, int rockCol) = GetRockPositioLeft(r, c);

                            if (rockCol != -1)
                            {
                                var xxx = rows[r].ToCharArray();
                                xxx[c] = 'O';
                                xxx[rockCol] = '.';
                                //  rows[r] = new string(xxx);

                                //var yyy = rows[rockRow].ToCharArray();
                                // yyy[c] = '.';
                                rows[rockRow] = new string(xxx);
                            }

                        }
                    }
                }
            }


            public void TiltSouth()
            {
                for (int r = rows.Count - 1; r >= 0; --r)
                {
                    for (int c = 0; c < rows[0].Length; c++)
                    {
                        if (rows[r][c] == '.')
                        {
                            (int rockRow, int rockCol) = GetRockPositionUp(r, c);

                            if (rockRow != -1)
                            {
                                var xxx = rows[r].ToCharArray();
                                xxx[c] = 'O';
                                rows[r] = new string(xxx);

                                var yyy = rows[rockRow].ToCharArray();
                                yyy[c] = '.';
                                rows[rockRow] = new string(yyy);
                            }
                        }
                    }
                }
            }

            private (int, int) GetRockPositionDown(int currentRow, int currentCol)
            {
                for (int r = currentRow + 1; r < rows.Count; ++r)
                {
                    if (rows[r][currentCol] == 'O')
                    {
                        return (r, currentCol);
                    }
                    else if (rows[r][currentCol] == '#')
                    {
                        return (-1, -1);
                    }
                }

                return (-1, -1);
            }

            private (int, int) GetRockPositionUp(int currentRow, int currentCol)
            {
                for (int r = currentRow - 1; r >= 0; --r)
                {
                    if (rows[r][currentCol] == 'O')
                    {
                        return (r, currentCol);
                    }
                    else if (rows[r][currentCol] == '#')
                    {
                        return (-1, -1);
                    }
                }

                return (-1, -1);
            }

            private (int, int) GetRockPositionRight(int currentRow, int currentCol)
            {
                for (int c = currentCol + 1; c < rows[0].Length; ++c)
                {
                    if (rows[currentRow][c] == 'O')
                    {
                        return (currentRow, c);
                    }
                    else if (rows[currentRow][c] == '#')
                    {
                        return (-1, -1);
                    }
                }

                return (-1, -1);
            }

            private (int, int) GetRockPositioLeft(int currentRow, int currentCol)
            {
                for (int c = currentCol - 1; c >= 0; --c)
                {
                    if (rows[currentRow][c] == 'O')
                    {
                        return (currentRow, c);
                    }
                    else if (rows[currentRow][c] == '#')
                    {
                        return (-1, -1);
                    }
                }

                return (-1, -1);
            }

            public int TotalLoad()
            {
                int load = 0;
                int weight = rows.Count;

                for (int r = 0; r < rows.Count; r++)
                {
                    load += rows[r].Count(c => c == 'O') * weight;
                    weight--;
                }

                return load;
            }

            public override string ToString()
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

            Platform platform = new();

            foreach (var line in lines)
            {
                platform.AddLine(line);
            }

        //    platform.TiltNorth();

          //  part1 = platform.TotalLoad();


            for (long i = 0; i < 1000000000; ++i)
            {
                platform.TiltNorth();
                platform.TiltWest();
                platform.TiltSouth();

                platform.TiltEast();

                if (i % 10000 == 0)
                {
                    Console.WriteLine($"{i} iteration");
                }
                //Console.WriteLine(platform.ToString());
            }

            return (part1, platform.TotalLoad());
        }
    }
}
