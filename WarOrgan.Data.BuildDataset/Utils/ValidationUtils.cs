using WarOrgan.Model.DataSet;
using WarOrgan.Model.Unit.Wargear;

namespace WarOrgan.Data.BuildDataset.Utils;

public static class ValidationUtils
{
    public static IEnumerable<string> SplitKeywords(this string? text)
    {
        return text?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
    }

    public static bool SequenceEqualUnordered(this IEnumerable<string>? items, IEnumerable<string>? other)
    {
        return (items ?? [])
            .Order(StringComparer.Ordinal)
            .SequenceEqual((other ?? []).Order(StringComparer.Ordinal));
    }

    public static bool AreEqual(this WargearChoice choice, WargearChoice other)
    {
        return choice.Items.SequenceEqualUnordered(other.Items);
    }

    public static bool AreEqual(this WargearOption option, WargearOption other)
    {
        return option.Max == other.Max
            && option.PerXModels == other.PerXModels
            && option.Replaces.SequenceEqualUnordered(other.Replaces)
            && option.RequiredDetachment == other.RequiredDetachment
            && option.Options.Count == other.Options.Count
            && option.Options.All(c => other.Options.Any(c.AreEqual));
    }

    public static string GetFileName(this FactionRosterEntry entry)
    {
        return entry.NameOverride?.Invoke(entry.Roster) ?? $"{entry.Roster.Name}.json";
    }

    public static string GetRosterName(this DataSetDefinition definition, string faction)
    {
        var factionInfo = definition.Factions.FirstOrDefault(f => f.FactionKeyword.Equals(faction, StringComparison.OrdinalIgnoreCase))
            ?? definition.Factions.FirstOrDefault(f => f.Name.Equals(faction, StringComparison.OrdinalIgnoreCase));

        return factionInfo?.Name ?? faction;
    }

    public static void ValidateIds(this IEnumerable<string?> ids, string context)
    {
        foreach (var id in ids.Where(id => string.IsNullOrWhiteSpace(id) || id != id.Trim()))
            LoggingBuffer.Error($"{context}: Invalid Id: \"{id}\", Id/names must be nonempty and trimmed.");
    }

    public static void ValidateUnique(this IEnumerable<string?> ids, string context, StringComparer? comparer = null)
    {
        var values = ids.ToList();

        values.ValidateIds(context);

        var duplicates = values
            .GroupBy(id => id, comparer ?? StringComparer.Ordinal)
            .Where(g => g.Count() > 1);

        foreach (var duplicate in duplicates)
            LoggingBuffer.Error($"{context}: duplicate ID/name \"{duplicate.Key}\".");
    }
}
