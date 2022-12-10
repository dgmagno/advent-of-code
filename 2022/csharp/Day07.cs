class Day07 : Puzzle
{
    protected override string Part1(string[] inputLines)
    {
        var fileSystem = FileSystemFactory.Create(consoleLines: inputLines);

        var sum = fileSystem.Directories
            .Select(it => it.Size())
            .Where(it => it <= 100000)
            .Sum();

        return sum.ToString();
    }

    protected override string Part2(string[] inputLines)
    {
        var fileSystem = FileSystemFactory.Create(consoleLines: inputLines);

        var unusedSpace = 70000000 - fileSystem.Root.Size();

        var size = 30000000 - unusedSpace;

        var min = fileSystem.Directories
            .Select(it => it.Size())
            .Where(it => it >= size)
            .Min();

        return min.ToString();
    }
}

class FileSystemFactory
{
    public static FileSystem Create(string[] consoleLines)
    {
        var fileSystem = new FileSystem();

        for (var i = 1; i < consoleLines.Length; i++)
        {
            var lineTokens = consoleLines[i].Split(" ");

            if (lineTokens[0] == "$")
            {
                if (lineTokens[1] == "cd" && lineTokens[2] == "..") fileSystem.GoBack();

                if (lineTokens[1] == "cd" && lineTokens[2] != "..") fileSystem.Enter(lineTokens[2]);
            }
            else if (lineTokens[0] == "dir")
            {
                fileSystem.Add(new DirectoryNode(
                    parent: fileSystem.Current, 
                    name: lineTokens[1]));
            }
            else
            {
                fileSystem.Add(new FileNode(
                    parent: fileSystem.Current, 
                    name: lineTokens[1], 
                    size: long.Parse(lineTokens[0])));
            }
        }

        return fileSystem;
    }
}

class FileSystem
{
    private readonly Dictionary<string, INode> nodes = new();

    public IDirectory Current { get; private set; }

    public IEnumerable<IDirectory> Directories => nodes.Values.OfType<IDirectory>();

    public IDirectory Root { get; }

    public FileSystem()
    {
        nodes = new Dictionary<string, INode>();

        Root = new DirectoryNode(parent: null, name: "");

        Current = Root;

        nodes[Root.FullPath] = Root;
    }

    public void Add(INode node)
    {
        Current.AddChild(node);

        nodes[node.FullPath] = node;
    }

    public void GoBack() => Current = Current.Parent ?? throw new InvalidOperationException();

    public void Enter(string name) => Current = (IDirectory)nodes[$"{Current.FullPath}/{name}"];
}

interface IDirectory : INode
{
    void AddChild(INode childNode);
}

interface INode
{
    IDirectory? Parent { get; }

    string Name { get; }

    string FullPath { get; }
    
    long Size();
}

class FileNode : INode
{
    private readonly long size;

    public IDirectory Parent { get; }
    public string Name { get; }
    public string FullPath { get; }

    public FileNode(IDirectory parent, string name, long size)
    {
        Parent = parent;
        Name = name;

        FullPath = $"{parent.FullPath}/{name}";

        this.size = size;
    }

    public long Size() => size;
}

class DirectoryNode : IDirectory
{
    private readonly Dictionary<string, INode> children = new();

    public IDirectory? Parent { get; }

    public string Name { get; }
    public string FullPath { get; }

    public DirectoryNode(IDirectory? parent, string name)
    {
        Parent = parent;
        Name = name;

        FullPath = parent is null ? "" : $"{parent.FullPath}/{name}";
    }

    public long Size() => children.Values.Sum(it => it.Size());

    public void AddChild(INode childNode) => children.Add(childNode.FullPath, childNode);
}
