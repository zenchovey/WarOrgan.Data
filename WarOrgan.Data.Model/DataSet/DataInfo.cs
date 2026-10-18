namespace WarOrgan.Model.DataSet;

public record class DataSetInfo(
    string? Id,
    Version? Version,
    int? DataSetFormatVersion,
    DateTime? PublishDate)
{
    public const string FileName = "DataSetInfo.json";
    public const int CurrentDataSetFormatVersion = 1;

    public DataSetInfo()
        : this(null, null, null, null)
    { }

    public override string ToString() => $"DataSetInfo({Id}, {Version}, {PublishDate?.ToShortDateString()})";

    public static DataSetInfo CreateNotFoundInfo(string id) => new DataSetInfo(id, null, null, null);
}
