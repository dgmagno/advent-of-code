using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Day04 : Puzzle
{
    private static readonly Regex regex = new(@"(\d+)-(\d+),(\d+)-(\d+)", RegexOptions.Compiled);

    protected override string Part1(string[] inputLines)
    {
        return Create(inputLines)
            .Count(pairs => pairs[0].Contains(pairs[1]) || pairs[1].Contains(pairs[0]))
            .ToString();
    }

    private static IEnumerable<Pair[]> Create(string[] inputLines)
    {
        return inputLines
            .Select(it => regex.Match(it).Groups)
            .Select(g => new[]
            {
                new Pair(int.Parse(g[1].Value), int.Parse(g[2].Value)),
                new Pair(int.Parse(g[3].Value), int.Parse(g[4].Value))
            });
    }

    protected override string Part2(string[] inputLines)
    {
        return Create(inputLines)
            .Count(pairs => pairs[0].Overlaps(pairs[1]) || pairs[1].Overlaps(pairs[0]))
            .ToString();
    }
}

record Pair(int Start, int End)
{
    public bool Contains(Pair pair)
    {
        return pair.Start >= Start && pair.End <= End;
    }

    public bool Overlaps(Pair pair)
    {
        return pair.End >= Start && End >= pair.Start;
    }
}