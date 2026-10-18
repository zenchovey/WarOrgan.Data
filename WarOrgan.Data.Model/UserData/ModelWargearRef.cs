namespace WarOrgan.Model.UserData;

public class ModelWargearRef
{
    public int ModelCount { get; set; } 
    public string ModelName { get; set; } = "Unknown"; // Note this is not an Id, a unit can have multiple instances of the same model where each model has different wargear
    public List<string> Wargear { get; set; } = new List<string>();

    public override string ToString() => $"{ModelName}({ModelCount}): [{string.Join(", ", Wargear)}]";
}
