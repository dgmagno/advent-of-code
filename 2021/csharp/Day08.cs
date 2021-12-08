class Day08 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        static int CountUniquePatterns(ILookup<int, string> patterns)
        {
            var uniqueNumbers = new HashSet<int> { 2, 3, 4, 7 };

            return patterns.Where(it => uniqueNumbers.Contains(it.Key)).Sum(it => it.Count());
        }

        var entries = CreateEntries(inputLines);

        return entries.Sum(it => CountUniquePatterns(it.OutputValues));
    }

    private static Entry[] CreateEntries(string[] inputLines)
    {
        return inputLines
            .Select(it => it.Split("|"))
            .Select(it => new Entry(it[0], it[1]))
            .ToArray();
    }
    protected override long Part2(string[] inputLines)
    {
        throw new NotImplementedException();
    }
}

class Entry
{
    public ILookup<int, string> SignalPatterns { get; }
    public ILookup<int, string> OutputValues { get; }

    public Entry(string signalPatterns, string outputValues)
    {
        SignalPatterns = signalPatterns.Split(" ").ToLookup(it => it.Length);
        OutputValues = outputValues.Split(" ").ToLookup(it => it.Length);
    }
}
