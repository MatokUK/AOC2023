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
    public class Day12: AdventSolution
    {
        class Stream
        {
            private readonly string springs;
            private int len;
            private List<int> config;

            public Stream(string springs, List<int> config)
            {                
                this.springs = springs;
                this.config = config;
                this.len = springs.Count(c => c == '?');
            }

            public long Aggamgements(int multiple = 1)
            {                                
                if (len == 0)
                {
                    return 0;
                }
                
                int max = Convert.ToInt32(string.Concat(Enumerable.Repeat('1', len)), 2);

                Debug.WriteLine($"len {len}");
                Debug.WriteLine($"max {max}");

                int possibilities = GetPossibilities(this.springs);


                Console.WriteLine($"{this.ToString()} has {possibilities} !!!!!!!!!!!!!!!");

                if (multiple > 1) 
                {
                    int possibilities2 = GetPossibilities("?" + this.springs);
                    Console.WriteLine($"                        {possibilities2} 2222222222222222222!!!!!!!!!!!!!!!");

                    int possibilities3 = GetPossibilities(this.springs + "?");
                    Console.WriteLine($"                        {possibilities3} 2222222222222222222!!!!!!!!!!!!!!!");

                }

                return possibilities;
            }

            public override string ToString()
            {
                string o = this.springs+ " <";

                foreach (var c in config)
                {
                    o += c.ToString() + ". ";
                }
                return o + ">";
            }

            private int GetPossibilities(string springs)
            {
                int possibilities = 0;
                int len = springs.Count(c => c == '?');
                int max = Convert.ToInt32(string.Concat(Enumerable.Repeat('1', len)), 2);

                while (max >= 0)
                {
                    string pattern = Convert.ToString(max, 2).PadLeft(len, '0').Replace('0', '.').Replace('1', '#');

                    //     Debug.WriteLine("Try " + pattern);

                    if (IsValid(pattern, springs))
                    {
                        ++possibilities;
                    }

                    --max;
                }

                return possibilities;
            }


            private bool IsValid(string pattern, string sss)
            {
                string option = CreatePatters(pattern, sss);

                return IsOptionValid(option, this.config.ToList());
            }

            private string CreatePatters(string pattern, string sss)
            {
                string option = "";
                int patternIdx = 0;

                foreach (char c in sss)
                {
                    if (c == '?')
                    {
                        option += pattern[patternIdx++];
                    } else
                    {
                        option += c;
                    }
                }

                return option;
            }

            private bool IsOptionValid(string option, List<int> config)
            {
                int seqLen = 0;
                bool seq = false;

                foreach (char c in option)
                {
                    if (c == '#')
                    {
                        if (!seq)
                        { 
                            seq = true;
                        }
                        seqLen++;
                    } 
                    else if (seq)
                    {
                        if (config.Count > 0 && config.First() == seqLen)
                        {
                            config.Remove(seqLen);
                        } 
                        else
                        {
                            return false;
                        }

                        seq = false;
                        seqLen = 0;
                    }
                }

                if (seqLen > 0)
                {
                    if (config.Count > 0 && config.First() == seqLen)
                    {
                        config.Remove(seqLen);
                    }
                    else
                    {
                        return false;
                    }
                }

                return config.Count == 0;
            }
        }


        public (long, long) Execute(string[] lines)

        {
            long arrangements = 0;
            long arrangementsUnfolded = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(' ');

                var counts = parts[1].Split(',').Select(int.Parse).ToList();
                Stream stream1 = new(parts[0], counts);
                //  Stream stream2 = new(new string(parts[0], 5), parts[1].Split(',').Select(int.Parse).ToList());

              //  var repeatedStream = string.Concat(Enumerable.Repeat(string.Concat(parts[0], "?"), 5));

                //Stream stream2 = new(repeatedStream.Remove(repeatedStream.Length - 1), Enumerable.Repeat(counts, 5).SelectMany(x => x).ToList());

                arrangements += stream1.Aggamgements();

                stream1.Aggamgements(2);
                //arrangementsUnfolded += stream2.Aggamgements();
            }

            return (arrangements, arrangementsUnfolded);
        }
    }
}
