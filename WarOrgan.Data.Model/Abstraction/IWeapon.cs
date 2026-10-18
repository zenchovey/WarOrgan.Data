namespace WarOrgan.Model.Abstraction;

using WarOrgan.Model.Unit;

public interface IWeapon
{
    string? Id { get; }
    string? DisplayName { get; }
    string? Keywords { get; }
    WeaponFlags Flags { get; }
    IReadOnlyCollection<ICharacteristic> Stats { get; }
}
