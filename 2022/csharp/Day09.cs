﻿class Day09 : Puzzle
{
    protected override string Part1(string[] inputLines)
    {
        return TailCount(inputLines, new Rope(length: 2));
    }

    private static string TailCount(string[] inputLines, Rope rope)
    {
        foreach (var line in inputLines)
        {
            var movement = Rope.Create(movement: line);

            rope.Move(movement);
        }

        return rope.TailCount().ToString();
    }

    protected override string Part2(string[] inputLines)
    {
        return TailCount(inputLines, new Rope(length: 10));
    }
}

enum Direction
{
    Up = 'U',
    Down = 'D',
    Left = 'L',
    Right = 'R'
}

record Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public double Distance(Position other)
    {
        return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }
}

record Movement(Direction Direction, int Steps);

class Rope
{
    private static readonly int[] units = { 0, 1, -1 };

    private readonly List<Position> knots = new();

    private Position Head => knots.First();
    private Position Tail => knots.Last();

    private readonly HashSet<Position> tailsPositions = new()
    {
        new Position { X = 0, Y = 0 }
    };

    public Rope(int length = 2)
    {
        for (var i = 0; i < length; i++)
        {
            knots.Add(new Position { X = 0, Y = 0 });
        }
    }

    public void Move(Movement movement)
    {
        for (var i = 0; i < movement.Steps; i++)
        {
            MoveHead(movement.Direction);

            for (var j = 1; j < knots.Count; j++)
            {
                MoveKnot(knots[j], knots[j - 1]);
            }

            tailsPositions.Add(Tail);
        }
    }

    public int TailCount() => tailsPositions.Count;

    public static Movement Create(string movement)
    {
        var tokens = movement.Split(" ");

        return new Movement(Direction: (Direction)tokens[0][0], Steps: int.Parse(tokens[1]));
    }

    private void MoveHead(Direction direction)
    {
        var _ = direction switch
        {
            Direction.Up => Head.Y++,
            Direction.Down => Head.Y--,
            Direction.Left => Head.X--,
            Direction.Right => Head.X++,
            _ => throw new NotImplementedException(),
        };
    }

    private static void MoveKnot(Position knot, Position reference)
    {
        if (IsTouching(knot, reference)) return;

        if (reference.X == knot.X)
        {
            knot.Y += Math.Sign(reference.Y - knot.Y);
        }
        else if (reference.Y == knot.Y)
        {
            knot.X += Math.Sign(reference.X - knot.X);
        }
        else if (reference.Distance(knot) > 1)
        {
            knot.Y += Math.Sign(reference.Y - knot.Y);
            knot.X += Math.Sign(reference.X - knot.X);
        }
    }

    private static bool IsTouching(Position knot, Position reference)
    {
        foreach (var x in units)
        {
            foreach (var y in units)
            {
                if (knot.X + x == reference.X && knot.Y + y == reference.Y) return true;
            }
        }
        return false;
    }
}