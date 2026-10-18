using WarOrgan.Model.Effects;

namespace WarOrgan.Model.Unit.Wargear;

public class WargearDefinition
{
    public string Key { get; set; } = string.Empty;
    public string? DisplayName { get; set; } // Optional alias label; otherwise the UI title-cases Key

    public int? Cost { get; set; }
    public string? TagName { get; set; } // This is just to make it show up on the unit data card

    public List<string>? GrantsAbilities { get; set; }
    public List<string>? GrantsWeapons { get; set; } // In addition to ones matching the Key
    public List<StatAdjustment>? StatAdjustments { get; set; }
    public List<WeaponStatAdjustment>? WeaponAdjustments { get; set; }
    public List<DatasheetAdjustment>? DatasheetAdjustments { get; set; }
}
