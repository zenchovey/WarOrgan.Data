namespace WarOrgan.Model.Faction;

public class UnitIncludeRule
{
    // Filters
    public List<string>? AffectedUnitsFilter { get; set; } // Target speicfic units, when null apply no filter and target all units
    public List<string>? AllKeywordFilter { get; set; }
    public List<string>? AnyKeywordFilter { get; set; }
    public List<string>? ExcludeKeywordFilter { get; set; }

    // Effects
    public List<string>? AddedKeywords { get; set; }
    public List<string>? RemovedKeywords { get; set; }
    public List<string>? ReplaceFactionKeywords { get; set; }

    // Filters can apply to specific unit sets, the exclusion of any rules at all
    // will result in all units being included as is. If you want to modify some of
    // the units and then include the rest, include a RemainingUnits entry at the end,
    // this will add unmodified instances of the units that were not included in the previous filters.
    public static UnitIncludeRule RemainingUnits { get; } = new();
}
