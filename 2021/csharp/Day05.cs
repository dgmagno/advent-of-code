class Day05 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        var lineSegments = CreateLineSegments(inputLines);

        var points = GetPoints(lineSegments, includeDiagonals: false);

        return CountOverlaps(points);
    }

    private static LineSegment[] CreateLineSegments(string[] inputLines)
    {
        return inputLines
            .Select(it => it.Replace(" -> ", ",").Split(",").Select(int.Parse).ToArray())
            .Select(it => new { A = new Point(it[0], it[1]), B = new Point(it[2], it[3]) })
            .Select(it => new LineSegment(it.A, it.B))
            .ToArray();
    }

    private static IEnumerable<Point> GetPoints(LineSegment[] lineSegments, bool includeDiagonals)
    {
        return lineSegments
            .Where(it => includeDiagonals || !it.IsDiagonal())
            .SelectMany(it => it.Rasterize());
    }

    private static int CountOverlaps(IEnumerable<Point> points)
    {
        return points
            .GroupBy(it => it)
            .Where(it => it.Count() > 1)
            .Count();
    }

    protected override long Part2(string[] inputLines)
    {
        var lineSegments = CreateLineSegments(inputLines);

        var points = GetPoints(lineSegments, includeDiagonals: true);

        return CountOverlaps(points);
    }
}

record LineSegment(Point A, Point B)
{
    public bool IsDiagonal() => A.X != B.X && A.Y != B.Y;

    // DDA Algorithm (Digital differential analyzer)
    public Point[] Rasterize()
    {
        var dx = B.X - A.X;
        var dy = B.Y - A.Y;

        var length = Math.Abs(dx) >= Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);

        dx /= length;
        dy /= length;

        var points = new List<Point>() { A };

        for (var i = 0; i < length; i++)
        {
            var point = points.Last();

            points.Add(new(point.X + dx, point.Y + dy));
        }

        return points.ToArray();
    }
}

record Point(int X, int Y);