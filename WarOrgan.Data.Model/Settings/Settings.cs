namespace WarOrgan.Model.Settings;

public class Settings
{
    public AppTheme Theme { get; set; } = AppTheme.System;
    public AppTextSize AppTextSize { get; set; } = AppTextSize.Medium;
    public FactionColors FactionColors { get; set; } = FactionColors.Auto;
    public AppLayoutOptions AppLayout { get; set; } = AppLayoutOptions.Auto;
    public HomeScreenSort HomeScreenSort { get; set; } = HomeScreenSort.Name;

    public bool FilterWeaponsEditMode { get; set; } = false;
    public bool FilterWeaponsViewerMode { get; set; } = true;
    public bool FilterEnhancements { get; set; } = true;
    public bool FilterAbilities { get; set; } = true;
    public bool ShowUnitImages { get; set; } = true;
    public bool EnableModifiedStatlines { get; set; } = true;
    public bool ShowArmyRulesInViewer { get; set; } = true;
    public bool ShowDetachmentsInViewer { get; set; } = true;
    public bool ShowStratagemsInViewer { get; set; } = true;
    public bool ShowCoreStratagemsInViewer { get; set; } = false;
    public bool AutoGenerateAttachedUnitNames { get; set; } = true;

    public CustomThemeColors CustomLightThemeColors { get; set; } = new();
    public CustomThemeColors CustomDarkThemeColors { get; set; } = new();
    public CustomFactionColors CustomFactionColors { get; set; } = new();
    public PdfExportSettings PdfExportSettings { get; set; } = new();
    public TtsExportSettings TtsExportSettings { get; set; } = new();

    public string? DataSetId { get; set; }
    public string? DataSetVersion { get; set; }
    public string? TtsForceOrgDataPathOverride { get; set; }
}
