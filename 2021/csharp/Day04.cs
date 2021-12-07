class Day04 : Puzzle
{
    protected override long Part1(string[] inputLines)
    {
        var (numbers, boards) = ParseInput(inputLines);

        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                if (board.Mark(number))
                {
                    return board.Score() * number;
                }
            }
        }

        throw new Exception("not found");
    }

    private static (int[], Board[]) ParseInput(string[] inputLines)
    {
        var numbers = inputLines[0]
            .Split(",")
            .Select(int.Parse)
            .ToArray();

        var boards = new List<Board>();

        for (var i = 2; i < inputLines.Length; i += 1 + Board.Size)
        {
            var boardNumbers = new List<int[]>();

            for (var j = 0; j < Board.Size; j++)
            {
                var boardRow = inputLines[i + j]
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                boardNumbers.Add(boardRow);
            }

            boards.Add(new Board(boardNumbers.ToArray()));
        }

        return (numbers, boards.ToArray());
    }

    protected override long Part2(string[] inputLines)
    {
        var (numbers, boards) = ParseInput(inputLines);

        var wonBoards = new List<Board>();

        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                if (board.Mark(number))
                {
                    wonBoards.Add(board);
                }
            }

            boards = boards.Except(wonBoards).ToArray();

            if (boards.Length == 0)
            {
                return wonBoards.Last().Score() * number;
            }
        }

        throw new Exception("not found");
    }
}

class Board
{
    public const int Size = 5;

    private readonly Dictionary<int, (int, int)> numbers = new();

    private readonly List<int> markedNumbers = new();

    private readonly Dictionary<int, int> markedCountX = new();
    private readonly Dictionary<int, int> markedCountY = new();

    public Board(int[][] boardNumbers)
    {
        for (var x = 0; x < Size; x++)
        {
            markedCountX.Add(x, 0);
            markedCountY.Add(x, 0);

            for (var y = 0; y < Size; y++)
            {
                numbers.Add(boardNumbers[x][y], (x, y));
            }
        }
    }

    public int Score() => numbers.Keys.Except(markedNumbers).Sum();

    public bool Mark(int number)
    {
        if (!numbers.ContainsKey(number))
        {
            return false;
        }

        markedNumbers.Add(number);

        var (x, y) = numbers[number];

        return Mark(x, markedCountX) || Mark(y, markedCountY);
    }

    private static bool Mark(int position, Dictionary<int, int> line)
    {
        line[position]++;

        return line[position] == Size;
    }
}
