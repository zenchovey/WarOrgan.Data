namespace WarOrgan.Model.UserData;

public class UnitRef
{ 
    public int InstanceId { get; set; } // Unique within the containing army list
    public int AttachedToInstanceId { get; set; } // The UnitRef InstanceId this UnitRef is attached to

    public string Id { get; set; } = "Unknown"; // unit definition id - unit name
    public string? EnhancementId { get; set; }
    public List<string> Upgrades { get; set; } = new List<string>();
    public List<ModelWargearRef> UnitCompositionRef { get; set; } = new List<ModelWargearRef>();
    public UnitNotes Notes { get; set; } = new UnitNotes();
    public bool IsWarlord { get; set; }

    public string? AttachedUnitName { get; set; }
    public AttachedUnitNameType AttachedUnitNameType { get; set; }

    public static UnitRef Empty { get; } = new UnitRef();
}
