namespace WarOrgan.Model.Effects;

public class DatasheetAdjustment
{
    public DatasheetFeature Target { get; set; }
    public StatAdjustmentType Type { get; set; }
    public StatAdjustmentScope Scope { get; set; }
    public List<string> Values { get; set; } = new();
    public List<ulong>? EnumValues { get; set; }

    public override string ToString()
    {
        return $"DatasheetAdjustment[{Type} {Target} - {string.Join(", ", Values)}";
    }
}
