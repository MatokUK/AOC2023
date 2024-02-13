using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatejTestingConsole
{
    internal interface AdventSolution
    {
        public (long, long) Execute(string[] lines);
    }
}
