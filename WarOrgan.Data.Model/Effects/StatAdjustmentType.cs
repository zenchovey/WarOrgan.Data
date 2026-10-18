using System.Text.Json.Serialization;

namespace WarOrgan.Model.Effects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatAdjustmentType
{
    Set,
    SetIfLower,
    Add,
    Subtract,
    Double
}
