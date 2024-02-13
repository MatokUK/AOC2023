using System.Collections.Generic;
using System.IO;

namespace MatejTestingConsole
{
    public class Day23: AdventSolution
    {
        sealed class HikingMap
        {
            private List<string> map;
            private List<(int, int)> intersections = new();
            private List<HikingPath> paths = new();
            private List<HikingPath> beginPaths = new();
            private List<HikingPath> endPaths = new();
            private HikingPath longestPath = null;
            private (int x, int y) start;
            private (int x, int y) end;            

            public HikingMap(string[] rows)
            {
                map = new List<string>(rows);

                start = FindPath(0);
                end = FindPath(map.Count - 1);

                ReadIntersection();

                (int, int) FindPath(int row)
                {
                    return (row, map[row].IndexOf('.'));
                }

                void ReadIntersection()
                {
                    for (int r = 1; r < map.Count - 1; r++)
                    { 
                        for (int c = 1; c < map[r].Length - 1; c++)
                        {
                            if (map[r][c] != '#')
                            {
                                List<(int x, int y)> moves = new() { (r + 1, c), (r - 1, c), (r, c + 1), (r, c - 1) };
                                if (moves.Select(coords => map[coords.x][coords.y] != '#').Count(condition => condition) > 2)
                                {
                                    intersections.Add((r, c));
                                }
                            }
                        }
                    }
                }
            }

            public HikingPath LongestPath(bool considerSlopes = true)
            {
                paths.Clear();

                paths.Add(new HikingPath(start));

                do
                {
                    var p = paths.ElementAt(0);
                    paths.RemoveAt(0);
                    Console.WriteLine(" - " + paths.Count);

                    bool validMoveOnPath;

                    do
                    {
                        var moves = PossibleMoves(p.CurrentX, p.CurrentY, p.previousMove, considerSlopes);
                        var firstMode = moves.ElementAt(0);
                        moves.RemoveAt(0);
                        validMoveOnPath = p.Move(firstMode);                            
                        foreach (var move in moves)
                        {
                            HikingPath cloned = (HikingPath)p.Clone();
                            if (cloned.Move(move))
                            {
                                paths.Add(cloned);
                                Console.WriteLine(" + " + paths.Count);
                            }
                        }

                        if (p.IsOnCell(end))
                        {
                            PathFinished(p);
                            //Console.WriteLine("FINISHJED " + finishedPaths.Count);
                        }
                    } while (validMoveOnPath && !p.IsOnCell(end));
                } while(paths.Count > 0);                    

                return longestPath;
            }

            public HikingPath LongestPathIntersections(bool considerSlopes = true)
            {
                var firtIntersection = ExpandPath(new HikingPath(start));
                var lastIntersection = ExpandPath(new HikingPath(end));

                Dictionary<(int x, int y), List<CompressPath>> results = new();


                var moves = PossibleMoves(firtIntersection.CurrentX, firtIntersection.CurrentY, firtIntersection.previousMove, false);

                foreach (var move in moves)
                {
                    var hp = new HikingPath(move, (firtIntersection.CurrentX, firtIntersection.CurrentY));
                    var lolo = ExpandPath(hp);

                    Console.WriteLine(lolo);

                    var key = (firtIntersection.CurrentX, firtIntersection.CurrentY);

                    if (!results.ContainsKey(key))
                    {
                        results[key] = new List<CompressPath>();
                    }

                    results[key].Add(new CompressPath((lolo.CurrentX, lolo.CurrentY), lolo.Length));
                }
                //(int x, int y) a = ExpandPath(new HikingPath(firstIntersection));


                return longestPath;
            }

            private HikingPath ExpandPath(HikingPath path)
            {
                do
                {
                    var moves = PossibleMoves(path.CurrentX, path.CurrentY, path.previousMove, false);
                    var firstMode = moves.ElementAt(0);
                    moves.RemoveAt(0);
                    path.Move(firstMode);
                } while (!path.IsOnCell(intersections));

                //beginPaths.Add(path);

                return path;
                //return (path.CurrentX, path.CurrentY, path.previousMove);
            }

            private void ExpandBeginPath()
            {
                var path = new HikingPath(start);

                do
                {
                    var moves = PossibleMoves(path.CurrentX, path.CurrentY, path.previousMove, false);
                    var firstMode = moves.ElementAt(0);
                    moves.RemoveAt(0);
                    path.Move(firstMode);
                } while (!path.IsOnCell(intersections));

                beginPaths.Add(path);

            }

            private void ExpandEndPath()
            {
                var path = new HikingPath(end);

                do
                {
                    var moves = PossibleMoves(path.CurrentX, path.CurrentY, path.previousMove, false);
                    var firstMode = moves.ElementAt(0);
                    moves.RemoveAt(0);
                    path.Move(firstMode);
                } while (!path.IsOnCell(intersections));

                endPaths.Add(path);
            }


            private List<(int x, int y)> PossibleMoves(int r, int c, (int, int) previousMove, bool considerSlopes)
            {
                List<(int x, int y)> allMoves = new(){(r + 1, c), (r - 1, c) , (r, c + 1) , (r, c -1) };
                allMoves.Remove(previousMove);
                List<(int x, int y)> moves = new();

                foreach (var move in allMoves)
                {
                    if (move.x < 0 || move.y < 0)
                    {
                        continue;
                    }

                    if (move.x >= map.Count || move.y >= map[0].Length)
                    {
                        continue;
                    }

                    if (map[move.x][move.y] == '#')
                    {
                        continue;
                    }

                    if (considerSlopes)
                    {
                        if (map[move.x][move.y] == 'v' && r + 1 != move.x)
                        {
                            continue;
                        }

                        if (map[move.x][move.y] == '^' && r - 1 != move.x)
                        {
                            continue;
                        }

                        if (map[move.x][move.y] == '>' && c + 1 != move.y)
                        {
                            continue;
                        }

                        if (map[move.x][move.y] == '<' && c - 1 != move.y)
                        {
                            continue;
                        }
                    }

                    moves.Add(move);
                }

                return moves;
            }

            private void PathFinished(HikingPath path)
            {
                if (longestPath == null || longestPath.Length < path.Length)
                {
                    longestPath = path;
                }
            }

            public void Visualise(HikingPath path)
            {
                string row = "";

                int c = 1;

                for (int x = 0; x < map.Count; ++x)
                {
                    for (int y = 0; y < map[x].Length; ++y)
                    {
                        if (path.Has((x, y)))
                        {
                            row += path.Index((x, y));
                        }
                        else
                        {
                            row += map[x][y];
                        }                        
                    }

                    Console.WriteLine(row);
                    row = "";
                }
            }
        }

        sealed class HikingPath: ICloneable
        {
            private (int x, int y) cell;
            private List<(int, int)> prevMoves = new();
         
            public int CurrentX => cell.x;
            public int CurrentY => cell.y;
            public int Length => prevMoves.Count;
            public (int, int) previousMove => prevMoves.Count > 0 ? prevMoves.Last() : (0,0);

            public HikingPath((int x, int y) cell)
            {
                this.cell = cell;
            }

            public HikingPath((int x, int y) cell, (int x, int y) prev)
            {
                this.cell = cell;
                prevMoves.Add(prev);
            }

            public bool Move((int x, int y) next)
            {
                if (prevMoves.Contains(next))
                {
                    return false;
                }

                prevMoves.Add(cell);
                cell = next;

                return true;
            }

            public bool Has((int x, int y) cell)
            {
                return prevMoves.Contains(cell);
            }

            public int Index((int x, int y) cell)
            {
                return prevMoves.IndexOf(cell) % 10;
            }

            public bool IsOnCell((int, int) cell)
            {
                return cell == this.cell;
            }

            public bool IsOnCell(List<(int, int)> cells)
            {
                return cells.Contains(this.cell);
            }

            public object Clone()
            {
                HikingPath cloned = new HikingPath(prevMoves.Last());
                cloned.prevMoves = new List<(int, int)>(prevMoves);
                cloned.prevMoves.RemoveAt(cloned.prevMoves.Count - 1);

                return cloned;
            }
        }

        sealed class CompressPath
        {
            public readonly int length;
            public readonly (int x, int y) endPos;

            public CompressPath((int, int) endPos, int length)
            {
                this.endPos = endPos;
                this.length = length;
            }
        }


        public (long, long) Execute(string[] lines)
        {
            HikingMap map = new HikingMap(lines);
            HikingPath pathA = map.LongestPath();
            Console.WriteLine("part II");
                 HikingPath pathB = map.LongestPathIntersections(false);
                map.Visualise(pathB);

            //HikingPath pathB = new((1,1));

            return (pathA.Length, pathB.Length);
        }
    }
}


