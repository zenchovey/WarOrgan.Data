namespace WarOrgan.Model.Abstraction;

using WarOrgan.Model.Unit.Wargear;

public interface IModelComposition
{
    string ModelName { get; }
    string? StatlineRef { get; }
    int? UpgradeCost { get; }
    ModelCountLimits? Limit { get; }
    IEnumerable<string> Keywords { get; }
    IEnumerable<ModelWargear> Wargear { get; }
}
