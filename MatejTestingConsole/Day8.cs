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

namespace MatejTestingConsole
{
    public class Day8: AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            List<char> steps = ReadSteps(lines[0]);
            SortedDictionary<string, (string, string)> nodes = ReadNodes(lines.Skip(2).ToArray());
            SortedDictionary<string, string> ghostNodes = ReadStartingNodes(nodes.Keys.ToList());
          /*  SortedDictionary<string, string> ghostNodes = new SortedDictionary<string, string>();
            ghostNodes["VSA"] = "VSA";
            ghostNodes["AAA"] = "AAA";
*/
            long n = 0;
            long x = 0;
            string key = "AAA";

            do
            {
                foreach (char step in steps)
                {
                    if (step == 'L')
                    {
                        key = nodes[key].Item1;
                    }
                    else
                    {
                        key = nodes[key].Item2;
                    }

                    //   Console.WriteLine($"step {step}, current node {key}");
                }
                n++;
            } while (key != "ZZZ");

            /*         foreach (var node in nodes)
                     {
                         Console.WriteLine($"{node.Key} = {node.Value}");
                     }
         */


            foreach (var ghostNode in ghostNodes.ToList())
            {
                x = 0;
                var keyX = ghostNode.Value;
                do
                {
                    foreach (char step in steps)
                    {
                        if (step == 'L')
                        {
                            // ghostNodes[ghostNode.Key] = nodes[ghostNode.Value].Item1;
                            keyX = nodes[keyX].Item1;
                        }
                        else
                        {
                            // ghostNodes[ghostNode.Key] = nodes[ghostNode.Value].Item2;
                            keyX = nodes[keyX].Item2;
                        }
                    }






                    //   Console.WriteLine($"step {step}, current node {key}");


                 /*   foreach (var ghostNode in ghostNodes)
                    {
                        Console.WriteLine(ghostNode.Value);
                    }

                    Console.WriteLine("------------------------------" + x * steps.Count);*/
                    x++;

                    if (keyX[2] == 'Z') {
                        Console.WriteLine($"Solution for {ghostNode.Key} => {x} ({steps.Count})");
                    }

                } while (keyX[2] != 'Z');
            }

            /*  foreach (var xx in ghostNodes)
              {
                  Console.WriteLine(xx.Value);
              }*/


            Console.WriteLine(293 * 63568204859);
            return (n * steps.Count, x * steps.Count);
        }

        private bool AllNodesFinished(SortedDictionary<string, string> nodes)
        {
            foreach(var node in nodes)
            {
                if (node.Value[2] != 'Z')
                {
                    return false;
                }
            }

            return true;
        }

        private SortedDictionary<string, string> ReadStartingNodes(List<string> nodes)
        {
            SortedDictionary<string, string> startNodes = new SortedDictionary<string, string> ();

            foreach (string node in nodes)
            {
                if (node[2] == 'A')
                {
                    startNodes[node] = node;
                }
            }

            return startNodes;
        }


        private List<char> ReadSteps(string line)
        {
            return line.ToList();
        }

        private SortedDictionary<string, (string, string)> ReadNodes(string[] lines)
        {
            SortedDictionary<string, (string, string)> nodes = new SortedDictionary<string, (string, string)>();


            foreach (var line in lines)
            {
                var parts = line.Split(" = ");

                nodes[parts[0]] = (parts[1].Substring(1, 3), parts[1].Substring(6, 3));
            }

            return nodes;
        }
    }
}
