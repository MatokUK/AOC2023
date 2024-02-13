namespace MatejTestingConsole
{
    public class Day3: AdventSolution
    {
        private const int GEAR = 2;
        private const int PART = 1;


        public readonly List<int> Parts = new List<int>();
        public readonly SortedDictionary<string, List<int>> Gears = new SortedDictionary<string, List<int>>();

        private int rows;
        private int cols;
        private char[,] plan;


        public (long, long) Execute(string[] lines)
        {
            rows = lines.Length;
            cols = lines[0].Length;
            plan = new char[rows + 2, cols + 2];

            readSchema(lines);

            parseSchema();

            int GearSum = 0;
            foreach (KeyValuePair<string, List<int>> pair in Gears)
            {
                if (pair.Value.Count == 2)
                {
                    GearSum += pair.Value[0] * pair.Value[1];
                }
            }

            return (Parts.Sum(x => Convert.ToInt32(x)), GearSum);
        }

        private void readSchema(string[] lines)
        {            
            plan[0, 0] = '.';
            plan[0, cols + 1] = '.';
            for (int j = 0; j < cols; j++)
            {
                plan[0, j + 1] = '.';
            }

            for (int i = 0; i < lines.Length; i++)
            {
                plan[i + 1, 0] = '.';
                for (int j = 0; j < lines[i].Length; j++)
                {
                    plan[i + 1, j + 1] = lines[i][j];
                }
                plan[i + 1, lines[i].Length + 1] = '.';
            }


            plan[lines[0].Length + 1, 0] = '.';
            plan[lines[0].Length + 1, lines[0].Length + 1] = '.';
            for (int j = 0; j < lines[0].Length; j++)
            {
                plan[lines[0].Length + 1, j + 1] = '.';
            }
        }
    

        private void parseSchema()
        {
            for (int r = 1; r < rows + 2; ++r)
            {
                int CurrentDigit = 0;
                bool isIncludedPart = false;
                bool isGearPart = false;
                string GearKey = "";

                for (int c = 1; c < cols + 2; ++c)
                {
                    if (Char.IsDigit(plan[r, c]))
                    {
                        if (CurrentDigit == 0 && plan[r, c - 1] != '.')
                        {
                            isIncludedPart = true;
                            if (plan[r, c - 1] == '*')
                            {
                                isGearPart = true;
                                GearKey = (r + "/" + (c -1));
                            }                            
                        }
                        CurrentDigit = CurrentDigit * 10 + (plan[r, c] - '0');


                        (int Part, int PartRow, int PartCol) = PartType(r, c);

                        if (Part >= PART)
                        {
                            isIncludedPart = true;
                        }

                        if (Part == GEAR)
                        {
                            isGearPart = true;
                            GearKey = (PartRow + "/" + PartCol);
                        }
                    }
                    else if (CurrentDigit != 0)
                    {
                        if (plan[r, c] != '.')
                        {
                            isIncludedPart = true;
                            if (plan[r, c] == '*')
                            {
                                isGearPart = true;
                                GearKey = (r + "/" + c);
                            }
                        }
                    
                        if (isIncludedPart)
                        {
                            Parts.Add(CurrentDigit);
                        }

                        if (isGearPart)
                        {
                            if (!Gears.ContainsKey(GearKey))
                            {
                                Gears.Add(GearKey, new List<int>());
                            }

                            Gears[GearKey].Add(CurrentDigit);
                        }
                        CurrentDigit = 0;
                        isIncludedPart = false;
                        isGearPart = false;
                        GearKey = "";
                    }
                }
            }
        }

        private (int, int, int) PartType(int r, int c)
        {
            if (plan[r - 1, c] == '*')
            {
                return (GEAR, r-1, c);
            }

            if (plan[r + 1, c] == '*')
            {
                return (GEAR, r + 1, c);
            }

            if (plan[r - 1, c - 1] == '*')
            {
                return (GEAR, r - 1, c - 1);
            }

            if (plan[r + 1, c - 1] == '*')
            {
                return (GEAR, r + 1, c - 1);
            }

            if (plan[r + 1, c + 1] == '*')
            {
                return (GEAR, r + 1, c + 1);
            }

            if (plan[r - 1, c + 1] == '*')
            {
                return (GEAR, r - 1, c + 1);
            }


            if (plan[r - 1, c] != '.' || plan[r + 1, c] != '.' ||
                          plan[r - 1, c - 1] != '.' || plan[r + 1, c - 1] != '.' || plan[r + 1, c + 1] != '.' || plan[r - 1, c + 1] != '.')
            {
                return (PART, 0, 0);
            }

            return (0, 0, 0);
        }
    }    
}
