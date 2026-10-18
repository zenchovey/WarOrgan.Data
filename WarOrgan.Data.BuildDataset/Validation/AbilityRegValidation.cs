using System.Text.RegularExpressions;
using WarOrgan.Data.BuildDataset.Utils;
using WarOrgan.Model.DataSet;
using WarOrgan.Model.Effects;
using WarOrgan.Model.Faction;

namespace WarOrgan.Data.BuildDataset.Validation;

public static class AbilityRegValidation
{
    private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(1);

    public static List<Regex> ValidateDefinitions(this List<RegAbility> abilities, string context)
    {
        var regexes = new List<Regex>();

        abilities.Select(a => a.Regex).ValidateUnique(context);

        foreach (var ability in abilities)
        {
            if (string.IsNullOrWhiteSpace(ability.Description) || ability.Description == "Unknown")
                LoggingBuffer.Error($"{context}: missing description for '{ability.Regex}'.");

            if (ability.TryParseRegex(out var error) is { } regex)
                regexes.Add(regex);
            else LoggingBuffer.Error($"{context}: invalid regex '{ability.Regex}': {error}");
        }

        return regexes;
    }

    public static void ValidateRegAbilities(this FactionRoster roster, DataSetDefinition definition)
    {
        var coreAbilities = definition.CoreAbilities.ParseDefinitions();
        var weaponAbilities = definition.WeaponAbilities.ParseDefinitions();

        roster.GetCoreAbilities().ValidateMatches(coreAbilities, roster.Name, "core");
        roster.GetWeaponAbilities().ValidateMatches(weaponAbilities, roster.Name, "weapon");
    }

    private static List<Regex> ParseDefinitions(this IEnumerable<RegAbility> abilities)
    {
        // Invalid syntax has already been reported by FactionInfoValidation.
        return abilities
            .Select(a => a.TryParseRegex(out _))
            .OfType<Regex>()
            .ToList();
    }

    private static Regex? TryParseRegex(this RegAbility ability, out string? error)
    {
        try
        {
            error = null;
            return new Regex(ability.Regex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, _timeout);
        }
        catch (ArgumentException ex)
        {
            error = ex.Message;
            return null;
        }
    }

    private static IEnumerable<string> GetCoreAbilities(this FactionRoster roster)
    {
        var datasheetAdjustments = roster.Units
            .SelectMany(u => u.UnitComposition.WargearDefinitions)
            .SelectMany(w => w.DatasheetAdjustments ?? [])
            .Concat(roster.Detachments
                .SelectMany(d => d.Enhancements)
                .SelectMany(e => e.DatasheetAdjustments ?? []));

        var addedCoreAbilities = datasheetAdjustments
            .Where(d => d.Target == DatasheetFeature.CoreAbilities)
            .SelectMany(d => d.Values);

        return roster.Units
            .SelectMany(u => u.CoreAbilities)
            .Concat(addedCoreAbilities);
    }

    private static IEnumerable<string> GetWeaponAbilities(this FactionRoster roster)
    {
        var weaponAdjustments = roster.Units
            .SelectMany(u => u.UnitComposition.WargearDefinitions)
            .SelectMany(w => w.WeaponAdjustments ?? [])
            .Concat(roster.Detachments
                .SelectMany(d => d.Enhancements)
                .SelectMany(e => e.WeaponStatAdjustments ?? []));

        var addedWeaponAbilities = weaponAdjustments
            .Where(w => w.TargetStatId == "Keywords")
            .SelectMany(w => w.Value.SplitKeywords());

        return roster.Units
            .SelectMany(u => u.Weapons)
            .SelectMany(w => w.Weapons)
            .SelectMany(w => w.Keywords.SplitKeywords())
            .Concat(addedWeaponAbilities);
    }

    private static void ValidateMatches(this IEnumerable<string> abilities, List<Regex> regexes, string rosterName, string kind)
    {
        foreach (var ability in abilities.Distinct())
        {
            try
            {
                if (!regexes.Any(r => r.IsMatch(ability.Trim('[', ']'))))
                    LoggingBuffer.Error($"{rosterName}: missing {kind} ability definition for '{ability}'.");
            }
            catch (RegexMatchTimeoutException)
            {
                LoggingBuffer.Error($"{rosterName}: regex timed out matching '{ability}'.");
            }
        }
    }
}
