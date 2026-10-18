namespace WarOrgan.Model.Unit.Wargear;

public class ModelWargear
{
    public int? Models { get; set; } // null indicates all
    public List<string> InitialWargear { get; set; } = new List<string>();

    // Each model wargear set can have multiple wargear options
    public List<WargearOption> Options { get; set; } = new List<WargearOption>();
}
