using System.Runtime.CompilerServices;

static class Paths
{
    public static readonly string Inputs;

    static Paths()
    {
        var path = GetSourceFilePathName();
        var projectPath = Directory.GetParent(path)!.FullName;

        Inputs = Path.Combine(projectPath, "..", "inputs");
    }

    private static string GetSourceFilePathName([CallerFilePath] string? callerFilePath = null) => callerFilePath ?? "";
}