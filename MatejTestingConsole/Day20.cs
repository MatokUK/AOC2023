namespace MatejTestingConsole
{
    public class Day20: AdventSolution
    {
        enum ModuleType: byte
        {
             FlipFlop,
             Conjuction,
             Broadcaster
        }

        enum Pulse: byte
        {
            LOW, 
            HIGH,
        }

        interface Module
        {
            public string Name { get; }

            public List<(string, Pulse, string)> Receive(Pulse pulse, string fromModule);

            public string ToString()
            {
                return "";
            }

        }

        sealed class FlipFlop: Module
        {
            private string name;
            private bool on = false;
            public string Name => this.name;

            private List<string> outputs;

            public FlipFlop(string name, string[] outs)
            {
                this.name = name;
                outputs = outs.ToList();
            }

            public List<(string, Pulse, string)> Receive(Pulse pulse, string fromModule)
            {
                Console.WriteLine($"{fromModule} [{(pulse == Pulse.HIGH ? '+' : '-')}] -> {Name} ");

                if (pulse == Pulse.LOW)
                {
                    on = !on;
                }

                List<(string, Pulse, string)> outs = new();

                foreach (string output in outputs)
                {
                    outs.Add((output, on ? Pulse.HIGH : Pulse.LOW, Name));
                }

                return outs;
            }
        }

        class Conjunction: Module
        {
            private string name;
            Dictionary<string, Pulse> inputs = new();
            private List<string> outputs;

            public string Name => this.name;


            public Conjunction(string name, string[] outs, List<string> inputs)
            {
                this.name = name;
                this.outputs = outs.ToList();

                foreach (string input in inputs)
                {
                    this.inputs.Add(input, Pulse.LOW);
                }    
            }

            public List<(string, Pulse, string)> Receive(Pulse pulse, string fromModule)
            {
                Console.WriteLine($"{fromModule} [{(pulse == Pulse.HIGH ? '+' : '-')}] -> {Name} ");

                List <(string, Pulse, string)> outs = new();
                inputs[fromModule] = pulse;

                var n = inputs.Values.All(x => x == Pulse.HIGH) ? Pulse.LOW : Pulse.HIGH;

                foreach (string output in outputs)
                {
                    outs.Add((output, n, Name));
                }

                return outs;
            }
        }

        class Broadcaster: Module
        {
            private List<string> outputs;

            public string Name => "broadcaster";

            public Broadcaster(string[] outs)
            {
                outputs = outs.ToList();
            }

            public List<(string, Pulse, string)> Receive(Pulse pulse, string fromModule)
            {
                Console.WriteLine($"{fromModule} [{(pulse == Pulse.HIGH ? '+' : '-')}] -> {Name} ");
                List<(string, Pulse, string)> outs = new();

                foreach (string output in outputs)
                {
                    outs.Add((output, pulse, Name));
                }

                return outs;
            }
        }
     

        public (long, long) Execute(string[] lines)
        {
            long partI = 0;

            var modules = ReadModules(lines);

            //var b = modules.GetValueOrDefault("broadcaster", null);

            List<(string, Pulse, string )> active = new();
            Dictionary<string, (Pulse, string)> ad = new();

            active.Add(("broadcaster", Pulse.LOW, "BUTTON"));

            ad.Add("broadcaster", (Pulse.LOW, "BUTTON"));

            for (int i = 0; i < 9; ++i)
            {
                //(string moduleName, var signal, var from) = active.ElementAt(0);
                var key = ad.Keys.ElementAt(0);

                (var signal, var from) = ad[key];
                ad.Remove(key);
                //active.RemoveAt(0);

                //var xxx = modules.GetValueOrDefault(moduleName, null);
                var xxx = modules.GetValueOrDefault(key, null);
                //var nnest = xxx.Receive(signal, from);
                var nnest = xxx.Receive(signal, from);
                //active.AddRange(nnest);
           //     Console.WriteLine(nnest);

                foreach (var q in nnest)
                {
                    if (!ad.ContainsKey(q.Item1))
                    {
                        ad[q.Item1] = (q.Item2, q.Item3);
                    }
                }

            }

            //b.Receive(Pulse.LOW);

            return (partI, 0);
        }

        private Dictionary<string, Module> ReadModules(string[] lines)
        {
            var reverseWires = ReadReverseWires(lines);

            Dictionary<string, Module> modules = new();

            foreach (string line in lines)
            {
                var parts = line.Split(" -> ");
                var outs = parts[1].Split(", ");
                Module module;

                if (parts[0] == "broadcaster")
                {
                    module = new Broadcaster(outs);                    
                } 
                else if (parts[0].ElementAt(0) == '%')
                {
                    module = new FlipFlop(parts[0].Substring(1), outs);
                }
                else
                {
                    module = new Conjunction(parts[0].Substring(1), outs, reverseWires[parts[0].Substring(1)]);
                }

                modules.Add(module.Name, module);
            }

            return modules;
        }

        private Dictionary<string, List<string>> ReadReverseWires(string[] lines)
        {
            Dictionary<string, List<string>> wires = new();

            foreach (string line in lines)
            {
                var parts = line.Split(" -> ");
                var outs = parts[1].Split(", ");

                foreach (var o in outs)
                {
                    if (!wires.ContainsKey(o))
                    {
                        wires.Add(o, new List<string>());
                    }

                    string moduleName = parts[0] == "broadcaster" ? "broadcaster" : parts[0].Substring(1);

                    wires[o].Add(moduleName);
                }                
            }

            return wires;
        }
    }
}
