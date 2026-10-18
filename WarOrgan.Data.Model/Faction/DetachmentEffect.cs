namespace WarOrgan.Model.Faction;

using WarOrgan.Model.Effects;
using WarOrgan.Model.Unit;

public class DetachmentEffect
{
    public List<string> AffectedUnits { get; set; } = new List<string>();
    public List<string>? AffectedUnitsByKeyword { get; set; }
    public List<string>? AffectedUnitsExcludeKeyword { get; set; }

    public List<string>? RemoveAbilities { get; set; }
    public List<UnitInfoSection>? AddedAbilities { get; set; }
    public List<UnitInfoSection>? AddedInfoSections { get; set; }
    public List<LeaderAbilityRef>? AddedLeaderAbilityRefs { get; set; } // Should always be paired with an AddedAbility

    public List<StatAdjustment>? StatAdjustments { get; set; }
    public List<DatasheetAdjustment>? DatasheetAdjustments { get; set; }
    public List<WeaponStatAdjustment>? WeaponStatAdjustments { get; set; }
    public List<PointsDefinition>? PointsOverride { get; set; }
}

