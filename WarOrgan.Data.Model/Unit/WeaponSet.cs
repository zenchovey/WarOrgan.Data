using WarOrgan.Model.Abstraction;

namespace WarOrgan.Model.Unit;

public class WeaponSet : IWeaponSet
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public List<Weapon> Weapons { get; set; } = new List<Weapon>();

    IEnumerable<IWeapon> IWeaponSet.Weapons => Weapons.OfType<IWeapon>();
}
