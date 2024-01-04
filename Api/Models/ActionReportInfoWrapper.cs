using Redis.OM.Modeling;

namespace Api.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { nameof(ActionReportInfoWrapper) })]
public class ActionReportInfoWrapper
{
    [Indexed]
    public Dictionary<string, ActionReportInfo>? Statistics { get; set; }
}