using WarOrgan.Model.Faction;

namespace WarOrgan.Model.DataSet;

public class DataSetDefinition
{
    public const string FileName = "DataSetDefinition.json";

    /// <summary>
    /// The features supported by this dataset, sections of 
    /// the UI will be hidden for unsupported features
    /// </summary>
    public DataSetFeatures Features { get; set; }

    /// <summary>
    /// The list of all available factions
    /// </summary>
    public List<FactionInfo> Factions { get; set; } = new();

    /// <summary>
    /// The core ability regexes that will be applied to Unit.CoreAbilities
    /// when the core abilties section is selected and information about 
    /// these core abilities is displayed.
    /// </summary>
    public List<RegAbility> CoreAbilities { get; set; } = new();

    /// <summary>
    /// The weapon abilitiy regexes that will be applied to the Weapon.Keywords 
    /// when a weapon is selected and information about its keywords is displayed
    /// </summary>
    public List<RegAbility> WeaponAbilities { get; set; } = new();

    /// <summary>
    /// The list of faction specific strategies
    /// </summary>
    public List<Stratagem> CoreStratagems { get; set; } = new();

    /// <summary>
    /// The set of valid list sizes
    /// </summary>
    public List<BattleSizeDefinition> BattleSizeDefinitions { get; set; } = new();

    /// <summary>
    /// The set of keywords that are used for unit categorization
    /// </summary>
    public List<CategoryKeyword> CategoryKeywords { get; set; } = new();
}
