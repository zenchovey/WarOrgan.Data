namespace WarOrgan.Model.Abstraction;

using WarOrgan.Model.Unit;

public interface IStatLine
{
    string? StatName { get; set; }
    IReadOnlyCollection<ICharacteristic> Stats { get; }
    StatFeature Features { get; }
    BaseInfo? BaseInfo { get; }
}
