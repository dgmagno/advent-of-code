class Day06 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        return FishGeneration(inputLines, 80);
    }

    private static long FishGeneration(string[] inputLines, int days)
    {
        var fishbowl = new long[9];

        foreach (var fishId in inputLines[0].Split(",").Select(int.Parse))
        {
            fishbowl[fishId]++;
        }

        for (var day = 0; day < days; day++)
        {
            var borns = fishbowl[0];

            for (int fishId = 0; fishId <= 7; fishId++)
            {
                fishbowl[fishId] = fishbowl[fishId + 1];
            }

            fishbowl[6] += borns;
            fishbowl[8] = borns;
        }

        return fishbowl.Sum();
    }

    protected override long Part2(string[] inputLines)
    {
        return FishGeneration(inputLines, 256);
    }
}
