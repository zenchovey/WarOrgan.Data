namespace WarOrgan.Model.DataSet;

public class RegAbility
{
    public string Regex { get; set; } = "Unknown";
    public string Description { get; set; } = "Unknown";

    public override string ToString() => Regex;
}
