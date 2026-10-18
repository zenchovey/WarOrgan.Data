using System.Text.Json.Serialization;

namespace WarOrgan.Model.Faction;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnhancementFeature
{
    None = 0,
    IsLeaderEnhancement = 1,
    IgnoreStandardEnhancementRules = 2,
    Upgrade = 4, // For "Upgrade" enhancements that can be equipped multiple times
    FreeEnhancementPointCost = 8,
}
