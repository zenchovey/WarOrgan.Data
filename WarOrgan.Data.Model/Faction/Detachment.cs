using WarOrgan.Model.Abstraction;

namespace WarOrgan.Model.Faction;

public class Detachment : ISearchableModel
{
    public string Name { get; set; } = "Unknown";
    public int Cost { get; set; } = 1;

    public DetachmentRule? Rule { get; set; }
    public List<string>? Tags { get; set; }
    public List<string>? ForceDispositions { get; set; }
    public List<Stratagem> Stratagems { get; set; } = new List<Stratagem>();
    public List<Enhancement> Enhancements { get; set; } = new List<Enhancement>();
    public List<DetachmentEffect> Effects { get; set; } = new List<DetachmentEffect>();
    public List<string>? AdditionalFactionKeywords { get; set; }
    public List<string>? RestrictedUnits { get; set; }
}