using WarOrgan.Model.Unit;

namespace WarOrgan.Model.Effects;

public class AddedWeapon
{
    public string WeaponSetId { get; set; } = string.Empty;
    public Weapon Weapon { get; set; } = new();
}
