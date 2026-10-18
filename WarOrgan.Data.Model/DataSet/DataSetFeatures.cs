using System.Text.Json.Serialization;

namespace WarOrgan.Model.DataSet;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DataSetFeatures : ulong
{
    None = 0,
    ArmyRules = 1,
    Detachments = 2,
    Stratagems = 4,
    Enhancements = 8,
    Dispositions = 16,
    All = ArmyRules | Detachments | Stratagems | Enhancements | Dispositions
}
