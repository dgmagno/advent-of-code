class Program
{
    public static void Main(string[] args)
    {
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
            "assert" => () =>
            {
                //Debugger.Launch();
                puzzle.Assert();
            },
            _ => throw new NotImplementedException()
        };

        invoke();
    }
}

abstract class Puzzle
{
    protected string[] InputLines => File.ReadAllLines($"{Paths.Inputs}/{Day}.txt");

    public string Day => GetType().Name[3..5];

    public long Part1() => Part1(InputLines);

    public long Part2() => Part2(InputLines);

    public void Assert()
    {
        var lines = File.ReadAllLines($"{Paths.Inputs}/{Day}.ex.txt");

        var exampleInputLines = lines.Skip(2).ToArray();

        if (long.TryParse(lines[0], out var answerPart1))
        {
            var answer = Part1(exampleInputLines);

            Console.WriteLine(answer);
            Debug.Assert(answer == answerPart1);
        }

        if (long.TryParse(lines[1], out var answerPart2))
        {
            var answer = Part2(exampleInputLines);

            Console.WriteLine(answer);
            Debug.Assert(answer == answerPart2);
        }
    }

    protected abstract long Part1(string[] inputLines);
    protected abstract long Part2(string[] inputLines);
}
