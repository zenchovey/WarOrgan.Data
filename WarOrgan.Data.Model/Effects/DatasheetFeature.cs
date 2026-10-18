using System.Text.Json.Serialization;

namespace WarOrgan.Model.Effects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DatasheetFeature
{
    None,
    Keywords,
    UnitFlags,
    CoreAbilities,
    LedUnitNames,
    LeaderType,
    LeaderDescription,
    LeaderFeatures,
}
