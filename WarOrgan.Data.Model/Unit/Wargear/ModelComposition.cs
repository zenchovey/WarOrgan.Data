using WarOrgan.Model.Abstraction;

namespace WarOrgan.Model.Unit.Wargear;

// In a unit there can be multiple models
// Each model type may have different inital war gear
public class ModelComposition : IModelComposition
{
    public int? UpgradeCost { get; set; }
    public ModelCountLimits? Limit { get; set; }
    public string ModelName { get; set; } = "Unknown";
    public string? StatlineRef { get; set; }

    // Additional keywords for these models; keywords shared by every model belong on the unit.
    public List<string> Keywords { get; set; } = new();

    // Each model type can have multiple different starting wargear sets
    public List<ModelWargear> Wargear { get; set; } = new List<ModelWargear>();

    IEnumerable<string> IModelComposition.Keywords => Keywords;
    IEnumerable<ModelWargear> IModelComposition.Wargear => Wargear;

    public override string ToString()
    {
        if (Limit == null)
            return $"{ModelName}";
        if (Limit.Min == Limit.Max)
            return $"{Limit.Min} {ModelName}";
        return $"{Limit.Min}-{Limit.Max} {ModelName}";
    }
}
