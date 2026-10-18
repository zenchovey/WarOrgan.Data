namespace WarOrgan.Model.Settings;

public class PdfExportSettings
{
    public bool IncludeUnitDescription { get; set; } = true;
    public bool IncludeStatLines { get; set; } = true;
    public bool IncludeWeapons { get; set; } = true;
    public bool IncludeWeaponCount { get; set; } = true;
    public bool IncludeAbilities { get; set; } = true;
    public bool IncludeFactionAndKeywords { get; set; } = true;
    public bool IncludeArmyRules { get; set; } = true;
    public bool IncludeStratagems { get; set; } = true;
    public bool IncludeCoreStratagems { get; set; } = false;
    public bool IncludeNotes { get; set; } = false;
    public bool IncludeCategoryHeaders { get; set; } = true;
    public bool FilterWeapons { get; set; } = true;
    public bool FilterAbilites { get; set; } = true;
    public bool ModifiedStatlines { get; set; } = true;
    public bool DatasheetPagnation { get; set; } = false;
}
