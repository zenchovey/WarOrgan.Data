using WarOrgan.Model.Abstraction;

namespace WarOrgan.Model.Search;

public record SearchResult(string FactionKeyword, ISearchableModel Item);
