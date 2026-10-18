using WarOrgan.Model.DataSet;
using WarOrgan.Model.Faction;

namespace WarOrgan.Data.BuildDataset.Utils;

public record DatasetBuildModel(
    DataSetInfo Info,
    DataSetDefinition Definition,
    List<FactionRosterEntry> Rosters,
    List<string> AdditionalFiles);

public record FactionRosterEntry(
    FactionRoster Roster, 
    Func<FactionRoster, string>? NameOverride = null);