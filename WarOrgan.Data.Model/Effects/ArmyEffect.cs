using System.Text.Json.Serialization;

using WarOrgan.Model.Unit;

namespace WarOrgan.Model.Effects;

public class ArmyEffect
{
    public ArmyEffectCondition Condition { get; set; }

    public List<string>? AffectedUnits { get; set; }
    public List<string>? AffectedUnitsByKeyword { get; set; }
    public List<string>? AffectedUnitsExcludeKeyword { get; set; }
    public List<string>? AllowedDetachments { get; set; }

    public List<UnitInfoSection>? AddedAbilities { get; set; }
    public List<StatAdjustment>? StatAdjustments { get; set; }
    public List<DatasheetAdjustment>? DatasheetAdjustments { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ArmyEffectCondition
{
    None,
    UnitIsWarlord,
    UnitIncluded
}
