using WarOrgan.Model.Abstraction;

namespace WarOrgan.Model.Unit;

public class Weapon : IWeapon
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Keywords { get; set; }
    public WeaponFlags Flags { get; set; }
    public List<Characteristic> Stats { get; set; } = new();

    IReadOnlyCollection<ICharacteristic> IWeapon.Stats => Stats;
}
