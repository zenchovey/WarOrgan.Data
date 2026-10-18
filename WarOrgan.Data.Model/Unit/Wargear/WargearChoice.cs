namespace WarOrgan.Model.Unit.Wargear;

/// <summary>A single alternative. Repeated IDs represent multiple copies of an item.</summary>
public class WargearChoice
{
    public List<string> Items { get; set; } = [];

    public override string ToString() => string.Join(", ", Items);
}
