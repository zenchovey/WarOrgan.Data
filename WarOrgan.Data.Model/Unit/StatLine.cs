using System.Text.Json.Serialization;
using WarOrgan.Model.Abstraction;

namespace WarOrgan.Model.Unit;

public class StatLine : IStatLine
{
    public string? StatName { get; set; }
    public List<Characteristic> Stats { get; set; } = new();
    public StatFeature Features { get; set; }
    public BaseInfo? BaseInfo { get; set; }

    IReadOnlyCollection<ICharacteristic> IStatLine.Stats => Stats;
}

public class Characteristic : ICharacteristic
{
    public string? Id { get; set; }
    public string? Value { get; set; }
    public string? Description { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatFeature
{
    None,
    AlwaysShow
}
