using WarOrgan.Data.BuildDataset.Utils;
using WarOrgan.Model.DataSet;
using WarOrgan.Model.Faction;
using WarOrgan.Model.Unit;

namespace WarOrgan.Data.BuildDataset.Validation;

// Copied/adapted from DataParser's roster validation, without game-specific rules.
public static class Validation
{
    public static DatasetBuildModel Validate(this DatasetBuildModel dataset)
    {
        var rosters = dataset.Rosters.Select(r => r.Roster).ToList();

        dataset.Definition.Validate(dataset.Rosters);
        var unitNames = rosters.SelectMany(r => r.Units).Select(u => u.Id).ToHashSet();

        foreach (var item in rosters)
        {
            var detachmentNames = rosters.Where(r => r.Name == item.Name)
                .SelectMany(r => r.Detachments)
                .Select(d => d.Name)
                .ToHashSet();

            item.ValidateRoster(dataset.Definition, unitNames, detachmentNames);
        }

        if (LoggingBuffer.ErrorCount > 0)
            throw new Exception($"Failed validation with {LoggingBuffer.ErrorCount} errors");
        LoggingBuffer.Info($"Validated {rosters.Count} rosters and {rosters.Sum(r => r.Units.Count)} units.");

        return dataset;
    }

    public static void ValidateRoster(
        this FactionRoster roster,
        DataSetDefinition definition,
        HashSet<string>? unitNames = null,
        HashSet<string>? detachmentNames = null)
    {
        if (!roster.Units.Any())
            LoggingBuffer.Error($"{roster.Name}: empty roster.");

        roster.Units.Select(u => u.Id).ValidateUnique($"{roster.Name} units");

        roster.ValidatePoints();
        roster.ValidateRegAbilities(definition);

        unitNames ??= roster.Units.Select(u => u.Id).ToHashSet();

        foreach (var unit in roster.Units)
        {
            unit.ValidateModelKeywords(roster.Name);
            unit.ValidateUnitWargear(roster.Name);
            unit.CheckForMatchingOptions(roster.Name);
            unit.ValidateWargearDefs(roster.Name);
            unit.ValidateStatlines(roster);

            if (unit.Id == "UNKNOWN")
                LoggingBuffer.Error($"{roster.Name}: unit is missing its ID.");

            unit.ValidateWargearOptions(roster, detachmentNames);
            unit.ValidateLeaderInfo(roster.Name, unitNames);
            unit.CheckWording(roster.Name);
        }
    }

    private static void ValidateLeaderInfo(this Unit unit, string rosterName, HashSet<string> unitNames)
    {
        if (unit.LeaderInfo is not { } leaderInfo)
            return;

        foreach (var name in leaderInfo.UnitNames ?? [])
            if (!unitNames.Contains(name))
                LoggingBuffer.Error($"{rosterName}.{unit.Id}: unrecognised unit to lead '{name}'.");

        foreach (var ability in leaderInfo.Abilities ?? [])
            if (!unit.UnitAbilities.Concat(unit.Infos).Any(a => a.Title == ability.AbilityId))
                LoggingBuffer.Error($"{rosterName}.{unit.Id}: unknown leader ability '{ability.AbilityId}'.");
    }

    private static void CheckWording(this Unit unit, string faction)
    {
        var abilities = unit.UnitAbilities
            .Concat(unit.Infos)
            .Concat(unit.SpecialAbility?.Sections ?? []);

        foreach (var ability in abilities)
            if (ability.Text.Split("\r\n").Length > 4)
                LoggingBuffer.Warn($"Potential wording layout issue in {faction}.{unit.Id}.{ability.Title}.");
    }
}
