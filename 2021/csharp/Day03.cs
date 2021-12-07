class Day03 : Challenge
{
    protected override long Part1(string[] inputLines)
    {
        var bitCounts = ParseInput(inputLines);

        for (var i = 0; i < bitCounts.Length; i++)
        {
            for (var j = 0; j < inputLines.Length; j++)
            {
                bitCounts[i].Increment(inputLines[j][i]);
            }
        }

        var gamma = new string(bitCounts.Select(it => it.ZeroAsCriteria()).ToArray());
        var epsilon = new string(bitCounts.Select(it => it.OneAsCriteria()).ToArray());

        return Convert.ToInt32(gamma, fromBase: 2) * Convert.ToInt32(epsilon, fromBase: 2);
    }

    private static BitCount[] ParseInput(string[] inputLines)
    {

        var bitCounts = inputLines[0].Select(it => new BitCount()).ToArray();

        return bitCounts;
    }

    protected override long Part2(string[] inputLines)
    {
        int GetLifeRating(string[] inputLines, Func<BitCount, char> bitCriteria)
        {
            var bitCounts = ParseInput(inputLines);

            for (var i = 0; i < bitCounts.Length && inputLines.Length > 1; i++)
            {
                for (var j = 0; j < inputLines.Length; j++)
                {
                    bitCounts[i].Increment(inputLines[j][i]);
                }

                inputLines = inputLines.Where(it => it[i] == bitCriteria(bitCounts[i])).ToArray();
            }

            return Convert.ToInt32(inputLines[0], fromBase: 2);
        }

        var oxygenRating = GetLifeRating(inputLines, bitCount => bitCount.ZeroAsCriteria());
        var co2Rating = GetLifeRating(inputLines, bitCount => bitCount.OneAsCriteria());

        return oxygenRating * co2Rating;
    }
}

class BitCount
{
    protected int One { get; set; }
    protected int Zero { get; set; }

    public void Increment(char bit)
    {
        if (bit == '1')
        {
            One++;
        }
        else
        {
            Zero++;
        }
    }

    public char ZeroAsCriteria() => Zero > One ? '0' : '1';

    public char OneAsCriteria() => Zero <= One ? '0' : '1';
}