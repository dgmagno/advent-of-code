class Day09 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        var matrix = CreateMatrix(inputLines);

        var lowPoints = new List<int>();

        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                if (IsLow(matrix, i, j))
                {
                    lowPoints.Add(matrix[i][j]);
                };
            }
        }

        return lowPoints.Sum(it => it + 1);
    }

    private static int[][] CreateMatrix(string[] inputLines)
    {
        return inputLines
            .Select(line => line.ToArray())
            .Select(digits => digits.Select(it => int.Parse(it.ToString())).ToArray())
            .ToArray();
    }

    private static bool IsLow(int[][] matrix, int x, int y)
    {
        return new[]
        {
            (X: x - 1, Y: y),
            (X: x + 1, Y: y),
            (X: x, Y: y - 1),
            (X: x, Y: y + 1),
        }
        .Where(point => point.X >= 0 && point.Y >= 0)
        .Where(point => point.X < matrix.Length && point.Y < matrix[0].Length)
        .All(point => matrix[x][y] < matrix[point.X][point.Y]);
    }


    protected override long Part2(string[] inputLines)
    {
        throw new NotImplementedException();
    }
}