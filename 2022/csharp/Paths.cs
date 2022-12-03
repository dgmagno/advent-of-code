using System.Runtime.CompilerServices;

static class Paths
{
    public static readonly string Inputs;
    public static readonly string DataFile;

    static Paths()
    {
        var path = GetSourceFilePathName();
        var projectPath = Directory.GetParent(path)!.FullName;

        Inputs = Path.Combine(projectPath, "..", "inputs");
        DataFile = Path.Combine(projectPath, ".data");

        if (!File.Exists(DataFile))
        {
            File.WriteAllText(DataFile, "\n");
        }
    }

    private static string GetSourceFilePathName([CallerFilePath] string? callerFilePath = null) => callerFilePath ?? "";
}