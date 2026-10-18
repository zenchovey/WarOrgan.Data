using System.IO.Compression;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using WarOrgan.Model.DataSet;

namespace WarOrgan.Data.BuildDataset.Utils;

public static class IoUtils
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        AllowDuplicateProperties = false,
    };

    public static void Save(this DataSetInfo info, string folderPath)
        => SaveJson(info, folderPath, DataSetInfo.FileName);
    public static void Save(this DataSetDefinition definition, string folderPath)
        => SaveJson(definition, folderPath, DataSetDefinition.FileName);
    public static void Save(this FactionRosterEntry rosterEntry, string folderPath)
        => SaveJson(rosterEntry.Roster, folderPath, rosterEntry.NameOverride?.Invoke(rosterEntry.Roster) ?? $"{rosterEntry.Roster.Name}.json");

    private static void SaveJson<T>(T value, string folderPath, string filename)
    {
        Directory.CreateDirectory(folderPath);
        File.WriteAllText(Path.Combine(folderPath, filename), JsonSerializer.Serialize(value, _jsonOptions));
    }

    public static T? LoadRequiredFile<T>(string filePath) where T : class
    {
        try
        {
            using var stream = File.OpenRead(filePath);
            return JsonSerializer.Deserialize<T>(stream, _jsonOptions)
                ?? throw new JsonException($"Deserialize returned null");
        }
        catch (Exception ex)
        {
            LoggingBuffer.Error($"Failed to load required file \"{filePath}\": {ex.Message}");
            return null;
        }
    }

    public static void CreateJsonZip(string sourceDirectory, string destinationZipPath, CompressionLevel level = CompressionLevel.Optimal)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(destinationZipPath))!);
        using var zipStream = File.Create(destinationZipPath);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);

        var files = Directory.EnumerateFiles(sourceDirectory, "*.json", SearchOption.AllDirectories)
            .Concat(Directory.EnumerateFiles(sourceDirectory, "README.md"))
            .Order(StringComparer.Ordinal);

        foreach (var file in files)
        {
            var entryName = Path.GetRelativePath(sourceDirectory, file).Replace('\\', '/');
            archive.CreateEntryFromFile(file, entryName, level);
        }
    }

    public static string EnsureDirectoryExists(this string directory)
    {
        Directory.CreateDirectory(directory);
        return directory;
    }

    public static IDisposable CreateTemporaryDirectory(string prefix, out string tempPath)
    {
        var path = tempPath = Directory.CreateTempSubdirectory(prefix).FullName;
        return new DisposableAction(() => Directory.Delete(path, recursive: true));
    }

    private class DisposableAction(Action DisposeAction) : IDisposable
    {
        public void Dispose() => DisposeAction();
    }
}
