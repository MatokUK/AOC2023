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
    public class Day17: AdventSolution
    {
        class Path: IComparable<Path> 
        {
            public static int maxRow;
            public static int maxCol;

            public readonly int heat;


            private int streakRow = 0;
            private int streakCol = 0;

            private int actRow;
            private int actCol;
            private Dictionary<(int, int), bool> visited = new();


            private Path? prev;

            public Path(int row, int col, int heat, Path? prev = null)
            {
                this.actRow = row;
                this.actCol = col;
                this.heat = heat + (prev != null ? prev.heat : 0);
                this.prev = prev;
                if (prev != null)
                {
                    this.visited = new (prev.visited);

                    if (row == prev.actRow)
                    {
                        streakRow = prev.streakRow + 1;
                        streakCol = 0;
                    }
                    else if (col == prev.actCol)
                    {
                        streakRow = 0;
                        streakCol = prev.streakCol + 1;
                    }

                }

                visited.Add((row, col), true);
            }

            public List<(int, int)> NextMoves()
            {
                List<(int, int)> next = new();

               /* if (prev == null)
                {
                    next.Add((actRow + 1, actCol));
                    next.Add((actRow, actCol + 1));
                }
                else
                {*/
                    if (actRow + 1 < maxRow && streakCol <= 3)
                    {
                        if (!visited.ContainsKey((actRow + 1, actCol)))
                        {
                            next.Add((actRow + 1, actCol));
                        }
                    }

                    if (actCol + 1 < maxCol && streakRow <= 3)
                    {
                        if (!visited.ContainsKey((actRow, actCol + 1)))
                        {
                            next.Add((actRow, actCol + 1));
                        }
                    }

                    if (actRow > 0 && streakCol <= 3)
                    {
                        if (!visited.ContainsKey((actRow - 1, actCol)))
                        {
                            next.Add((actRow - 1, actCol));
                        }
                    }                    

                    if (actCol > 0 && streakRow <= 3)
                    {
                        if (!visited.ContainsKey((actRow, actCol - 1)))
                        {
                            next.Add((actRow, actCol - 1));
                        }
                    }

              //      RemoveVisited2(next);
                  //  RemoveVisited(next);
                   // RemoveStrait(next);
              //  }

                return next;
            }

            private void RemoveVisited2(List<(int, int)> moves)
            {
                for (int i = moves.Count - 1; i >= 0; --i)
                {
                    if (visited.ContainsKey((moves[i].Item1, moves[i].Item2)))
                    {
                        moves.RemoveAt(i);
                    }
                }
            }

          /*  private void RemoveVisited(List<(int, int)> moves)
            {
                if (this.prev == null)
                {
                    return;
                }

                moves.Remove((prev.actRow, prev.actCol));

                prev.RemoveVisited(moves);
            }*/

            private void RemoveStrait(List<(int, int)> moves)
            {
                if (this.prev == null)
                {
                    return;
                }

                if (this.prev.prev == null)
                {
                    return;
                }

                if (this.actRow == this.prev.actRow && this.actRow == prev.prev.actRow)
                {
                  /*  for (int idx = 0; idx < moves.Count; idx++)
                    {
                        if (moves[idx].Item1 == this.actRow)
                        {
                            moves.RemoveAt(idx);
                        }
                    }*/
                }

                /*if (this.actCol == this.prev.actCol && this.actCol == prev.prev.actCol)
                {
                    for (int idx = 0; idx < moves.Count; idx++)
                    {
                        if (moves[idx].Item2 == this.actCol)
                        {
                            moves.RemoveAt(idx);
                        }
                    }
                }*/

            }

            public bool IsFinished()
            {
                return actCol == maxCol - 1 && actRow == maxRow - 1;
            }

            public string ToString()
            {
                return "[" + actRow + "," + actCol + "]" + (prev != null ? " -> " + prev.ToString() : "") ;
            }

            public int CompareTo(Path? other)
            {
                return heat.CompareTo(other.heat);
            }
        }

        class HeatMap
        {
            private List<List<int>> map = new();
            private Path? shortestPath = null;
            private List<Path> paths = new();

            public HeatMap(string[] lines) 
            { 
                foreach (string line in lines)
                {
                    map.Add(line.Select(c => int.Parse(c.ToString())).ToList());
                }

                paths.Add(new Path(0, 0, map[0][0]));
                Path.maxRow = map.Count;
                Path.maxCol = map[0].Count;
            }

            public long ShortestPath()
            {
                do
                {
                    Path path = paths.Last();
                    paths.RemoveAt(paths.Count - 1);

                   // Console.WriteLine(path.ToString());

                    if (path.IsFinished())
                    {

                       // Console.WriteLine(":FINISHED " + path.ToString());
                        Console.WriteLine("Rest " + paths.Count);

                        if (shortestPath == null || shortestPath.heat > path.heat)
                        {
                            //this.finished.Add(path);
                            shortestPath = path;

                            for (int i = paths.Count - 1; i >= 0; --i)
                            {
                                if (paths[i].heat >= shortestPath.heat)
                                {
                                    paths.RemoveAt(i);
                                }
                            }
                        }
                        //var mmm = this.finished.Min();

                        Console.WriteLine("min " + shortestPath.heat);
                    }
                    else// if (shortestPath == null || shortestPath.heat > path.heat)
                    {
                        List<(int, int)> moves = path.NextMoves();

                        foreach (var move in moves)
                        {
                            var np = new Path(move.Item1, move.Item2, map[move.Item1][move.Item2], path);
                            if (shortestPath == null || np.heat < shortestPath.heat)
                            { 
                                paths.Add(np);
                            }
                        }
                    }

                    if (shortestPath != null && shortestPath.heat <= 140)
                    {
                        Console.WriteLine("REmaining: " + paths.Count);
                    }




                } while (paths.Count > 0);



                Console.WriteLine("TA-daaaaaa");
                Console.WriteLine(this.shortestPath.ToString());
                Console.WriteLine(this.shortestPath.heat);

                return 6;
            }   
           
        }

        public (long, long) Execute(string[] lines)
        {
            long max = 0;

            HeatMap map = new(lines);


            return (map.ShortestPath(), max);
        }
    }
}
