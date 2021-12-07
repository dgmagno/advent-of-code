class Day02 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        return GetResult(inputLines, new DefaultCommandProcessor());
    }

    private static int GetResult(string[] inputLines, ICommandProcessor commandProcessor)
    {
        var commands = CreateCommands(inputLines);

        foreach (var command in commands)
        {
            commandProcessor.Add(command);
        }

        return commandProcessor.Process();
    }

    private static IReadOnlyList<Command> CreateCommands(string[] inputLines)
    {
        return inputLines
            .Select(line => line.Split(" "))
            .Select(line => new Command(line[0], int.Parse(line[1])))
            .ToArray();
    }

    protected override long Part2(string[] inputLines)
    {
        return GetResult(inputLines, new AimCommandProcessor());
    }
}

interface ICommandProcessor
{
    void Add(Command command);
    int Process();
}

record Command(string Direction, int Units)
{
    public bool IsUp => Direction == "up";
    public bool IsDown => Direction == "down";
    public bool IsForward => Direction == "forward";
}

class DefaultCommandProcessor : ICommandProcessor
{
    private int depth = 0;
    private int horizontal = 0;

    public void Add(Command command)
    {
        if (command.IsUp)
        {
            depth -= command.Units;
        }
        else if (command.IsDown)
        {
            depth += command.Units;
        }
        else if (command.IsForward)
        {
            horizontal += command.Units;
        }
    }

    public int Process() => depth * horizontal;
}

class AimCommandProcessor : ICommandProcessor
{
    private int depth = 0;
    private int horizontal = 0;
    private int aim = 0;

    public void Add(Command command)
    {
        if (command.IsUp)
        {
            aim -= command.Units;
        }
        else if (command.IsDown)
        {
            aim += command.Units;
        }
        else if (command.IsForward)
        {
            horizontal += command.Units;
            depth += command.Units * aim;
        }
    }

    public int Process() => depth * horizontal;
}