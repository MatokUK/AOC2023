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
    public class Day18: AdventSolution
    {
       enum Direction: byte
        {
            R,
            L,
            U,
            D
        }


        record DigInstruction
        {
            public Direction direction { get; init; }
            public int size { get; init;  }
            public string color { get; init; }
        }

        class Terrain
        {
            private List<List<char>> map = new List<List<char>>();
            private int x = 180;
            private int y = 180;

            private List<(int, int)> coordToVisit = new();
            private int volume = 0;

            public Terrain(int rows, int cols) 
            { 
                for (int r = 0; r < rows; r++)
                {
                    map.Add(Enumerable.Repeat('.', cols).ToList());
                }
            }

            public void Dig(DigInstruction instruction)
            {
               switch (instruction.direction) 
               {
                    case Direction.R:
                        for (int i = 0; i < instruction.size; ++i)
                        {
                            map[x][y++] = '#';
                        }

                        break; 
                    case Direction.L:
                        for (int i = 0; i < instruction.size; ++i)
                        {
                            map[x][y--] = '#';
                        }
                        break;
                    case Direction.U:
                        for (int i = 0; i < instruction.size; ++i)
                        {
                            map[x--][y] = '#';
                        }
                        break;
                    case Direction.D:
                        for (int i = 0; i < instruction.size; ++i)
                        {
                            map[x++][y] = '#';
                        }
                        break;
               }
            }

            public int GetVolume()
            {
                /*  List<List<(char, int)>> rowsPacket = new();

                  for (int r = 0; r < map.Count; ++r)
                  {
                      List<(char, int)> rowPacked = new();
                      char act = map[r][0];
                      int size = 1;
                      for (int c = 1; c < map[r].Count; ++c)
                      {
                          if (act == map[r][c])
                          {
                              size++;
                          }
                          else
                          {
                              rowPacked.Add((act, size));
                              act = map[r][c];
                              size = 1;
                          }
                      }
                      Console.WriteLine(rowPacked);
                      rowsPacket.Add(rowPacked);
                  }
                  return;*/


                //  Visit(30, 300);

                coordToVisit.Add((181, 181));
                Visit();

                int volume = 0; 
                foreach (var row in map)
                {
                   volume += row.FindAll(x => x == '#' || x == 'X').Count();
                }

                return volume;
            }

            public void Visit()
            {
                do
                {
                    (int r, int c) = coordToVisit.First();
                    coordToVisit.RemoveAt(0);

                    if (r < 0 || c < 0)
                    {
                        continue;
                    }

                    if (r >= map.Count || c >= map[0].Count)
                    {
                        continue;
                    }

                    if (map[r][c] != '.')
                    {
                        continue;
                    }

                    map[r][c] = 'X';
                  //  volume++;

                    coordToVisit.Add((r + 1, c));
                    coordToVisit.Add((r - 1, c));
                    coordToVisit.Add((r, c + 1));
                    coordToVisit.Add((r, c - 1));

                } while (coordToVisit.Count > 0);
            }

            public override string ToString()
            {
                string output = "";

                foreach (var line in map)
                {
                    output += new string(line.ToArray()) + "\n";
                }


                return output;
            }
        }

        public (long, long) Execute(string[] lines)
        {
            long max = 0;

            List <DigInstruction> instructions = ReadInstructions(lines);

            Terrain terrain = new(430, 470);

            foreach (var instruction in instructions)
            {                
                terrain.Dig(instruction);
            }

          

            
            Console.WriteLine(terrain.ToString());


            return (terrain.GetVolume(), max);
        }

        private List<DigInstruction> ReadInstructions(string[] lines)
        {
            List<DigInstruction> outpupt = new();

            foreach (string line in lines)
            {
                var parts = line.Split(' ');

                Enum.TryParse<Direction>(parts[0], out var direction);

                int.TryParse(parts[1], out var meters);

                outpupt.Add(new DigInstruction() { direction = direction, size = meters, color = parts[2] });
            }

            return outpupt;
        }
    }
}
