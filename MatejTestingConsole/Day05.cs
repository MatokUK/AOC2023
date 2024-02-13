using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MatejTestingConsole.Day2Class;
using System.Globalization;
using System.Numerics;
using System.Drawing;

namespace MatejTestingConsole
{
    public class Day05 : AdventSolution
    {
        sealed class Interval
        {
            private ulong start;
            private ulong end;
            private ulong length;

            public ulong Start
            {
                get { return start; }
            }

            public ulong Length
            {
                get { return length; }
            }


            public Interval(ulong start, ulong length)
            {
                this.start = start;
                this.length = length;
                this.end = start + length - 1;
            }

            public static Interval operator +(Interval interval, long offset)
            {
                if (offset >= 0)
                {
                    return new Interval(interval.start + (ulong)offset, interval.length);
                }
                else
                {
                    return new Interval(interval.start - (ulong)Math.Abs(offset), interval.length);
                }
            }

            public static bool operator *(ulong point, Interval interval)
            {
                return point >= interval.start && point <= interval.end;
            }

            public static List<Interval> operator /(Interval interval, IntervalOffset intervalOffset)
            {
                List <Interval> split = new();

                if (interval.end < intervalOffset.interval.start || interval.start > intervalOffset.interval.end)
                {
                    // no overlap
                    return split;
                }

                //  A) ********************************
                //  B)            -----  
                if (interval.start <= intervalOffset.interval.start && interval.end >= intervalOffset.interval.end)
                {

                    var overlap = new Interval(intervalOffset.interval.start, intervalOffset.interval.length);
                    var begin = new Interval(interval.start, intervalOffset.interval.start - interval.start);
                    var end = new Interval(overlap.end + 1, interval.end - intervalOffset.interval.end);


                    split.Add(overlap);
                    split.Add(begin);
                    split.Add(end);

                    return split;
                }


                //  A)            *****  
                //  B) --------------------------------
                if (interval.start >= intervalOffset.interval.start && interval.end <= intervalOffset.interval.end)
                {
                    // full
                    return split;
                }

                //  A)       ***************  
                //  B)   -------------
                if (interval.start >= intervalOffset.interval.start && interval.start <= intervalOffset.interval.end)
                {                    
                    var overlap = new Interval(interval.start, intervalOffset.interval.end - interval.start + 1);
                    var rest = new Interval(overlap.end + 1, interval.end - overlap.end);

                    
                    split.Add(overlap);
                    split.Add(rest);

                    return split;
                }

                //  A)       ***************  
                //  B)                -------------
                if (interval.end >= intervalOffset.interval.start && interval.end <= intervalOffset.interval.end)
                {
                    var rest = new Interval(interval.start, intervalOffset.interval.start - interval.start);
                    var overlap = new Interval(intervalOffset.interval.start, interval.end - intervalOffset.interval.start + 1);
                    
                    split.Add(overlap);
                    split.Add(rest);

                    return split;
                }

                return split;
            }

            public override string ToString()
            {
                return $"<{start}, {start+length-1}>";
            }

            public bool isIncludedIn(Interval other)
            {
                return this.start >= other.start && this.end <= other.end;
            }

            public override bool Equals(object? obj)
            {
                if (obj is Interval other)
                {
                    return this.start == other.start && this.end == other.end;
                }

                return false;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17; // Prime number

                    // Combine hash codes of the two main values
                    hash = hash * 31 + start.GetHashCode();
                    hash = hash * 31 + end.GetHashCode();

                    return hash;
                }
            }
        }


        sealed class IntervalOffset
        {
            public readonly Interval interval;
            private long offet;            

            public IntervalOffset(Interval interval, long offet)
            {
                this.interval = interval;
                this.offet = offet;
            }

            public static ulong operator *(ulong point, IntervalOffset intervalOffet)
            {
               
                if (point * intervalOffet.interval)
                {
                    if (intervalOffet.offet >= 0)
                    {
                        return point + (ulong)intervalOffet.offet;
                    }
                    else
                    {
                        return point - (ulong)Math.Abs(intervalOffet.offet);
                    }
                }

                return point;
            }


            public static Interval operator *(Interval interval, IntervalOffset intervalOffet)
            {               
                if (interval.isIncludedIn(intervalOffet.interval))
                {
                    return interval + intervalOffet.offet;
                }

                return interval;
            }


            public override string ToString()
            {
                return $"{interval.ToString()} ({offet})";
            }
        }

        public (long, long) Execute(string[] lines)
        {
            List<ulong> Seeds = readSeeds(lines[0]);            
            List<IntervalOffset> seed2SoilInterval = readMapInterval(lines, "seed-to-soil");
            List<IntervalOffset> soil2FertilizerInterval = readMapInterval(lines, "soil-to-fertilizer");            
            List<IntervalOffset> fertilizer2WaterInterval = readMapInterval(lines, "fertilizer-to-water");
            List<IntervalOffset> water2LightInterval = readMapInterval(lines, "water-to-light");
            List<IntervalOffset> light2TemparatureInterval = readMapInterval(lines, "light-to-temperature");
            List<IntervalOffset> temparature2HumidityInterval = readMapInterval(lines, "temperature-to-humidity");
            List<IntervalOffset> humidity2LocationInterval = readMapInterval(lines, "humidity-to-location");

            // PART 1:
            Seeds = MakeMapping(Seeds, seed2SoilInterval);
            Seeds = MakeMapping(Seeds, soil2FertilizerInterval);
            Seeds = MakeMapping(Seeds, fertilizer2WaterInterval);
            Seeds = MakeMapping(Seeds, water2LightInterval);
            Seeds = MakeMapping(Seeds, light2TemparatureInterval);
            Seeds = MakeMapping(Seeds, temparature2HumidityInterval);
            Seeds = MakeMapping(Seeds, humidity2LocationInterval);

            // PART 2:       
            List<Interval> seedIntervals = readSeedIntervals(lines[0]);
            seedIntervals = MakeIntervalMapping(seedIntervals, seed2SoilInterval);
            seedIntervals = MakeIntervalMapping(seedIntervals, soil2FertilizerInterval);
            seedIntervals = MakeIntervalMapping(seedIntervals, fertilizer2WaterInterval);
            seedIntervals = MakeIntervalMapping(seedIntervals, water2LightInterval);
            seedIntervals = MakeIntervalMapping(seedIntervals, light2TemparatureInterval);
            seedIntervals = MakeIntervalMapping(seedIntervals, temparature2HumidityInterval);
            seedIntervals = MakeIntervalMapping(seedIntervals, humidity2LocationInterval);

            ulong partII = long.MaxValue;
            foreach (var interval in seedIntervals)              
            {                  
                if (interval.Start < partII)                  
                {  
                    partII = interval.Start;                  
                }              
            }

            return ((long)Seeds.Min(), (long)partII);
        }

        private List<ulong> readSeeds(string line)
        {
            List<ulong> Seeds = new();
            var idx = line.IndexOf(':');
            var parts = line.Substring(idx + 2).Split(" ");


            foreach (var part in parts)
            {
                ulong.TryParse(part, out var Seed);

                Seeds.Add(Seed);
            }

            return Seeds;
        }

        private List<Interval> readSeedIntervals(string line)
        {
            List<Interval> Seeds = new();
            var idx = line.IndexOf(':');
            var parts = line.Substring(idx + 2).Split(" ");


            for (int i = 0; i < parts.Length; i += 2)
            {
                ulong.TryParse(parts[i], out var start);
                ulong.TryParse(parts[i+1], out var length);


                Seeds.Add(new Interval(start, length));

            }

            return Seeds;
        }

        private List<ulong> MakeMapping(List<ulong> seeds, List<IntervalOffset> intervalOffsets)
        {
            for (int i = 0; i < seeds.Count; i++)
            {
                foreach (var interval in intervalOffsets)
                {
                    var mapping = seeds[i] * interval;
                    if (mapping != seeds[i])
                    {
                        seeds[i] = mapping;
                        break;
                    }
                }
            }

            return seeds;
        }      

        private List<Interval> MakeIntervalMapping(List<Interval> seeds, List<IntervalOffset> intervalOffsets)
        {
            var splittedIntervals = SplitIntervals(seeds, intervalOffsets);

            for (int i = 0; i < splittedIntervals.Count; i++)
            {
                foreach (var mappingInterval in intervalOffsets)
                {
                    if (splittedIntervals[i].isIncludedIn(mappingInterval.interval))
                    {
                        // operator *: add/remove number from interval
                        splittedIntervals[i] = splittedIntervals[i] * mappingInterval;
                        break;
                    }                    
                }
            }

            return splittedIntervals;
        }

        private List<Interval> SplitIntervals(List<Interval> intervals, List<IntervalOffset> intervalOffsets)
        {
            List<Interval> split = new();

            foreach (var offsetInterval in intervalOffsets)
            {
                foreach (var interval in intervals)
                {
                    var parts = interval / offsetInterval;
                    split.AddRange(parts.Except(split));
                }
            }

            foreach (var interval in intervals)
            {
                bool found = false;
                foreach (var splitInterval in split)
                {
                    if (splitInterval.Start == interval.Start)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    split.Add(interval);
                }
            }

            List<int> keysToRemove = new();

            for (int i = 0; i < split.Count; i++)
            {
                for (int j = 0; j < split.Count; j++)
                {
                    if (i != j && split[i].Start == split[j].Start && split[i].Length > split[j].Length)
                    {
                        keysToRemove.Add(i);
                        break;
                    }
                }
            }

            for (int i = keysToRemove.Count - 1; i >= 0; i--)
            {
                split.RemoveAt(keysToRemove[i]);
            }

            return split;
        }

        private List<IntervalOffset> readMapInterval(string[] lines, string MapName)
        {
            bool InSection = false;
            List<IntervalOffset> Map = new();

            foreach (var line in lines)
            {
                if (InSection)
                {
                    if (line == "")
                    {
                        break;
                    }


                    var parts = line.Split(" ");
                    ulong.TryParse(parts[0], out var Destination);
                    ulong.TryParse(parts[1], out var Source);
                    ulong.TryParse(parts[2], out var Length);

                    Map.Add(new IntervalOffset(new Interval(Source, Length), (long)(Destination - Source)));

                }

                if (line.StartsWith(MapName))
                {
                    InSection = true;
                }
            }

            return Map;
        }
    }
}
