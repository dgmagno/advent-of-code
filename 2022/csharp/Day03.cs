using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Day03 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        return inputLines
            .Select(it => new Rucksack(it))
            .Sum(it => it.FindSharedItem().Priority);
    }

    protected override long Part2(string[] inputLines)
    {
        return inputLines
            .Select(it => new Rucksack(it))
            .Chunk(3)
            .Sum(it => Rucksack.FindSharedItem(it).Priority);
    }
}

class Rucksack
{
    private readonly List<IRucksackItem> firstCompartmentItems = new();
    private readonly List<IRucksackItem> secondCompartmentItems = new();

    public Rucksack(ReadOnlySpan<char> rawItems)
    {
        for (var i = 0; i < rawItems.Length / 2; i++)
        {
            firstCompartmentItems.Add(CreateItem(rawItems[i]));
            secondCompartmentItems.Add(CreateItem(rawItems[rawItems.Length -1 - i]));
        }
    }

    public IRucksackItem FindSharedItem()
    {
        return FirstCompartmentItems.Intersect(SecondCompartmentItems).Single();
    }

    public static IRucksackItem FindSharedItem(IEnumerable<Rucksack> rucksacks)
    {
        return rucksacks
            .Select(it => it.AllItems)
            .Aggregate((a, b) => a.Intersect(b).ToList())
            .Single();
    }

    private static IRucksackItem CreateItem(char rawItem)
    {
        if (!char.IsLetter(rawItem))
        {
            throw new ArgumentException($"Invalid rucksack item: {rawItem}");
        }

        if (char.IsLower(rawItem))
        {
            return new LowPriorityRucksackItem { Value = rawItem };
        }

        return new HighPriorityRucksackItem { Value = rawItem };
    }

    public IReadOnlyList<IRucksackItem> FirstCompartmentItems => firstCompartmentItems;
    public IReadOnlyList<IRucksackItem> SecondCompartmentItems => secondCompartmentItems;
    public IReadOnlyList<IRucksackItem> AllItems => FirstCompartmentItems
        .Concat(SecondCompartmentItems)
        .ToList();
}

interface IRucksackItem
{
    char Value { get; }
    int Priority { get; }
}

class LowPriorityRucksackItem : IRucksackItem, IEquatable<LowPriorityRucksackItem?>
{
    public  required char Value { get; init; }

    public int Priority => Value - 'a' + 1;

    public override bool Equals(object? obj)
    {
        return Equals(obj as LowPriorityRucksackItem);
    }

    public bool Equals(LowPriorityRucksackItem? other)
    {
        return other is not null &&
               Value == other.Value &&
               Priority == other.Priority;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Priority);
    }
}

class HighPriorityRucksackItem : IRucksackItem, IEquatable<HighPriorityRucksackItem?>
{
    public  required char Value { get; init; }

    public int Priority => Value - 'A' + 27;

    public override bool Equals(object? obj)
    {
        return Equals(obj as HighPriorityRucksackItem);
    }

    public bool Equals(HighPriorityRucksackItem? other)
    {
        return other is not null &&
               Value == other.Value &&
               Priority == other.Priority;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Priority);
    }
}