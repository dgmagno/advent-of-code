using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Day06 : Puzzle
{
    protected override string Part1(string[] inputLines)
    {
        return FindMarker(inputLines[0].AsSpan(), 4).ToString();
    }

    private static int FindMarker(ReadOnlySpan<char> input, int size)
    {
        var set = new HashSet<char>();

        for (var i = 0; i < input.Length; i++)
        {
            var slice = input.Slice(i, size);

            for (var j = 0; j < size; j++)
            {
                set.Add(slice[j]);
            }

            if (set.Count == size)
            {
                return i + size;
            }

            set.Clear();
        }

        throw new Exception();
    }

    protected override string Part2(string[] inputLines)
    {
        return FindMarker(inputLines[0].AsSpan(), 14).ToString();
    }
}
