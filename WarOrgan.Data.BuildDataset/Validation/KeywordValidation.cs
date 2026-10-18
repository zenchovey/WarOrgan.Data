using WarOrgan.Data.BuildDataset.Utils;
using WarOrgan.Model.DataSet;
using WarOrgan.Model.Unit;

namespace WarOrgan.Data.BuildDataset.Validation;

public static class KeywordValidation
{
    public static void ValidateCategoryKeywords(this DataSetDefinition definition, IEnumerable<Unit> units)
    {
        var keywords = units
            .SelectMany(unit => unit.Keywords.Concat(
                unit.UnitComposition.ModelCompositions.SelectMany(model => model.Keywords)))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var category in definition.CategoryKeywords)
            if (!keywords.Contains(category.Keyword))
                LoggingBuffer.Error($"Category keyword '{category.Keyword}' does not match any unit.");
    }

    public static void ValidateModelKeywords(this Unit unit, string rosterName)
    {
        foreach (var model in unit.UnitComposition.ModelCompositions)
            foreach (var keyword in model.Keywords.Intersect(unit.Keywords, StringComparer.OrdinalIgnoreCase))
                LoggingBuffer.Error($"{rosterName}.{unit.Id}.{model.ModelName}: model keyword '{keyword}' is already on the unit.");
    }
}
