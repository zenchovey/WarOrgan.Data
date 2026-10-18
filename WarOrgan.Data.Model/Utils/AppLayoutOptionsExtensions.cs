using WarOrgan.Model.Settings;

namespace WarOrgan.Model.Utils;

public static class AppLayoutOptionsExtensions
{
    public static string ToDisplayName(this AppLayoutOptions appLayoutOptions)
    {
        switch (appLayoutOptions)
        {
            case AppLayoutOptions.Desktop:
                return "Tablet & Desktop";
            default: return appLayoutOptions.ToString();
        }
    }
}