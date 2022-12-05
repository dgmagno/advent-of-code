using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Day05 : Puzzle
{
    private readonly Regex craneCommandRegex = new(@"move (\d+) from (\d+) to (\d+)", RegexOptions.Compiled);

    protected override string Part1(string[] inputLines)
    {
        var (crane, commands) = Create(inputLines);

        commands.ForEach(it => crane.Execute(it));

        return crane.Peek();
    }

    private (Crane, List<CraneCommand>) Create(string[] inputLines)
    {
        var craneState = new List<string>();
        var craneCommands = new List<CraneCommand>();

        foreach (var line in inputLines)
        {
            if (!line.Any()) continue;

            if (line[0] == 'm')
            {
                var matchGroups = craneCommandRegex.Match(line).Groups;

                craneCommands.Add(new CraneCommand(
                    int.Parse(matchGroups[1].Value),
                    int.Parse(matchGroups[2].Value),
                    int.Parse(matchGroups[3].Value)));
            }
            else
            {
                craneState.Add(line);
            }
        }

        return (new Crane(craneState), craneCommands);
    }

    protected override string Part2(string[] inputLines)
    {
        var (crane, commands) = Create(inputLines);

        commands.ForEach(it => crane.ExecutePlus(it));

        return crane.Peek();
    }
}

class Crane
{
    private readonly Dictionary<int, CrateStack> stacks = new();

    public Crane(IReadOnlyList<string> craneState)
    {
        for (int i = 1; i < craneState[0].Length - 1; i += 4)
        {
            var stackState = string.Concat(craneState.Select(it => it[i])).Trim();

            var crateStack = new CrateStack(stackState);

            stacks[crateStack.Id] = crateStack;
        }
    }

    public void Execute(CraneCommand command)
    {
        for (int i = 0; i < command.Quantity; i++)
        {
            var crate = stacks[command.From].Pop();

            stacks[command.To].Push(crate);
        }
    }

    public void ExecutePlus(CraneCommand command)
    {
        var crates = stacks[command.From].Pop(command.Quantity);

        stacks[command.To].Push(crates);
    }

    public string Peek()
    {
        return string.Concat(stacks.Select(it => it.Value.Peek()));
    }
}

record CraneCommand(int Quantity, int From, int To);

class CrateStack
{
    private readonly Stack<char> stack = new();

    public int Id { get; private set; }

    public CrateStack(string stackState)
    {
        Id = stackState.TakeLast(1).Single() - '0';

        foreach (var crate in stackState.Reverse().Skip(1))
        {
            stack.Push(crate);
        }
    }

    public void Push(char crate) => stack.Push(crate);

    public void Push(string crates)
    {
        foreach (var crate in crates)
        {
            stack.Push(crate);
        }
    }

    public char Pop() => stack.Pop();

    public char Peek() => stack.Peek();

    public string Pop(int quantity)
    {
        var crates = Enumerable
            .Range(0, quantity)
            .Select(_ => stack.Pop())
            .Reverse();

        return string.Concat(crates);
    }
}