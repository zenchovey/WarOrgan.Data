namespace WarOrgan.Model.Abstraction;

using WarOrgan.Model.Effects;
using WarOrgan.Model.Unit;
using WarOrgan.Model.Unit.Wargear;

public interface IUnit : ISearchableModel
{
    string Id { get; }
    string? DisplayName { get; }
    string? Faction { get; }
    UnitFlags Flags { get; }
    bool IsLegend { get; }

    UnitComposition UnitComposition { get; }
    IEnumerable<IModelComposition> ModelCompositions { get; }
    AttachedUnitInfo? AttachedUnitInfo { get; }
    LeaderInfo? LeaderInfo { get; }

    SpecialAbility? SpecialAbility { get; }
    IEnumerable<ArmyEffect>? ArmyEffects { get; }
    IEnumerable<UnitInfoSection> Infos { get; }
    IEnumerable<UnitInfoSection> UnitAbilities { get; }

    IEnumerable<string> Keywords { get; }
    IEnumerable<string> CoreAbilities { get; }
    IEnumerable<string> FactionKeywords { get; }
    IEnumerable<IStatLine> StatLines { get; }
    IEnumerable<IWeaponSet> Weapons { get; }
    IEnumerable<UnitUpgrade> Upgrades { get; }
    IEnumerable<PointsDefinition> Points { get; }
}
