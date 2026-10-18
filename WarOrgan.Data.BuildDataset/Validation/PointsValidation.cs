using WarOrgan.Data.BuildDataset.Utils;
using WarOrgan.Model.Faction;
using WarOrgan.Model.Unit;
using WarOrgan.Model.Unit.Wargear;

namespace WarOrgan.Data.BuildDataset.Validation;

public static class PointsValidation
{
    public static void ValidatePoints(this FactionRoster roster)
    {
        foreach (var unit in roster.Units)
        {
            var context = $"{roster.Name}.{unit.Id}";

            unit.Points.ValidatePointDefinitions(context);
            unit.UnitComposition.ModelCompositions.ValidateModelCompositions(context);
            unit.ValidatePointModelCounts(context);

            if (unit.Upgrades.Any(u => u.Cost < 0) || unit.UnitComposition.WargearDefinitions.Any(d => d.Cost < 0))
                LoggingBuffer.Error($"{context}: negative equipment or upgrade cost.");
        }
    }

    private static void ValidatePointDefinitions(this List<PointsDefinition> points, string context)
    {
        if (!points.Any())
            LoggingBuffer.Error($"{context}: missing points definitions.");

        if (points.Any(p => p.Cost < 0 || p.ModelCount <= 0 || p.UnitCount <= 0))
            LoggingBuffer.Error($"{context}: invalid points cost or count.");

        if (points.Any(p => p.UnitCount != null) && points.Any(p => p.UnitCount == null))
            LoggingBuffer.Error($"{context}: UnitCount must be present in all or none of the points definitions.");

        foreach (var duplicate in points.GroupBy(p => (p.ModelCount, p.UnitCount)).Where(g => g.Count() > 1))
        {
            var costs = duplicate.Select(p => p.Cost).Distinct().ToList();
            if (costs.Count > 1)
                LoggingBuffer.Error($"{context}: conflicting points costs for ModelCount {duplicate.Key.ModelCount}, UnitCount {duplicate.Key.UnitCount?.ToString() ?? "any"}: {string.Join(", ", costs)}.");
            else LoggingBuffer.Warn($"{context}: repeated points definition for ModelCount {duplicate.Key.ModelCount}, UnitCount {duplicate.Key.UnitCount?.ToString() ?? "any"}.");
        }
    }

    private static void ValidateModelCompositions(this List<ModelComposition> models, string context)
    {
        models.Select(m => m.ModelName).ValidateUnique($"{context} model compositions");

        if (!models.Any())
            LoggingBuffer.Error($"{context}: missing model compositions.");

        foreach (var model in models)
        {
            if (model.Limit is { } limit && (limit.Min < 0 || limit.Max < 0 || limit.Min > limit.Max))
                LoggingBuffer.Error($"{context}.{model.ModelName}: invalid model count limits.");

            if (model.Wargear.Any(w => w.Models < 0))
                LoggingBuffer.Error($"{context}.{model.ModelName}: negative starting model count.");

            if (model.UpgradeCost < 0)
                LoggingBuffer.Error($"{context}.{model.ModelName}: negative model upgrade cost.");
        }
    }

    private static void ValidatePointModelCounts(this Unit unit, string context)
    {
        var baseModels = unit.UnitComposition.ModelCompositions
            .Where(m => m.UpgradeCost == null)
            .ToList();

        var min = baseModels.Sum(m => m.Limit?.Min ?? m.GetStartingModelCount());
        var max = baseModels.Sum(m => m.Limit?.Max ?? m.GetStartingModelCount());

        foreach (var pointsDef in unit.Points)
            if (pointsDef.ModelCount < min || pointsDef.ModelCount > max)
                LoggingBuffer.Warn($"{context}: points definition model count [{pointsDef.ModelCount} for {pointsDef.Cost}pts] lies outside composition limits ({min}-{max}).");
    }

    private static int GetStartingModelCount(this ModelComposition model)
    {
        return model.Wargear.Any() ? model.Wargear.Sum(w => w.Models ?? 1) : 1;
    }
}
