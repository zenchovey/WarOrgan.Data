using System.Text.Json.Serialization;

namespace WarOrgan.Model.Unit;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WeaponFlags
{
    None = 0,

    // Some weapons can be added as part of abilities
    // so shouldn't be filtered out by equipped wargear
    NeverFilter = 1,
}
