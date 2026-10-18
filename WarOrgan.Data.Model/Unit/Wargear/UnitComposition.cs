namespace WarOrgan.Model.Unit.Wargear;

/// <summary>
/// Represents all model compositions for a unit
/// Along with all options to add or reaplce wargear
/// </summary>
public class UnitComposition
{
    // Each unit may contain multiple different model types
    public List<ModelComposition> ModelCompositions { get; set; } = new List<ModelComposition>();

    // List of wargear definitions with information about their effects on the datasheet
    public List<WargearDefinition> WargearDefinitions { get; set;} = new List<WargearDefinition>();
}
