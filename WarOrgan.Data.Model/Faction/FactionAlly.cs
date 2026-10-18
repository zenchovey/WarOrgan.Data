namespace WarOrgan.Model.Faction;

public class FactionAlly
{
    public string FactionKeyword { get; set; } = string.Empty;
    public List<string>? AdditionalFactionKeywords { get; set; }
    public bool IsIncludedInPrimaryRoster { get; set; } // When true, units will be included as if they are part of the main faction roster
    public float? ArmyPercentageRestriction { get; set; }
    public List<string>? AllowedDetachments { get; set; } // When null, allow, else apply this filter 

    // When units are added to the main factions roster cache
    // they may be cloned and adjusted via effets in this list
    public List<UnitIncludeRule>? UnitIncludeRules { get; set; }
}
