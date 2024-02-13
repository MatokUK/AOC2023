using MatejTestingConsole;
using System.Reflection;
//using CommandLine;
//using CommandLine.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        long part1;
        long part2;

        string inputFilePath = @"..\..\..\input\day12.small";
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), inputFilePath);
        string[] lines = File.ReadAllLines(path);

        Console.WriteLine(args[0]);

        Day12 day = new();
        (part1, part2) = day.Execute(lines);

        Console.WriteLine("Part I: " + part1);
        Console.WriteLine("Part II: " + part2);
    }
}