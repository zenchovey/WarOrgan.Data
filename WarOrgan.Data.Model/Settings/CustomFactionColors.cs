namespace WarOrgan.Model.Settings;

public class CustomFactionColors
{
    public CustomFactionColorSetup Setup { get; set; }

    public string Text { get; set; } = string.Empty;
    public string Primary { get; set; } = string.Empty;
    public string Secondary { get; set; } = string.Empty;
    public string Highlight { get; set; } = string.Empty;
    public string SectionBorder { get; set; } = string.Empty;
}

public enum CustomFactionColorSetup
{
    BasedOffPrimary,
    FullSelection
}
