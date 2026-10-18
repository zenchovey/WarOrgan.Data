using WarOrgan.Model.DataSet;
using WarOrgan.Model.Faction;

namespace WarOrgan.Data.BuildDataset.Utils;

public static class AssetUtils
{
    public static DatasetBuildModel CreateDatasetBuildModel(this DataSetInfo datasetInfo, string assetsFolder)
    {
        var dataSetDefinitionFilePath = Path.Combine(assetsFolder, DataSetDefinition.FileName);

        if (!DataSetValidation.IsValidDataSetId(datasetInfo.Id))
            throw new Exception($"Invalid dataset ID: {datasetInfo.Id}");

        if (IoUtils.LoadRequiredFile<DataSetDefinition>(dataSetDefinitionFilePath) is not { } definition)
            throw new FileNotFoundException($"Failed to find data set definition at \"{dataSetDefinitionFilePath}\"");

        return new(datasetInfo, definition, new(), new());
    }

    public static DatasetBuildModel AddRosters(
        this DatasetBuildModel dataset,
        string rostersFolder,
        Func<FactionRoster, string>? nameOverride = null)
    {
        var rosters = Directory.GetFiles(rostersFolder, "*.json")
            .Order(StringComparer.Ordinal)
            .Select(path => new { FileName = Path.GetFileName(path), Roster = IoUtils.LoadRequiredFile<FactionRoster>(path) })
            .Where(item => item.Roster != null)
            .Select(item => new FactionRosterEntry(item.Roster!, nameOverride ?? (_ => item.FileName)))
            .ToList();

        dataset.Rosters.AddRange(rosters);
        return dataset;
    }

    /// <summary>
    /// Save all the things and compress into a .zip
    /// </summary>
    public static DatasetBuildModel PublishToZipArchive(this DatasetBuildModel dataset, string outputFolderPath)
    {
        using var _ = IoUtils.CreateTemporaryDirectory("WarOrgan-dataset-", out var buildFolder);

        dataset.Info.Save(buildFolder);
        dataset.Definition.Save(buildFolder);
        foreach (var item in dataset.Rosters)
            item.Save(buildFolder);

        foreach (var file in dataset.AdditionalFiles)
        {
            if (File.Exists(file))
                File.Copy(file, Path.Combine(buildFolder, Path.GetFileName(file)));
            else LoggingBuffer.Error($"Failed to find additional file \"{file}\"");
        }

        var dataSetZipFilePath = Path.Combine(outputFolderPath, $"{dataset.Info.Id}.zip");
        IoUtils.CreateJsonZip(buildFolder, dataSetZipFilePath);
        LoggingBuffer.Info($"Finished creating {dataset.Info.Id} dataset, version {dataset.Info.Version}: \"{dataSetZipFilePath}\"");

        return dataset;
    }

    public static DatasetBuildModel IncludeReadme(this DatasetBuildModel dataset, string assetsFolder)
    {
        return dataset.WithAdditionalFiles(Path.Combine(assetsFolder, "README.md"));
    }

    public static DatasetBuildModel WithAdditionalFiles(this DatasetBuildModel dataset, params string[] files)
    {
        dataset.AdditionalFiles.AddRange(files);
        return dataset;
    }
}
