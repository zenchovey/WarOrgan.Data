namespace WarOrgan.Model.Unit.Wargear;

public class WargearOption
{
    public int? Max { get; set; } // Max times this can be applied to the parent UnitComposition
    public int? PerXModels { get; set; } // How many total models are required in the UnitComposition each time this is applied
    public List<string>? Replaces { get; set; } // Which wargear this replaces if any
    public List<WargearChoice> Options { get; set; } = []; // Each alternative grants all of its Items
    public string? RequiredDetachment { get; set; } // Don't show or apply this wargear option unless this detachment is selected

    public override string ToString()
    {
        return $"[{string.Join(", ", Replaces ?? Enumerable.Empty<string>())}] => [{string.Join(", ", Options)}]";
    }
}
