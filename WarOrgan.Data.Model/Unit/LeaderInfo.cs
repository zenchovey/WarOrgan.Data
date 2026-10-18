namespace WarOrgan.Model.Unit;

using WarOrgan.Model.Effects;

public class LeaderInfo
{
    public List<string> UnitNames { get; set; } = new List<string>();
    public List<string>? AttachableByKeyword { get; set; } // Some leaders might specify they can be attached to "Imperium Battleline Infantry" which may include many units across factions

    public string? LeaderType { get; set; } // Units are limited to attaching 1 of each LeaderType, null will mean it is the default main leader type
    public string? Description { get; set; }
    public LeaderFeatures Features { get; set; }

    public List<string>? RequiredLeaderKeywords { get; set; } // Optional - for units like Cyptothralls that require a Cryptek
    public CanAttachAlongsideLeadersInfo? CanAttachAlongsideLeadersInfo { get; set; }

    public List<LeaderAbilityRef>? Abilities { get; set; }

    public const string DefaultLeaderId = "Leader";
    public const string SupportLeaderId = "Support";
    public const string BodyguardId = "Bodyguard";
}

public class CanAttachAlongsideLeadersInfo
{
    // The list of leaders that the owning leader can be attached alongside
    public List<string>? LeaderKeywords { get; set; }

    // The list of attached units the owning leader can share with other leaders,
    // when null then allow any unit the leader may already be attached to
    public List<string>? UnitNameFilter { get; set; }
}

public class LeaderAbilityRef
{
    // Actually referencing the ability title for now, needs renaming
    public string AbilityId { get; set; } = string.Empty;

    // Usually applies all adjustments within the adjustment scope, this is for further filtering
    public List<string>? RequiredUnitKeywords{ get; set; }

    public List<StatAdjustment>? StatAdjustments { get; set; }
    public List<DatasheetAdjustment>? DatasheetAdjustments { get; set; }
    public List<WeaponStatAdjustment>? WeaponStatAdjustments { get; set; }
}

[Flags]
public enum LeaderFeatures : ulong
{
    None = 0,
    MultipleSelection = 1,  // Allow multiple of these leaders to join
    SoloLeader = 2,         // Prevent other leaders from being attached to the same unit
    All = MultipleSelection | SoloLeader,
}

