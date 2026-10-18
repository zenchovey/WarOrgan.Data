using WarOrgan.Data.BuildDataset.Utils;
using WarOrgan.Model.Effects;
using WarOrgan.Model.Faction;
using WarOrgan.Model.Unit;
using WarOrgan.Model.Unit.Wargear;

namespace WarOrgan.Data.BuildDataset.Validation;

public static class WarGearValidation
{
    private static readonly List<DatasheetFeature> _singleTargetDatasheetFeatures =
    [
        DatasheetFeature.LeaderDescription,
        DatasheetFeature.LeaderType,
    ];

    public static void ValidateWargearDefs(this Unit unit, string factionName)
    {
        foreach (var wargearDef in unit.UnitComposition.WargearDefinitions)
            wargearDef.DatasheetAdjustments?.Validate($"{factionName}.{unit.Id}");
    }

    public static void Validate(this IEnumerable<DatasheetAdjustment> datasheetAdjustments, string name)
    {
        foreach (var datasheetAdjustment in datasheetAdjustments ?? [])
        {
            if (datasheetAdjustment.Target == DatasheetFeature.None)
                LoggingBuffer.Error($"Incomplete wargear def datasheet adjustment for {name}");

            if (!_singleTargetDatasheetFeatures.Contains(datasheetAdjustment.Target))
                continue;

            if (datasheetAdjustment.Values.Count != 1)
                LoggingBuffer.Error($"DatasheetAdjustment expects single \"Value\" for target {datasheetAdjustment.Target}");

            if (datasheetAdjustment.Type != StatAdjustmentType.Set)
                LoggingBuffer.Error($"DatasheetAdjustment expects type of \"Set\" for target {datasheetAdjustment.Target}");
        }
    }

    /// <summary>
    /// Replacement options with the same starting items must not overlap unless
    /// they are exact copies representing separate selections of the same weapons.
    /// </summary>
    public static void CheckForMatchingOptions(this Unit unit, string factionName)
    {
        foreach (var modelComp in unit.UnitComposition.ModelCompositions)
        {
            if (modelComp.Wargear.Count != 1)
            {
                if (modelComp.Wargear.Count > 1 && modelComp.Wargear.Any(w => w.Options.Any()))
                    LoggingBuffer.Error($"{factionName}.{unit.Id} with {modelComp.Wargear.Count} wargear sets, not handled in UI anymore.");
                continue;
            }

            var wargear = modelComp.Wargear[0];

            for (var i = 0; i < wargear.Options.Count; i++)
            {
                var option = wargear.Options[i];
                if (option.Replaces == null)
                    continue;

                var otherOptions = wargear.Options.Skip(i + 1).ToList();
                foreach (var otherOption in otherOptions)
                {
                    if (!option.Replaces.SequenceEqualUnordered(otherOption.Replaces))
                        continue;

                    if (option.AreEqual(otherOption))
                        continue;

                    var matches = option.Options.Where(c => otherOption.Options.Any(c.AreEqual));
                    if (matches.Any())
                        LoggingBuffer.Error($"Multiple wargear options with matching options {factionName}.{unit.Id}: [{string.Join(", ", option.Replaces)}] => [{string.Join(", ", matches)}]");
                }
            }
        }
    }

    public static void ValidateWargearOptions(this Unit unit, FactionRoster roster, HashSet<string>? detachmentNames = null)
    {
        detachmentNames ??= roster.Detachments.Select(d => d.Name).ToHashSet();

        var options = unit.UnitComposition.ModelCompositions
            .SelectMany(m => m.Wargear)
            .SelectMany(w => w.Options);

        foreach (var option in options)
        {
            if (!option.Options.Any() || option.Max <= 0 || option.PerXModels <= 0)
                LoggingBuffer.Error($"{roster.Name}.{unit.Id}: empty or invalid wargear option.");

            if (option.RequiredDetachment != null && !detachmentNames.Contains(option.RequiredDetachment))
                LoggingBuffer.Error($"{roster.Name}.{unit.Id}: unknown required detachment '{option.RequiredDetachment}'.");
        }
    }

    public static void ValidateUnitWargear(this Unit unit, string rosterName)
    {
        var context = $"{rosterName}.{unit.Id}";
        var weapons = unit.Weapons.SelectMany(s => s.Weapons).ToList();
        var weaponIds = weapons.Select(w => w.Id).OfType<string>().ToHashSet(StringComparer.Ordinal);

        var definitions = unit.UnitComposition.WargearDefinitions;
        var definitionIds = definitions.Select(d => d.Key).ToHashSet(StringComparer.Ordinal);
        var knownIds = weaponIds.Concat(definitionIds).ToHashSet(StringComparer.Ordinal);

        var abilityIds = unit.UnitAbilities
            .Concat(unit.Infos)
            .Concat(unit.SpecialAbility?.Sections ?? [])
            .Select(a => a.Title)
            .ToHashSet(StringComparer.Ordinal);

        var duplicateDefinitions = definitions.GroupBy(d => d.Key).Where(g => g.Count() > 1);
        foreach (var duplicate in duplicateDefinitions)
            LoggingBuffer.Error($"{context}: duplicate wargear definition key '{duplicate.Key}'");

        foreach (var id in weaponIds.Concat(definitionIds))
            id.ValidateWargearId(context, "ID/key");

        foreach (var definition in definitions)
            definition.ValidateReferences(weaponIds, abilityIds, context);

        foreach (var model in unit.UnitComposition.ModelCompositions)
            model.ValidateModelWargear(knownIds, context);

        var selectable = unit.GetSelectableWeaponIds();
        foreach (var weapon in weapons.Where(w => !w.Flags.HasFlag(WeaponFlags.NeverFilter)))
            if (weapon.Id == null || !selectable.Contains(weapon.Id))
                LoggingBuffer.Error($"{context}: unselectable weapon '{weapon.Id}'");

        if (!unit.Weapons.Any())
            LoggingBuffer.Warn($"No weapons found for '{context}'");
    }

    private static void ValidateReferences(this WargearDefinition definition, HashSet<string> weaponIds, HashSet<string> abilityIds, string context)
    {
        foreach (var id in definition.GrantsWeapons ?? [])
            if (!weaponIds.Contains(id))
                LoggingBuffer.Error($"{context}: '{definition.Key}' grants unknown weapon '{id}'");

        foreach (var id in definition.GrantsAbilities ?? [])
            if (!abilityIds.Contains(id))
                LoggingBuffer.Error($"{context}: '{definition.Key}' grants unknown ability '{id}'");
    }

    private static void ValidateModelWargear(this ModelComposition model, HashSet<string> knownIds, string context)
    {
        foreach (var gear in model.Wargear)
        {
            gear.InitialWargear.ValidateWargearIds(knownIds, context, $"{model.ModelName}.InitialWargear");

            foreach (var option in gear.Options)
                option.ValidateChoices(model.ModelName, knownIds, context);
        }
    }

    private static void ValidateChoices(this WargearOption option, string modelName, HashSet<string> knownIds, string context)
    {
        option.Replaces?.ValidateWargearIds(knownIds, context, $"{modelName}.Replaces");

        foreach (var choice in option.Options)
        {
            if (!choice.Items.Any())
                LoggingBuffer.Error($"{context}.{modelName}: empty wargear choice");

            choice.Items.ValidateWargearIds(knownIds, context, $"{modelName}.Options");
        }

        var uniqueChoices = option.Options
            .Select(c => string.Join("\u001f", c.Items.OrderBy(i => i, StringComparer.Ordinal)))
            .Distinct();

        if (option.Options.Count != uniqueChoices.Count())
            LoggingBuffer.Error($"{context}.{modelName}: duplicate wargear choices");
    }

    private static void ValidateWargearIds(this IEnumerable<string> ids, HashSet<string> knownIds, string context, string location)
    {
        foreach (var id in ids)
        {
            id.ValidateWargearId(context, location);

            if (!knownIds.Contains(id))
                LoggingBuffer.Error($"{context}.{location}: unknown wargear ID '{id}'");
        }
    }

    private static void ValidateWargearId(this string id, string context, string location)
    {
        if (string.IsNullOrWhiteSpace(id) || id != id.Trim())
            LoggingBuffer.Error($"{context}.{location}: expected a nonempty, trimmed ID, found '{id}'");
    }

    private static HashSet<string> GetSelectableWeaponIds(this Unit unit)
    {
        var selectable = unit.UnitComposition.ModelCompositions
            .SelectMany(m => m.Wargear)
            .SelectMany(w => w.InitialWargear.Concat(w.Options
                .SelectMany(o => o.Options)
                .SelectMany(c => c.Items)))
            .ToHashSet(StringComparer.Ordinal);

        var grantedWeapons = unit.UnitComposition.WargearDefinitions
            .Where(d => selectable.Contains(d.Key))
            .SelectMany(d => d.GrantsWeapons ?? [])
            .ToList();

        selectable.UnionWith(grantedWeapons);
        return selectable;
    }
}
