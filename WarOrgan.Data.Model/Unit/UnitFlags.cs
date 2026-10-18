namespace WarOrgan.Model.Unit;

public enum UnitFlags : ulong
{ 
    None = 0,
    NotWarlord = 1,
    MustBeWarlord = 2,
    NoEnhancements = 4,
    MayBeWarlord = 8, // Regardless of Character keyword
    All = NotWarlord | MustBeWarlord | NoEnhancements | MayBeWarlord
}

