namespace WarOrgan.Model.Settings;

public class CustomThemeColors
{
    public string Background { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string HeaderText { get; set; } = string.Empty;
    public string Accent { get; set; } = string.Empty;
    public string ButtonPressed { get; set; } = string.Empty;
    public string ButtonHighlight { get; set; } = string.Empty;
    public string Foreground { get; set; } = string.Empty;

    public void Reset()
    {
        Background = Section = HeaderText = Accent = ButtonPressed = ButtonHighlight = Foreground = string.Empty;
    }
}
