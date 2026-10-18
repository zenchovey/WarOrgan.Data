namespace WarOrgan.Model.Effects;

public class StatAdjustment
{
    public string? TargetStatId { get; set; }
    public int? Min { get; set; }
    public int? Max { get; set; }
    public StatAdjustmentType Type { get; set; }
    public StatAdjustmentScope Scope { get; set; }
    public string Value { get; set; } = string.Empty;
    public string? Info { get; set; }
    public List<string>? NameFilter { get; set; }

    public override string ToString()
    {
        var info = Info != null ? $" - {Info}" : string.Empty;
        if (Type == StatAdjustmentType.Set)
            return $"StatAdjustment[Set {TargetStatId} to {Value} for {Scope}]{info}";
        return $"StatAdjustment[Add {Value} to {TargetStatId} for {Scope}]{info}";
    }
}
