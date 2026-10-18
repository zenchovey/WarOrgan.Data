namespace WarOrgan.Model.Unit;

public class BaseInfo
{
    public string? Size { get; set; } // Only used when Round or Oval
    public BaseType Type { get; set; }
    public string? Description { get; set; } // Only used for base info that if different from the main statline
}

public enum BaseType
{
    Round,
    Oval,
    Hull,
    SmallFlying,
    LargeFlying,
    Unique
}
