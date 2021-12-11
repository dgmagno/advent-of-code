class Day11 : Puzzle
{
    private const int GridSize = 10;

    protected override long Part1(string[] inputLines)
    {
        var octopuses = CreateOctopuses(inputLines);

        for (var i = 0; i < 100; i++)
        {
            foreach (var needsToFlash in octopuses.Where(it => it.Tick()))
            {
                needsToFlash.Flash();
            }

            foreach (var octopus in octopuses)
            {
                octopus.Reset();
            }
        }

        return octopuses.Sum(it => it.FlashCount);
    }

    private static Octopus[] CreateOctopuses(string[] inputLines)
    {
        var octopuses = new Dictionary<(int, int), Octopus>();

        for (var i = 0; i < GridSize; i++)
        {
            for (var j = 0; j < GridSize; j++)
            {
                octopuses.TryAdd((i, j), new Octopus(inputLines[i][j] - '0'));

                var octopus = octopuses[(i, j)];

                foreach (var (x, y) in CreateAdjacents(i, j, GridSize, GridSize))
                {
                    octopuses.TryAdd((x, y), new Octopus(inputLines[x][y] - '0'));

                    octopus.AddAdjacent(octopuses[(x, y)]);
                }
            }
        }

        return octopuses.Values.ToArray();
    }

    private static IEnumerable<(int, int)> CreateAdjacents(int x, int y, int maxX, int maxY)
    {
        return new[]
        {
            (X: x - 1, Y: y),
            (X: x + 1, Y: y),
            (X: x, Y: y - 1),
            (X: x, Y: y + 1),
            (X: x - 1, Y: y - 1),
            (X: x + 1, Y: y + 1),
            (X: x + 1, Y: y - 1),
            (X: x - 1, Y: y + 1),
        }
        .Where(point => point.X >= 0 && point.Y >= 0)
        .Where(point => point.X < maxX && point.Y < maxY);
    }

    protected override long Part2(string[] inputLines)
    {
        var octopuses = CreateOctopuses(inputLines);

        for (var i = 1; ; i++)
        {
            foreach (var needsToFlash in octopuses.Where(it => it.Tick()))
            {
                needsToFlash.Flash();
            }

            foreach (var octopus in octopuses)
            {
                octopus.Reset();
            }

            if (octopuses.All(it => it.Energy == 0))
            {
                return i;
            }
        }

        throw new InvalidOperationException("Step not found");
    }
}

class Octopus
{
    public int Energy { get; private set; }

    private bool flashed;

    private readonly HashSet<Octopus> adjacents = new();

    public int FlashCount { get; private set; }

    public Octopus(int energy)
    {
        Energy = energy;
    }

    public void AddAdjacent(Octopus octopus) => adjacents.Add(octopus);

    public bool Tick()
    {
        Energy++;

        return Energy > 9 && !flashed;
    }

    public void Flash()
    {
        flashed = true;

        foreach (var adjacent in adjacents.Where(it => it.Tick()))
        {
            adjacent.Flash();
        }
    }

    public void Reset()
    {
        flashed = false;

        if (Energy > 9)
        {
            Energy = 0;
            FlashCount++;
        }
    }
}