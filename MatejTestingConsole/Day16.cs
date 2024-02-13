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
    public class Day16: AdventSolution
    {
        enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        class Light: IEquatable<Light>
        {
            public readonly int row;
            public readonly int col;
            public readonly Direction direction;

            public Light(int row, int col, Direction direction)
            {
                this.row = row;
                this.col = col;
                this.direction = direction;
            }

            public bool Equals(Light? other)
            {
                return this.col == other.col && this.row == other.row && this.direction == other.direction;
            }

            public (int, int) Move()
            {
                if (direction == Direction.Left)
                {
                    return (row, col - 1);
                }

                if (direction == Direction.Right)
                {
                    return (row, col + 1);
                }

                if (direction == Direction.Up)
                {
                    return (row - 1, col);
                }

                return (row + 1, col);
            }
        }

        class Area
        {
            private List<string> rows = new();
            private List<Light> lights = new();
            private List<string> lightSquares = new();
            private List<Light> lightMoves = new();

            public Area(string[] lines) 
            { 
                foreach (string line in lines)
                {
                    rows.Add(line);
                }
            }

            public void EntryLight(Light light)
            {
                this.lights.Add(light);
            }

            public void Iterate()
            {
                Light light = lights.First();
                lights.RemoveAt(0);

                if (!lightSquares.Contains(light.row + "/" + light.col) && light.col >= 0 && light.row >= 0)
                { 
                    lightSquares.Add(light.row + "/" + light.col);
                }

                List<Light> ls = Step(light);
                foreach (Light l in ls)
                {
                    if (!lightMoves.Contains(l))
                    {
                        lightMoves.Add(l);
                        lights.Add(l);
                        
                    }
                }               
            }

            private List<Light> Step(Light light)
            {
                List<Light> lights = new();
               
                char square;
                (var r, var c) = light.Move();
                try
                {
                    
                    square = rows[r][c];
                }
                catch (IndexOutOfRangeException)
                {
                    return lights;
                }
                catch(ArgumentOutOfRangeException)
                {
                    return lights;
                }
                        

                if (square == '.' || (square == '-' && (light.direction == Direction.Left || light.direction == Direction.Right)) || (square == '|' && (light.direction == Direction.Up || light.direction == Direction.Down)))
                {
                    lights.Add(new Light(r, c, light.direction));
                }
                else if (square == '|' && (light.direction == Direction.Left || light.direction == Direction.Right))
                {
                    lights.Add(new Light(r, c, Direction.Down));
                    lights.Add(new Light(r, c, Direction.Up));
                }
                else if (square == '-' && (light.direction == Direction.Up || light.direction == Direction.Down))
                {
                    lights.Add(new Light(r, c, Direction.Right));
                    lights.Add(new Light(r, c, Direction.Left));
                } 
                else if (square == '/' && light.direction == Direction.Right)
                {
                    lights.Add(new Light(r, c, Direction.Up));
                }
                else if (square == '/' && light.direction == Direction.Left)
                {
                    lights.Add(new Light(r, c, Direction.Down));
                }
                else if (square == '/' && light.direction == Direction.Up)
                {
                    lights.Add(new Light(r, c, Direction.Right));
                }
                else if (square == '/' && light.direction == Direction.Down)
                {
                    lights.Add(new Light(r, c, Direction.Left));
                }
                else if (square == '\\' && light.direction == Direction.Right)
                {
                    lights.Add(new Light(r, c, Direction.Down));
                }
                else if (square == '\\' && light.direction == Direction.Left)
                {
                    lights.Add(new Light(r, c, Direction.Up));
                }
                else if (square == '\\' && light.direction == Direction.Up)
                {
                    lights.Add(new Light(r, c, Direction.Left));
                }
                else if (square == '\\' && light.direction == Direction.Down)
                {
                    lights.Add(new Light(r, c, Direction.Right));
                }

                for (int idx = 0; idx < lights.Count; idx ++)
                {
                    if (lights[idx].row < 0 || lights[idx].col < 0)
                    {
                        lights.RemoveAt(idx);
                    }
                    else if (lights[idx].row > this.rows.Count - 1 || lights[idx].col > this.rows[0].Length - 1)
                    {
                        lights.RemoveAt(idx);
                    }

                    if (!lightSquares.Contains(lights[idx].row + "/" + lights[idx].col))
                    {
                        lightSquares.Add(r + "/" + c);
                    }
                }

                return lights;
            }

            public bool HasNextMove()
            {
                return lights.Count > 0;
            }

            public int GetLitSquares()
            {
                return this.lightSquares.Count;
            }

            public string Lit()
            {
                string output = "";
                for (int r  = 0; r < rows.Count; ++r) {
                    if (r < 10)
                    {
                        output += " ";
                    }
                    output += (r+1) + ": ";
                    for (int c = 0; c < rows[r].Length; ++c)
                    {
                        var key = r + "/" + c;

                        if (this.lightSquares.Contains(key))
                        {
                            output += '#';
                        } else
                        {
                            output += rows[r][c];
                        }
                    }
                    output += "\n";
                }

                return output;
            }
        }

       

        public (long, long) Execute(string[] lines)
        {
            
            long max = 0;
            long act = 0;
            long first = 0;


            for (int r = 0; r < lines.Length; ++r)
            {
                Area area = new(lines);
                area.EntryLight(new Light(r, -1, Direction.Right));
                do
                {
                    area.Iterate();
                } while (area.HasNextMove());

                if (first == 0)
                {
                    first = area.GetLitSquares();
                }

                if (max < area.GetLitSquares())
                {
                    max = area.GetLitSquares();
                }

                Console.WriteLine(r);
            }

            Console.WriteLine("----------------------------------------------------------------------------------");


            for (int r = 0; r < lines.Length; ++r)
            {
                Area area = new(lines);
                area.EntryLight(new Light(r, lines[0].Length, Direction.Left));
                do
                {
                    area.Iterate();
                } while (area.HasNextMove());

                if (first == 0)
                {
                    first = area.GetLitSquares();
                }

                if (max < area.GetLitSquares())
                {
                    max = area.GetLitSquares();
                }

                Console.WriteLine(r);
            }

            Console.WriteLine("----------------------------------------------------------------------------------");


            for (int c = 0; c < lines[0].Length; ++c)
            {
                Area area = new(lines);
                area.EntryLight(new Light(-1, c, Direction.Down));
                do
                {
                    area.Iterate();
                } while (area.HasNextMove());

                if (first == 0)
                {
                    first = area.GetLitSquares();
                }

                if (max < area.GetLitSquares())
                {
                    max = area.GetLitSquares();
                }

                Console.WriteLine(c);
            }
            Console.WriteLine("----------------------------------------------------------------------------------");

            for (int c = 0; c < lines[0].Length; ++c)
            {
                Area area = new(lines);
                area.EntryLight(new Light(lines.Length, c, Direction.Up));
                do
                {
                    area.Iterate();
                } while (area.HasNextMove());

                if (first == 0)
                {
                    first = area.GetLitSquares();
                }

                if (max < area.GetLitSquares())
                {
                    max = area.GetLitSquares();
                }

                Console.WriteLine(c);
            }


            //            int x = 0;
            //do
            //{
            //  area.Iterate();
            /*
                            if (x % 1000 == 0) { 
                                Console.WriteLine(area.Lit());
                                Console.WriteLine();
                            }
                            x++;*/
            //} while (area.HasNextMove());




            return (first, max);
        }
    }
}
