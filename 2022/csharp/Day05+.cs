using System.Diagnostics;

class Day05Plus
{
    private record Command(int Count, int From, int To);

    private const string InputFilePath = "input-90mb.txt"; // this file was not committed

    public static string Part1() => Execute(reverse: true);
    public static string Part2() => Execute(reverse: false);

    private static string Execute(bool reverse)
    {
        var stopWatch = Stopwatch.StartNew();

        var (stacks, stackHeadIndexes) = CreateStacks();

        CreateCommands().ForEach(it => DoExecute(it, stacks, stackHeadIndexes, reverse));

        var result = string.Concat(stacks.Select((it, i) => it[stackHeadIndexes[i]]));

        Console.WriteLine($"{result}. Elapsed: {stopWatch.Elapsed}");

        return result;
    }

    private static void DoExecute(
        Command command, char[][] stacks, int[] stackHeadIndexes, bool reverse)
    {
        var (count, from, to) = command;

        var sliceFrom = stacks[from]
            .AsSpan()
            .Slice(stackHeadIndexes[from] - count + 1, count);

        var sliceTo = stacks[to]
            .AsSpan()
            .Slice(stackHeadIndexes[to] + 1, count);

        sliceFrom.CopyTo(sliceTo);

        if (reverse)
        {
            sliceTo.Reverse();
        }

        stackHeadIndexes[to] += count;
        stackHeadIndexes[from] -= count;
    }

    private static (char[][], int[]) CreateStacks()
    {
        var maxSize = File
            .ReadLines(InputFilePath)
            .TakeWhile(it => it[1] != '1')
            .Count() * 9;

        var stacks = new char[9][];
        var stackHeadIndexes = new int[9];

        for (var i = 0; i < stacks.Length; i++)
        {
            stacks[i] = new char[maxSize];
            stackHeadIndexes[i] = -1;
        }

        foreach (var line in File.ReadLines(InputFilePath).TakeWhile(it => it[1] != '1').Reverse())
        {
            for (var i = 1; i < line.Length; i += 4)
            {
                if (char.IsLetter(line[i]))
                {
                    var id = i / 4;

                    stackHeadIndexes[id]++;

                    stacks[id][stackHeadIndexes[id]] = line[i];
                }
            }
        }

        return (stacks, stackHeadIndexes);
    }

    private static List<Command> CreateCommands()
    {
        return File
            .ReadLines(InputFilePath)
            .Where(it => it.FirstOrDefault() == 'm')
            .Select(it => it.Split(new[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries))
            .Select(it => new Command(
                int.Parse(it[0]), 
                int.Parse(it[1]) - 1, 
                int.Parse(it[2]) - 1))
            .ToList();
    }
}