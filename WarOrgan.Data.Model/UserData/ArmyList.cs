using System.Text.Json;
using System.Text.Json.Serialization;
using WarOrgan.Model.Settings;

namespace WarOrgan.Model.UserData;

public class ArmyList
{
    public Guid Id { get; set; } // This is the ID used by the sync'ing system and also the filename used
    public string? Name { get; set; }

    public int MaxPoints { get; set; } = 2000;
    public string Faction { get; set; } = "Unknown";
    public string? Disposition { get; set; }
    public List<UnitRef> Units { get; set; } = new();
    public bool IsLegendsEnabled { get; set; }
    public DateTime? LastModified { get; set; }
    public FactionColors FactionColors { get; set; } = FactionColors.Auto;

    // Nullable because lists may have been created before this was added, only access with DataSetId extension
    public string? CreatedWithDataSetId { get; set; } 

    public List<string> Detachments { get; set; } = new();
    public CustomFactionColors CustomFactionColors { get; set; } = new();

    [Obsolete($"Use {nameof(Detachments)} instead")]
    public string? Detachment { get; set; }

    // For preserving unknown properties on save, mainly for use
    // on the web server when I forget to update it after updating the ArmyList
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}
