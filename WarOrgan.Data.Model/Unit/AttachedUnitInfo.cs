using System.Text;

namespace WarOrgan.Model.Unit;

/// <summary>
/// This is for conditions and extra information that lives on the bodyguard unit
/// </summary>
public class AttachedUnitInfo
{
    // This is only for the default leader type, if we need to extend
    // this to allow for multiple definitions of max leaders then this
    // will need to be a list and we need to add a LeaderType to MaxLeaderInfo
    public List<MaxLeaderInfo>? MaxLeaders { get; set; }

    // For when a unit has some rule about pretending to be another unit for the purpose of attaching a leader
    public List<LeaderAttachmentRule>? LeaderAttachmentRules { get; set; }

    public override string ToString()
    {
        return MaxLeaders?.Any() == true ? string.Join("\n", MaxLeaders.Select(s => s.ToString())) : "Empty";
    }
}

/// <summary>
/// If the potential leader has this keyword and the units it can lead 
/// contain one of the LedUnitNames then the owning unit may attach that leader
/// </summary>
public class LeaderAttachmentRule
{
    public string LeaderKeyword { get; set; } = string.Empty;
    public List<string>? LedUnitNames { get; set; }
}

/// <summary>
/// Information for bodyguard units leader maximum override
/// </summary>
public class MaxLeaderInfo
{
    public int MaxLeaderOverride { get; set; }

    public string? LeaderType { get; set; }

    // Requires the bodyguard unit has at least this many models to unlock the MaxLeaderOverride
    public int? RequiredModels { get; set; }

    // Must have one of these leaders to allow the MaxLeaderOverride
    public List<string>? RequiresOneOfLeaders { get; set; }

    // May still only have one of these leaders regardless of the MaxLeaderOverride
    public List<string>? PreventDuplicates { get; set; }

    public override string ToString()
    {
        var strBuffer = new StringBuilder();
        strBuffer.AppendLine($"MaxLeaderOverride: {MaxLeaderOverride}");
        if (RequiredModels != null)
            strBuffer.AppendLine($"RequiredModels: {RequiredModels}");
        if (RequiresOneOfLeaders != null)
            strBuffer.AppendLine($"RequiresOneOfLeaders: [{string.Join(", ", RequiresOneOfLeaders)}]");
        if (PreventDuplicates != null)
            strBuffer.AppendLine($"LimitedToOneLeaders: [{string.Join(", ", PreventDuplicates)}]");

        return strBuffer.ToString();
    }
}
