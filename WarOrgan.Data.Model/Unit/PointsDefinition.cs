namespace WarOrgan.Model.Unit;

public class PointsDefinition
{
    public int? UnitCount { get; set; } // For when duplicate units may increase the unit cost
    public int ModelCount { get; set; }
    public int Cost { get; set; }
}
