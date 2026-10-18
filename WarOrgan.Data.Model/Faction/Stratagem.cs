using WarOrgan.Model.Abstraction;

namespace WarOrgan.Model.Faction;

public class Stratagem : ISearchableModel
{
    public string Name { get; set; } = "Unknown";
    public string Category { get; set; } = "Unknown";
    public int CPCost { get; set; }

    public string Phase { get; set; } = "Unknown";
    public string When { get; set; } = "Unknown";
    public string? Target { get; set; }
    public string Effect { get; set; } = "Unknown";
    public string? Restrictions { get; set; }
    public List<StratagemOption>? Options { get; set; }
}
