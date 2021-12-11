class Day10 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        var score = new Dictionary<char, int>()
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137,
        };

        return inputLines
            .Select(it => ChunkFactory.Create(it))
            .OfType<CorruptedChunk>()
            .Sum(chunk => score[chunk.CorruptedBracket]);
    }

    protected override long Part2(string[] inputLines)
    {
        var score = new Dictionary<char, int>()
        {
            [')'] = 1,
            [']'] = 2,
            ['}'] = 3,
            ['>'] = 4,
        };

        var scores = inputLines
            .Select(it => ChunkFactory.Create(it))
            .OfType<IncompleteChunk>()
            .Select(chunk => chunk.MissingBrackets.Aggregate(0L, (acc, it) => (5 * acc) + score[it]))
            .OrderBy(it => it)
            .ToArray();

        return scores[scores.Length / 2];
    }
}

abstract record Chunk();

record CorruptedChunk(char CorruptedBracket) : Chunk;

record IncompleteChunk(IReadOnlyList<char> MissingBrackets) : Chunk;

class ChunkFactory
{
    private static readonly Dictionary<char, char> openingPairs = new()
    {
        ['('] = ')',
        ['['] = ']',
        ['{'] = '}',
        ['<'] = '>',
    };

    public static readonly Dictionary<char, char> closingPairs = new()
    {
        [')'] = '(',
        [']'] = '[',
        ['}'] = '{',
        ['>'] = '<',
    };

    public static Chunk Create(string line)
    {
        var brackets = new LinkedList<char>();

        for (var i = 0; i < line.Length; i++)
        {
            var bracket = line[i];

            if (openingPairs.ContainsKey(bracket))
            {
                brackets.AddLast(bracket);
            }
            else if(closingPairs[bracket] != brackets.Last())
            {
                return new CorruptedChunk(bracket);
            }
            else
            {
                brackets.RemoveLast();
            }
        }

        var missingBrackets = brackets
            .Select(it => openingPairs[it])
            .Reverse()
            .ToArray();

        return new IncompleteChunk(missingBrackets);
    }
}