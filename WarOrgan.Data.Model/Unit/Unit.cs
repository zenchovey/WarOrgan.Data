
using System.Text.Json.Serialization;
using WarOrgan.Model.Abstraction;
using WarOrgan.Model.Effects;
using WarOrgan.Model.Unit.Wargear;

namespace WarOrgan.Model.Unit;

public class Unit : IUnit
{
    // The ID of the unit
    public string Id { get; set; } = "UNKNOWN";
    public string? DisplayName { get; set; }

    public string? Faction { get; set; }
    public UnitFlags Flags { get; set; }

    public UnitComposition UnitComposition { get; set; } = new UnitComposition();
    public List<UnitInfoSection> UnitAbilities { get; set; } = new List<UnitInfoSection>();
    public AttachedUnitInfo? AttachedUnitInfo { get; set; }
    public LeaderInfo? LeaderInfo { get; set; }
    public List<UnitInfoSection> Infos { get; set; } = new List<UnitInfoSection>();
    public SpecialAbility? SpecialAbility { get; set; }
    public List<ArmyEffect>? ArmyEffects { get; set; }

    public List<string> Keywords { get; set; } = new List<string>();
    public List<string> CoreAbilities { get; set; } = new List<string>();
    public List<string> FactionKeywords { get; set; } = new List<string>();
    public List<StatLine> StatLines { get; set; } = new List<StatLine>();
    public List<WeaponSet> Weapons { get; set; } = new List<WeaponSet>();
    public List<UnitUpgrade> Upgrades { get; set; } = new List<UnitUpgrade>();
    public List<PointsDefinition> Points { get; set; } = new List<PointsDefinition>();

    IEnumerable<IModelComposition> IUnit.ModelCompositions => UnitComposition.ModelCompositions;
    IEnumerable<string> IUnit.Keywords => Keywords;
    IEnumerable<string> IUnit.CoreAbilities => CoreAbilities;
    IEnumerable<string> IUnit.FactionKeywords => FactionKeywords;
    IEnumerable<IWeaponSet> IUnit.Weapons => Weapons.OfType<IWeaponSet>();
    IEnumerable<IStatLine> IUnit.StatLines => StatLines.OfType<IStatLine>();
    IEnumerable<UnitInfoSection> IUnit.Infos => Infos;
    IEnumerable<UnitInfoSection> IUnit.UnitAbilities => UnitAbilities;
    IEnumerable<UnitUpgrade> IUnit.Upgrades => Upgrades;
    IEnumerable<PointsDefinition> IUnit.Points => Points;
    IEnumerable<ArmyEffect>? IUnit.ArmyEffects => ArmyEffects;

    // Set post deserialization if the unit came from a "{Name} Legends".json roster
    [JsonIgnore]
    public bool IsLegend { get; set; }

    // For the ISearchableModel interface
    [JsonIgnore]
    public string Name => DisplayName ?? Id;
}

