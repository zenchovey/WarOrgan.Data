namespace WarOrgan.Data.BuildDataset.Utils;

public static class LoggingBuffer
{
    private static readonly Dictionary<string, List<string>> _msgs = new();

    public static void Info(string message) => Custom("INFOS", message);
    public static void Warn(string message) => Custom("WARNINGS", message);
    public static void Error(string message) => Custom("ERRORS", message);
    public static void Custom(string key, string message)
    {
        if (!_msgs.ContainsKey(key))
            _msgs[key] = new List<string>();
        _msgs[key].Add(message);
    }

    public static int ErrorCount => _msgs.TryGetValue("ERRORS", out var errors) ? errors.Count : 0;
    public static int WarningCount => _msgs.TryGetValue("WARNINGS", out var warnings) ? warnings.Count : 0;

    public static void Print(bool waitForInput = true)
    {
        var infos = _msgs.Where(p => p.Key == "INFOS").SelectMany(x => x.Value).ToList();
        var warnings = _msgs.Where(p => p.Key == "WARNINGS").SelectMany(x => x.Value).ToList();
        var errors = _msgs.Where(p => p.Key == "ERRORS").SelectMany(x => x.Value).ToList();

        if (infos.Any())
            Console.WriteLine($"\nLOGGING.INFOS");
        foreach (var info in infos)
            Console.WriteLine(info);

        if (warnings.Any())
            Console.WriteLine($"\nLOGGING.WARNINGS");
        foreach (var info in warnings)
            Console.WriteLine(info);

        if (errors.Any())
            Console.WriteLine($"\nLOGGING.ERRORS");
        foreach (var info in errors)
            Console.WriteLine(info);

        var errorCount = _msgs.ContainsKey("ERRORS") ? _msgs["ERRORS"].Count : 0;
        var warningCount = _msgs.ContainsKey("WARNINGS") ? _msgs["WARNINGS"].Count : 0;

        Console.WriteLine($"\nCompleted with {warningCount} warnings and {errorCount} errors.");
        if (waitForInput && (errorCount > 0 || warningCount > 0))
            Console.ReadLine();
        _msgs.Clear();
    }
}
