class Day08 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        static int CountUniquePatterns(IReadOnlyList<string> patterns)
        {
            var uniqueNumbers = new HashSet<int> { 2, 3, 4, 7 };

            return patterns
                .GroupBy(it => it.Length)
                .Where(it => uniqueNumbers.Contains(it.Key))
                .Sum(it => it.Count());
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
        var entries = CreateEntries(inputLines);

        return entries.Sum(it => it.DecodeOutput());
    }
}

class Entry
{
    public IReadOnlyList<string> SignalPatterns { get; }
    public IReadOnlyList<string> OutputValues { get; }

    public ILookup<int, string> EncodedNumbers { get; }

    public Entry(string signalPatterns, string outputValues)
    {
        static IReadOnlyList<string> ToList(string input)
        {
            return input.Split(" ")
            .Select(pattern => new string(pattern.OrderBy(it => it).ToArray()))
            .ToArray();
        }

        SignalPatterns = ToList(signalPatterns);
        OutputValues = ToList(outputValues);

        EncodedNumbers = SignalPatterns.Union(OutputValues).ToLookup(it => it.Length);
    }

    public int DecodeOutput()
    {
        var decoder = new Decoder(EncodedNumbers);

        var decodedNumbers = decoder.Decode();

        var digits = OutputValues
            .Where(it => decodedNumbers.ContainsKey(it))
            .Select(it => (char)('0' + decodedNumbers[it]))
            .ToArray();
                
        return int.Parse(new string(digits));
    }
}

class Decoder
{
    private readonly ILookup<int, string> encodedNumbers;
    private readonly IDictionary<int, string> decodedNumbers;

    public Decoder(ILookup<int, string> encodedNumbers)
    {
        this.encodedNumbers = encodedNumbers;

        decodedNumbers = new Dictionary<int, string>
        {
            [1] = encodedNumbers[2].Single(),
            [4] = encodedNumbers[4].Single(),
            [7] = encodedNumbers[3].Single(),
            [8] = encodedNumbers[7].Single()
        };
    }

    public IDictionary<string, int> Decode()
    {
        decodedNumbers[6] = encodedNumbers[6].Single(SixPredicate);
        decodedNumbers[9] = encodedNumbers[6].Single(NinePredicate);
        decodedNumbers[0] = encodedNumbers[6].Single(ZeroPredicate);
        decodedNumbers[5] = encodedNumbers[5].Single(FivePredicate);
        decodedNumbers[2] = encodedNumbers[5].Single(TwoPredicate);
        decodedNumbers[3] = encodedNumbers[5].Single(ThreePredicate);

        return decodedNumbers
            .Select(it => (Key: it.Value, Value: it.Key))
            .ToDictionary(it => it.Key, it => it.Value);
    }

    private bool SixPredicate(string encodedNumber)
    {
        return !decodedNumbers[8].Except(decodedNumbers[1]).Except(encodedNumber).Any();
    }

    private bool NinePredicate(string encodedNumber)
    {
        return decodedNumbers[8].Except(decodedNumbers[4]).Except(encodedNumber).Any();
    }

    private bool ZeroPredicate(string encodedNumber)
    {
        return encodedNumber != decodedNumbers[6] && encodedNumber != decodedNumbers[9];
    }

    private bool FivePredicate(string encodedNumber)
    {
        return decodedNumbers[6].Except(encodedNumber).Count() == 1;
    }

    private bool TwoPredicate(string encodedNumber)
    {
        return decodedNumbers[5].Except(encodedNumber).Count() == 2;
    }

    private bool ThreePredicate(string encodedNumber)
    {
        return decodedNumbers[5].Except(encodedNumber).Count() == 1;
    }
}
