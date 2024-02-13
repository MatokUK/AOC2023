namespace MatejTestingConsole
{
    public class Day01: AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            long sumPart1 = 0;
            long sumPart2 = 0;
            for (int i = 0; i < lines.Length; ++i)
            {
                IEnumerable<char> stringQuery =
                  from ch in lines[i]
                  where Char.IsDigit(ch)
                  select ch;


                if (stringQuery.ToList().Count != 0)
                {
                    sumPart1 += Int32.Parse(stringQuery.First().ToString() + stringQuery.Last().ToString());
                }

                string transformed = TransformTextNumbers(lines[i]);
                Console.WriteLine(transformed);

                sumPart2 += Int32.Parse(transformed.First().ToString() + transformed.Last().ToString());
            }

            return (sumPart1, sumPart2);
        }


        private string TransformTextNumbers(string text)
        {
            string[] numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            string output = "";

            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] >= '0' && text[i] <= '9')
                {
                    output += text[i];
                }
                else
                {
                    for (int n = 0; n < numbers.Length; ++n)
                    {
                        try { 
                            string possibleNumber = text.Substring(i, numbers[n].Length);
                            if (possibleNumber == numbers[n])
                            {
                                output += (n + 1).ToString();
                            }
                        }
                        catch (System.ArgumentOutOfRangeException) 
                        {
                             // pass
                        }
                    }
                }
            }

            return output;
        }
    }
}
