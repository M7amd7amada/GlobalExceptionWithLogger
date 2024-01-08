using Redis.OM.Modeling;

namespace Api.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { nameof(ActionReportInfoWrapper) })]
public class ActionReportInfoWrapper
{
    [Indexed]
    public int Id { get; set; }

    [Indexed]
    public Dictionary<string, ActionReportInfo>? Statistics { get; set; }
}