class Program
{
    public static void Main(string[] args)
    {
        args = Load(args);

        if (args.Length != 2)
        {
            Console.WriteLine("Command: dotnet run <day> <part>. Values <day>: 1 - 25, <part>: 1, 2, assert");
            return;
        }

        var day = $"{args[0].PadLeft(2, '0')}";
        var part = args[1];

        var puzzleType = Type.GetType($"Day{day}")
            ?? throw new NullReferenceException();

        var puzzle = Activator.CreateInstance(puzzleType) as Puzzle
            ?? throw new NullReferenceException();

        Action invoke = part switch
        {
            "1" => () => Console.WriteLine(puzzle.Part1()),
            "2" => () => Console.WriteLine(puzzle.Part2()),
            "assert" => () => puzzle.Assert(),
            _ => throw new NotImplementedException()
        };

        invoke();
    }

    private static string[] Load(string[] args)
    {
        var data = File.ReadAllLines(Paths.DataFile);

        if (args.Length != 2)
        {
            return data[0].Split(" ");
        }
        else
        {
            data[0] = string.Join(" ", args);

            File.WriteAllLines(Paths.DataFile, data);

            return args;
        }
    }
}

abstract class Puzzle
{
    protected string[] InputLines => File.ReadAllLines($"{Paths.Inputs}/{Day}.txt");
    protected string Input => File.ReadAllText($"{Paths.Inputs}/{Day}.txt");

    public string Day => GetType().Name[3..5];

    public string Part1() => Part1(InputLines);

    public string Part2() => Part2(InputLines);

    public void Assert()
    {
        var lines = File.ReadAllLines($"{Paths.Inputs}/{Day}.ex.txt");

        var exampleInputLines = lines.Skip(2).ToArray();

        var answerPart1 = lines[0];
        var answer1 = Part1(exampleInputLines);

        Console.WriteLine(answer1);
        Debug.Assert(answer1 == answerPart1);

        var answerPart2 = lines[1];
        var answer2 = Part2(exampleInputLines);

        Console.WriteLine(answer2);
        Debug.Assert(answer2 == answerPart2);
    }

    protected abstract string Part1(string[] inputLines);
    protected abstract string Part2(string[] inputLines);
}
