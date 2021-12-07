class Day07 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        var positions = inputLines[0]
            .Split(",")
            .Select((n, i) => (Id: i, Position: int.Parse(n)))
            .ToDictionary(it => it.Id, it => it.Position);

        var fuels = new Dictionary<int, int>(positions.Select((n, i) => KeyValuePair.Create(n.Key, 0)));

        foreach (var i in positions.Keys)
        {
            foreach (var j in positions.Keys)
            {
                fuels[i] += Math.Abs(positions[i] - positions[j]);
            }
        }

        return fuels.Values.Min();
    }

    protected override long Part2(string[] inputLines)
    {
        var positions = inputLines[0]
            .Split(",")
            .Select((n, i) => (Id: i, Position: int.Parse(n)))
            .ToDictionary(it => it.Id, it => it.Position);

        var minDistance = positions.Values.Min();
        var maxDistance = positions.Values.Max();

        var fuels = new Dictionary<int, int>();

        for (var i = minDistance; i <= maxDistance; i++)
        {
            fuels[i] = 0;
        }

        foreach (var position in fuels.Keys)
        {
            foreach (var crabId in positions.Keys)
            {
                var distance = Math.Abs(position - positions[crabId]);
                var fuel = distance * (1 + distance) / 2;

                fuels[position] += fuel;
            }
        }

        return fuels.Values.Min();
    }
}