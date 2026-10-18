using WarOrgan.Model.Faction;

namespace WarOrgan.Model.Faction;

using WarOrgan.Model.Unit;

public class FactionRoster
{
    public string Name { get; set; } = "Unknown";
    public List<Unit> Units { get; set; } = new List<Unit>();
    public List<Detachment> Detachments { get; set; } = new List<Detachment>();
    public List<UnitInfoSection> ArmyRules { get; set; } = new List<UnitInfoSection>();
}
