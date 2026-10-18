namespace WarOrgan.Model.DataSet;

public record class BattleSizeDefinition(
    string Name,
    int PointsTotal,
    int DetachmentPoints,
    int EnhancementLimit,
    int UnitLimit);