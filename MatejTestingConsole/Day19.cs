using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MatejTestingConsole.Day2Class;
using System.Globalization;
using System.Text.RegularExpressions;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.VisualBasic;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata;
using System.Linq.Expressions;

namespace MatejTestingConsole
{
    public class Day19: AdventSolution
    {
        record Variables
        {
            public readonly int x;
            public readonly int m;
            public readonly int a;
            public readonly int s;

            public Variables(string input)
            {
                string pattern = @"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}$";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(input);

                int.TryParse(match.Groups[1].ToString(), out x);
                int.TryParse(match.Groups[2].ToString(), out m);
                int.TryParse(match.Groups[3].ToString(), out a);
                int.TryParse(match.Groups[4].ToString(), out s);
            }

            public int GetValue(string name)
            {
                return name switch
                {
                    "x" => x,
                    "m" => m,
                    "a" => a,
                    "s" => s,
                };
            }

            public override string ToString()
            {
                return $"x = {x} m = {m} a = {a}";
            }

            public long Sum()
            {
                return x + m + a + s;
            }
        }

        record Instruction
        {
            private string? condition = null;
            private string sucessState;

            public Instruction(string code)
            {
                var parts = code.Split(":");

                if (parts.Length == 1)
                {
                    sucessState = parts[0];
                }
                else
                {
                    condition = parts[0];
                    sucessState = parts[1];
                }
            }

            public string? Evaluate(Variables variables)
            {
                if (condition == null)
                {
                    return sucessState;
                }

                ParameterExpression x = Expression.Parameter(typeof(int), condition.ElementAt(0).ToString());

                LambdaExpression e = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { x }, null, condition);
                bool evaluation = (bool) e.Compile().DynamicInvoke(variables.GetValue(condition.ElementAt(0).ToString()));             

                return evaluation ? sucessState : null;
            }
        }

        record WorkFlow
        {
            public readonly string name;
            public readonly Instruction[] instructions;

            public WorkFlow(string input)
            {
                string pattern = @"^([a-z]+){(.+)}$";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(input);

                name = match.Groups[1].ToString();
                instructions = match.Groups[2].ToString().Split(",").Select(x => new Instruction(x)).ToArray();
            }

            public string Evaluate(Variables variables)
            {
                foreach (var instruction in instructions) 
                {
                    var nextWorkflow = instruction.Evaluate(variables);
                    if (nextWorkflow != null)
                    {
                        return nextWorkflow;
                    }
                }

                throw new NotImplementedException();
            }
        }

        class Engine
        {
            private Dictionary<string, WorkFlow> workFlows;

            public Engine (Dictionary<string, WorkFlow> workFlows)
            {
                this.workFlows = workFlows;
            }

            public bool Execute(Variables variables)
            {
                string nextWorkflow = "in";

                do
                {
                    WorkFlow workFlow = workFlows[nextWorkflow];
                    nextWorkflow = workFlow.Evaluate(variables);


                } while (nextWorkflow != "A" && nextWorkflow != "R");


                return nextWorkflow == "A";
            }

            public void Combinations()
            {
                List<Path> openPaths = new();
                List<Path> finishedPaths = new();


                var wf = workFlows["in"];
                Path path = new();

                path.Add(wf);

                openPaths.Add(path);

                do
                {
                    var pp = openPaths.First();
                    openPaths.RemoveAt(0);

                    path.Expand();

                } while (openPaths.Count > 0);


                Console.WriteLine(wf);
            }
        }

        class Path
        {
            private List<WorkFlow> workFlows = new();

            public void Add(WorkFlow workFlow)
            {
                workFlows.Add(workFlow);
            }

            public void Expand()
            {
                Console.WriteLine(workFlows);
            }
        }


        public (long, long) Execute(string[] lines)
        {
            long partI = 0;

            Dictionary<string, WorkFlow> workflows = ReadWorkFlows(lines);
            List<Variables> variables = ReadVariables(lines);

            Engine engine = new(workflows);

            foreach (Variables v in variables) 
            {
                if (engine.Execute(v))
                {
                    partI += v.Sum();
                }
            }

            engine.Combinations();

            return (partI, 0);
        }

        private Dictionary<string, WorkFlow> ReadWorkFlows(string[] lines)
        {
            Dictionary <string, WorkFlow> workFlows = new();

            foreach (string line in lines)
            {
                if (line == "")
                {
                    break;
                }
            
                WorkFlow workFlow = new WorkFlow(line);
                workFlows.Add(workFlow.name, workFlow);
            }

            return workFlows;
        }

        private List<Variables> ReadVariables(string[] lines)
        {
            List<Variables> variables = new();
            bool varPart = false;

            foreach (string line in lines)
            {
                if (varPart)
                {
                    variables.Add(new Variables(line));
                }

                if (line == "")
                {
                    varPart = true;
                }
            }

            return variables;
        }
    }
}
