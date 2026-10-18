using System.Text.Json.Serialization;

namespace WarOrgan.Model.Effects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatAdjustmentScope
{
    Model,              // Only the bearer of the wargear or enhancement
    ModelWhileLeading,  // Only the bearer of the wargear or enhancement while in an attached unit
    Unit,               // Always applies to the bearer and the rest of the attached unit if there is one
    AttachedUnit,       // Only applies to the bearer and the rest of the attached unit while attached
    BodyguardUnitOnly,  // Only applies to the Bodyguard Unit, excluding attached leaders
    LeaderUnitsOnly,    // Only applies to non-Bodyguard units while attached
}
