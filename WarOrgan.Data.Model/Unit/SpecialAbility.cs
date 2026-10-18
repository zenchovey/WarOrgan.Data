namespace WarOrgan.Model.Unit
{
    public class SpecialAbility
    {
        public string Name { get; set; } = "Unknown";
        public List<UnitInfoSection> Sections { get; set; } = new List<UnitInfoSection>();
    }
}
