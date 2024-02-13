using System.Collections.Generic;
using System.IO;

namespace MatejTestingConsole
{
    public class Day24: AdventSolution
    {
        private readonly long MIN_AREA = 7;
        private readonly long MAX_AREA = 27;

        sealed class Line
        {
            private (long x, long y, long z) point;
            private (long x, long y, long z) delta;

            public Line(long x, long y, long z, long d1, long d2, long d3)
            {
                point = (x, y, z);
                delta = (d1, d2, d3);
            }

            public (double x, double y)? FindIntersectionPoint(Line other)
            {
                double x1 = point.x;
                double y1 = point.y;
                double x2 = point.x + delta.x;
                double y2 = point.y + delta.y;

                double x3 = other.point.x;
                double y3 = other.point.y;
                double x4 = other.point.x + other.delta.x;
                double y4 = other.point.y + other.delta.y;

                double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

                // Check if the lines are parallel
                if (denominator == 0)
                {
                    return null;
                }

                double intersectionX = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominator;
                double intersectionY = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominator;

                return (intersectionX, intersectionY);
            }



            public string TwoPoints()
            {
                string o = $"[{point.x}, {point.y}]";

                o += $" [{(point.x + delta.x)}, {(point.y + delta.y)}]";

                return o;
            }


            public override string ToString()
            {
                return $"[{point.x}, {point.y}, {point.z}]";
            }
        }

        public (long, long) Execute(string[] lines)
        {
            var vectors = ReadLines(lines);

            for (int a = 0; a < vectors.Count; ++a)
            {
                for (int b = a + 1; b < vectors.Count; ++b)
                {                    
                    if (vectors[a] != vectors[b])
                    {
                        Console.WriteLine(vectors[a].ToString() + " / " + vectors[b].ToString());
                        var intersection = vectors[a].FindIntersectionPoint(vectors[b]);
                        if (intersection.HasValue)
                        {
                            if (intersection.Value.x >= MIN_AREA && intersection.Value.x <= MAX_AREA && intersection.Value.y >= MIN_AREA && intersection.Value.y <= MAX_AREA)
                            {
                                Console.WriteLine($"Intersection in {intersection}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Notinh");
                        }
                        Console.WriteLine();
                    }
                }
            }

            /*Console.WriteLine(vectors[0].TwoPoints());
            Console.WriteLine(vectors[1].TwoPoints());

            var inter = vectors[0].FindIntersectionPoint(vectors[1]);

            Console.WriteLine(inter);*/
            return (0, 0);
        }

        private List<Line> ReadLines(string[] lines)
        {
            List <Line> vectors = new();  

            foreach (var line in lines)
            {
                var parts = line.Split(" @ ");

                var point = parts[0].Split(", ");
                long.TryParse(point[0], out long x);
                long.TryParse(point[1], out long y);
                long.TryParse(point[2], out long z);

                var delta = parts[1].Split(", ");
                long.TryParse(delta[0], out long d1);
                long.TryParse(delta[1], out long d2);
                long.TryParse(delta[2], out long d3);

                var vector = new Line(x, y, z, d1, d2, d3);
                vectors.Add(vector);
            }

            return vectors;
        }
    }
}


