using System.Collections;

class Day08 : Puzzle
{
    protected override string Part1(string[] inputLines)
    {
        var trees = inputLines;

        var size = trees.Length;

        var result = 0;

        bool IsVisible(string[] trees, int i, int j)
        {
            var tree = trees[i][j];

            var isVisible = true;
            for (var k = j - 1; k >= 0; k--) // left
            {
                isVisible &= tree > trees[i][k];
            }
            if (isVisible) return true;

            isVisible = true;
            for (var k = j + 1; k < size; k++) // right
            {
                isVisible &= tree > trees[i][k];
            }
            if (isVisible) return true;

            isVisible = true;
            for (var k = i - 1; k >= 0; k--) // up
            {
                isVisible &= tree > trees[k][j];
            }
            if (isVisible) return true;

            isVisible = true;
            for (var k = i + 1; k < size; k++) // down
            {
                isVisible &= tree > trees[k][j];
            }

            return isVisible;
        }

        for (var i = 1; i < size - 1; i++)
        {
            for (var j = 1; j < size - 1; j++)
            {
                if (IsVisible(trees, i, j)) result++;
            }
        }

        return (result + (4 * size) - 4).ToString();
    }

    protected override string Part2(string[] inputLines)
    {
        var trees = inputLines;

        var size = trees.Length;

        var result = 0;

        for (var i = 1; i < size - 1; i++)
        {
            for (int j = 1; j < size - 1; j++)
            {
                int k;
                int count;
                var scenicScore = 1;

                var tree = trees[i][j];

                for (k = j - 1, count = 0; k >= 0; k--) // left
                {
                    count++;
                    if (tree <= trees[i][k]) break;
                }
                scenicScore *= count;

                for (k = j + 1, count = 0; k < size; k++) // right
                {
                    count++;
                    if (tree <= trees[i][k]) break;
                }
                scenicScore *= count;

                for (k = i - 1, count = 0; k >= 0; k--) // up
                {
                    count++;
                    if (tree <= trees[k][j]) break;
                }
                scenicScore *= count;

                for (k = i + 1, count = 0; k < size; k++) // down
                {
                    count++;
                    if (tree <= trees[k][j]) break;
                }
                scenicScore *= count;

                result = Math.Max(result, scenicScore);
            }
        }

        return result.ToString();
    }
}
