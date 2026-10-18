using WarOrgan.Model.Abstraction;
using WarOrgan.Model.Effects;

namespace WarOrgan.Model.Faction;

public class Enhancement : ISearchableModel
{
    public string Name { get; set; } = "Unknown";
    public string? Description { get; set; }
    public int Cost { get; set; }
    public EnhancementFeature Features { get; set; }

    public List<AddedWeapon>? AddedWeapons { get; set; }
    public List<StatAdjustment>? StatAdjustments { get; set; }
    public List<WeaponStatAdjustment>? WeaponStatAdjustments { get; set; }
    public List<DatasheetAdjustment>? DatasheetAdjustments { get; set; }

    // Requirements for Unit adjustments
    public List<string>? RequiredAttachedUnitKeywords { get; set; }

    // Requirements for equip
    public List<string> RequiredUnitNames { get; set; } = new List<string>();
    public List<string> RequiredKeywords { get; set; } = new List<string>();
    public List<string> RequiredOneOfKeywords { get; set; } = new List<string>();
    public List<string> ExcludedKeywords { get; set; } = new List<string>();
    public List<string> RequiredAbilities { get; set; } = new List<string>();
}
