class Day10 : Puzzle
{
    private static readonly Dictionary<char, int> score = new()
    {
        [')'] = 3,
        [']'] = 57,
        ['}'] = 1197,
        ['>'] = 25137,
    };

    private static readonly Dictionary<char, char> closingBrackets = new()
    {
        [')'] = '(',
        [']'] = '[',
        ['}'] = '{',
        ['>'] = '<',
    };

    private static readonly Dictionary<char, char> openingBrackets = new()
    {
        ['('] = ')',
        ['['] = ']',
        ['{'] = '}',
        ['<'] = '>',
    };

    protected override long Part1(string[] inputLines)
    {
        var result = 0;

        foreach (var line in inputLines)
        {
            var brackets = new LinkedList<char>();

            foreach (var bracket in line)
            {
                if (openingBrackets.TryGetValue(bracket, out var closingBracket))
                {
                    brackets.AddLast(bracket);
                }
                else
                {
                    if (closingBrackets[bracket] != brackets.Last())
                    {
                        result += score[bracket];
                        break;
                    }
                    else
                    {
                        brackets.RemoveLast();
                    }
                }
            }
        }

        return result;
    }

    protected override long Part2(string[] inputLines)
    {
        throw new NotImplementedException();
    }
}