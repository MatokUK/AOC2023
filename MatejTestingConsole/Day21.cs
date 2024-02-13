namespace MatejTestingConsole
{
    public class Day21: AdventSolution
    {
        sealed class Garden
        {
            private int rows;
            private int columns;

            private (int, int) start;
            private List<string> map = new();
            private List<(int x, int y)> steps = new();

            public int Plots => steps.Count;

            public Garden(string[] lines)
            {
                int row = 0;
                foreach (var line in lines)
                {
                    if (line.Contains('S'))
                    {
                        var col = line.IndexOf('S');
                        start = (row, col);
                        char[] charArray = line.ToCharArray();
                        
                        charArray[col] = '.';

                        // Create a new string from the modified char array
                        map.Add(new string(charArray));
                    }   
                    else
                    {
                        map.Add(line);
                    }

                    
                    row++;
                }

                steps.Add(start);

                rows = map.Count;
                columns = map[0].Length;
            }

            public void step(int count) 
            { 
                for (int idx = 0; idx < count; idx++)
                {
                    List<(int x, int y)> actSteps = new(this.steps);
                    steps.Clear();

                    foreach (var pos in actSteps)
                    {
                      //  Console.WriteLine(pos);
                        if (canStep(pos.x + 1, pos.y))
                        {
                            steps.Add((pos.x + 1, pos.y));
                        }

                        if (canStep(pos.x - 1, pos.y))
                        {
                            steps.Add((pos.x - 1, pos.y));
                        }

                        if (canStep(pos.x, pos.y + 1))
                        {
                            steps.Add((pos.x, pos.y + 1));
                        }

                        if (canStep(pos.x, pos.y - 1))
                        {
                            steps.Add((pos.x, pos.y - 1));
                        }
                    }
                }

                Console.Write("\n");
            }

            private bool canStep(int x, int y)
            {
                if (steps.Contains((x, y)))
                {
                    return false;
                }

                if (x < 0 || y < 0)
                {
                    return false;
                }

                if (x >= rows || y >= columns)
                {
                    return false;
                }

                return map[x][y] == '.';
            }

            public override string ToString()
            {
                string o = "";

                for (int r = 0; r < rows; ++r)
                {
                    for (int c = 0; c < columns; ++c)
                    {
                        o += steps.Contains((r, c)) ? 'O' : map[r][c];

                    }
                    o += "\n";
                }

                return o;
            }
        }

      
        public (long, long) Execute(string[] lines)
        {
            long partI = 0;

            Garden garden = new(lines);
            garden.step(16);

            /*garden.step(145);
            Console.WriteLine(garden.ToString());
            Console.WriteLine(garden.Plots);

            garden = new(lines);
            garden.step(146);
            Console.WriteLine(garden.ToString());
            Console.WriteLine(garden.Plots);

            garden = new(lines);
            garden.step(147);*/
            Console.WriteLine(garden.ToString());
            Console.WriteLine(garden.Plots);

            return (garden.Plots, 0);
        }
    }
}
