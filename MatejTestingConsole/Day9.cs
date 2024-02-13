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

    class OasisReport
    {
        private List<List<long>> prediction = new List<List<long>>();

        public OasisReport(List<long> report) 
        {
            this.prediction.Add(report);
        }

        public long NextNumber()
        {
            do
            {
                var prev = this.prediction.Last();

                List<long> next = new List<long>();
                for (var i = 1; i < prev.Count; i++)
                {

                    next.Add(prev[i] - prev[i - 1]);
                }

                this.prediction.Add(next);
            } while (!this.prediction.Last().All(x => x == 0));



            this.prediction.Last().Add(0);

            for (var i = this.prediction.Count - 2; i >= 0; --i)
            {
                this.prediction[i].Add(this.prediction[i].Last() + this.prediction[i + 1].Last());
            }

            return this.prediction[0].Last();
        }

        public long PrevNumber()
        {

            this.prediction.Last().Insert(0, 0);

            for (var i = this.prediction.Count - 2; i >= 0; --i)
            {
                this.prediction[i].Insert(0, this.prediction[i].First() - this.prediction[i + 1].First());
            }

            return this.prediction[0].First();
        }
    }

    public class Day9: AdventSolution
    {
        public (long, long) Execute(string[] lines)
        {
            List<OasisReport> reports = ReadReports(lines);
            long sum1 = 0;
            long sum2 = 0;

            foreach (OasisReport report in reports)
            {
                sum1 += report.NextNumber();
                sum2 += report.PrevNumber();
            }

            return (sum1, sum2);
        }


        private List<OasisReport> ReadReports(string[] lines)
        {
            List<OasisReport> reports = new List<OasisReport>();

            foreach (var line in lines)
            {
                reports.Add(new OasisReport(line.Split(" ").Select(long.Parse).ToList()));
            }

            return reports;
        }
    }
}
