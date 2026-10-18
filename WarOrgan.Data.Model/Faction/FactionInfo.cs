using System.Text.Json.Serialization;

namespace WarOrgan.Model.Faction;

public class FactionInfo
{
    public string Name { get; set; } = string.Empty;
    public string MainColor { get; set; } = string.Empty;
    public string DarkColor { get; set; } = string.Empty;
    public string FactionKeyword { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public List<string>? AdditionalFactionKeywords { get; set; }
    public List<FactionAlly>? Allies { get; set; }

    [JsonIgnore]
    public string? CoreFactionKeyword => AlliedFactions.SingleOrDefault(a => a.IsIncludedInPrimaryRoster)?.FactionKeyword;

    [JsonIgnore]
    public IEnumerable<string> ExtraFactionKeywords => AdditionalFactionKeywords ?? Enumerable.Empty<string>();

    [JsonIgnore]
    public IEnumerable<FactionAlly> AlliedFactions => Allies ?? Enumerable.Empty<FactionAlly>();
    
    [JsonIgnore]
    public IEnumerable<string> AllFactionKeywords
    {
        get
        {
            yield return FactionKeyword;
            foreach (var keyword in AdditionalFactionKeywords ?? [])
                yield return keyword;
        }
    }
}
