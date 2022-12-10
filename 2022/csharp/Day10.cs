class Day10 : Puzzle
{
    private readonly Dictionary<string, int> commandCycles = new()
    {
        ["addx"] = 2,
        ["noop"] = 1,
    };

    protected override string Part1(string[] inputLines)
    {
        var parsedInputs = inputLines
            .Select(it => it.Split(" "))
            .Select(it => it.Length == 1 ? (1, 0) : (commandCycles[it[0]], int.Parse(it[1])));

        int cycles = 1;
        int register = 1;
        int signal = 0;

        foreach (var (cycle, value) in parsedInputs)
        {
            for (int i = 0; i < cycle; i++)
            {
                if (cycles == 20 || (20 + cycles) % 40 == 0)
                {
                    signal += cycles * register;
                }

                cycles++;
            }
            register += value;
        }

        return signal.ToString();
    }

    protected override string Part2(string[] inputLines)
    {
        var crt = new char[6][];

        for (int i = 0; i < crt.Length; i++)
        {
            crt[i] = new char[40];
        }

        var parsedInputs = inputLines
           .Select(it => it.Split(" "))
           .Select(it => it.Length == 1 ? (1, 0) : (commandCycles[it[0]], int.Parse(it[1])));

        int cycles = 1;
        int register = 1;

        var sprite = DrawSprite(register);

        foreach (var (cycle, value) in parsedInputs)
        {
            for (int count = 0; count < cycle; count++)
            {
                var i = (cycles - 1) / 40;
                var j = (cycles - 1) % 40;

                crt[i][j] = sprite[j];

                cycles++;
            }

            register += value;

            sprite = DrawSprite(register);
        }

        foreach (var crtLines in crt)
        {
            Console.WriteLine(string.Concat(crtLines));
        }

        return string.Concat(crt.SelectMany(it => it));
    }

    private static char[] DrawSprite(int register)
    {
        static bool IsValid(int x) => x >= 0 && x < 40;

        var sprite = Enumerable.Repeat('.', 40).ToArray();

        for (int i = 0; i < 3; i++)
        {
            if (IsValid(register + i - 1)) sprite[register + i - 1] = '#';
        }

        return sprite;
    }
}