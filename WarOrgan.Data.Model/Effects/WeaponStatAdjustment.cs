namespace WarOrgan.Model.Effects;

public class WeaponStatAdjustment
{
    public string Value { get; set; } = string.Empty;
    public StatAdjustmentType Type { get; set; }
    public StatAdjustmentScope Scope { get; set; }
    public string? TargetStatId { get; set; }
    public int? Min { get; set; }
    public int? Max { get; set; }
    public string? WeaponSetId { get; set; }
    public List<string>? NameFilter { get; set; }
    public List<string>? ContainsNameFilter { get; set; }
    public List<string>? IncludedKeywords { get; set; }
    public List<string>? ExcludedKeywords { get; set; }

    public string? CreateRenamedCopy { get; set; }

    public override string ToString()
    {
        var filterStr = string.Empty;
        if (NameFilter != null)
            filterStr += $" NameFilter:[{string.Join(",", NameFilter)}]";
        if (ContainsNameFilter != null)
            filterStr += $" ContainsNameFilter:[{string.Join(",", ContainsNameFilter)}]";
        if (IncludedKeywords != null)
            filterStr += $" IncludedKeywords:[{string.Join(",", IncludedKeywords)}]";
        if (ExcludedKeywords != null)
            filterStr += $" ExcludedKeywords:[{string.Join(",", ExcludedKeywords)}]";

        var scope = WeaponSetId != null ? $" for {WeaponSetId}" : string.Empty;

        switch (Type)
        {
            case StatAdjustmentType.Add:
                return $"WeaponStatAdjustment[Add {Value} to {TargetStatId}{scope} on {Scope}]{filterStr}";
            case StatAdjustmentType.Subtract:
                return $"WeaponStatAdjustment[Subtract {Value} from {TargetStatId}{scope} on {Scope}]{filterStr}";
            case StatAdjustmentType.Double:
                return $"WeaponStatAdjustment[Double {TargetStatId}{scope} on {Scope}]{filterStr}";
            default:
                return $"WeaponStatAdjustment[Set {TargetStatId} to {Value}{scope} on {Scope}]{filterStr}";
        }
            
    }
}
