using System.Text.RegularExpressions;
using WarOrgan.Data.BuildDataset.Utils;
using WarOrgan.Model.DataSet;
using WarOrgan.Model.Faction;

namespace WarOrgan.Data.BuildDataset.Validation;

public static class FactionInfoValidation
{
    public static void Validate(this DataSetDefinition definition, List<FactionRoster> rosters)
    {
        definition.Validate(rosters.Select(r => new FactionRosterEntry(r)).ToList());
    }

    public static void Validate(this DataSetDefinition definition, List<FactionRosterEntry> entries)
    {
        var rosters = entries.Select(e => e.Roster).ToList();

        if (!definition.Factions.Any())
            LoggingBuffer.Error("Missing factions.");

        definition.Factions.Select(f => f.Name).ValidateUnique("Faction names", StringComparer.OrdinalIgnoreCase);
        definition.Factions.Select(f => f.FactionKeyword).ValidateUnique("Faction keywords", StringComparer.OrdinalIgnoreCase);
        entries.Select(e => e.GetFileName()).ValidateUnique("Roster filenames", StringComparer.OrdinalIgnoreCase);

        foreach (var filename in entries.Select(e => e.GetFileName()))
            if (!filename.IsValidRosterFileName())
                LoggingBuffer.Error($"Invalid roster filename: '{filename}'.");

        if ((definition.Features & ~DataSetFeatures.All) != 0)
            LoggingBuffer.Error("Unrecognised dataset features.");

        definition.BattleSizeDefinitions.ValidateBattleSizes();
        definition.ValidateCategoryKeywords(rosters.SelectMany(r => r.Units));

        foreach (var faction in definition.Factions)
            faction.ValidateFaction(entries);

        var unknownRosters = rosters.Where(r => !definition.Factions.Any(f => f.Name == r.Name));
        foreach (var roster in unknownRosters)
            LoggingBuffer.Error($"Roster '{roster.Name}' is not registered in the dataset definition.");

        definition.ValidateAllies(entries);
        definition.CoreAbilities.ValidateDefinitions("Core abilities");
        definition.WeaponAbilities.ValidateDefinitions("Weapon abilities");
    }

    private static void ValidateBattleSizes(this List<BattleSizeDefinition> definitions)
    {
        if (!definitions.Any())
            LoggingBuffer.Error("Missing battle size definitions.");

        if (definitions.Any(b => b.PointsTotal <= 0 || b.DetachmentPoints < 0 || b.EnhancementLimit < 0 || b.UnitLimit < 0))
            LoggingBuffer.Error("Invalid battle size definition.");

        if (definitions.GroupBy(b => b.PointsTotal).Any(g => g.Count() > 1))
            LoggingBuffer.Error("Duplicate battle size definitions.");
    }

    private static void ValidateFaction(this FactionInfo faction, List<FactionRosterEntry> entries)
    {
        if (!faction.Name.IsValidFactionFileName())
            LoggingBuffer.Error($"Invalid faction filename: '{faction.Name}'.");

        if (!faction.MainColor.IsValidFactionColor() || !faction.DarkColor.IsValidFactionColor())
            LoggingBuffer.Error($"{faction.Name}: invalid faction colours (expected #RRGGBB).");

        var mainRosters = entries.Where(e => e.GetFileName() == $"{faction.Name}.json").ToList();
        if (mainRosters.Count != 1)
            LoggingBuffer.Error($"{faction.Name}: expected exactly one main roster file, found {mainRosters.Count}.");

        var matches = entries.Select(e => e.Roster).Where(r => r.Name == faction.Name).ToList();
        var keywords = faction.AllFactionKeywords
            .Concat(matches.SelectMany(r => r.Detachments).SelectMany(d => d.AdditionalFactionKeywords ?? []))
            .ToHashSet();

        foreach (var roster in matches)
            roster.ValidateFactionKeywords(keywords);
    }

    private static void ValidateFactionKeywords(this FactionRoster roster, HashSet<string> keywords)
    {
        foreach (var unit in roster.Units)
            if (!unit.FactionKeywords.Any(keywords.Contains))
                LoggingBuffer.Error($"{roster.Name}.{unit.Id}: unit is unobtainable with this faction's keywords.");
    }

    private static void ValidateAllies(this DataSetDefinition definition, List<FactionRosterEntry> entries)
    {
        var detachmentNames = entries.Select(e => e.Roster)
            .SelectMany(r => r.Detachments)
            .Select(d => d.Name)
            .ToHashSet();

        var rosterNames = entries
            .Select(e => Path.GetFileNameWithoutExtension(e.GetFileName()))
            .ToHashSet();

        foreach (var ally in definition.Factions.SelectMany(f => f.Allies ?? []))
        {
            var rosterName = definition.GetRosterName(ally.FactionKeyword);
            if (!rosterNames.Contains(rosterName))
                LoggingBuffer.Error($"Unknown allied faction: '{ally.FactionKeyword}'.");

            foreach (var detachment in ally.AllowedDetachments ?? [])
                if (!detachmentNames.Contains(detachment))
                    LoggingBuffer.Error($"Unknown detachment in ally AllowedDetachments: '{detachment}'.");
        }
    }

    private static bool IsValidFactionFileName(this string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || name.EndsWith('.') || name is "." or "..")
            return false;

        var reservedNames = new[] { "DataSetDefinition", "DataSetInfo" };
        return !reservedNames.Contains(name, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidRosterFileName(this string filename)
    {
        return filename == Path.GetFileName(filename)
            && Path.GetExtension(filename).Equals(".json", StringComparison.OrdinalIgnoreCase)
            && Path.GetFileNameWithoutExtension(filename).IsValidFactionFileName();
    }

    private static bool IsValidFactionColor(this string color)
    {
        return Regex.IsMatch(color, "^#[0-9a-fA-F]{6}$");
    }
}
