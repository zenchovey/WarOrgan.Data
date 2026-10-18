namespace WarOrgan.Model.Abstraction;

public interface IWeaponSet
{
    string? Id { get; }
    string? DisplayName { get; }
    IEnumerable<IWeapon> Weapons { get; }
}
