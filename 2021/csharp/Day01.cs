class Day01 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        var measurements = ParseInput(inputLines);

        return CountIncreasingMeasurements(measurements, windowSize: 1);
    }

    private static int[] ParseInput(string[] inputLines)
    {
        return inputLines
            .Select(int.Parse)
            .ToArray();
    }

    private static int CountIncreasingMeasurements(int[] measurements, int windowSize)
    {
        var count = 0;

        for (var i = 0; i < measurements.Length - windowSize; i++)
        {
            var currentSum = measurements.Skip(i).Take(windowSize).Sum();
            var nextSum = measurements.Skip(i + 1).Take(windowSize).Sum();

            if (nextSum > currentSum)
            {
                count++;
            }
        }

        return count;
    }

    protected override long Part2(string[] inputLines)
    {
        var measurements = ParseInput(inputLines);

        return CountIncreasingMeasurements(measurements, windowSize: 3);
    }
}