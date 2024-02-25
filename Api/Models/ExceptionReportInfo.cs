using Redis.OM.Modeling;

namespace Api.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "ExceptionReportInfoWrapper" })]
public class ExceptionReportInfo
{
    [Indexed]
    public string ExMessage { get; set; } = default!;

    [Indexed]
    public string InnerExMessage { get; set; } = default!;

    [Indexed]
    public string ExceptionType { get; set; } = default!;

    [Indexed]
    public string ActionName { get; set; } = default!;
    [Indexed]
    public string ControllerName { get; set; } = default!;

    [Indexed]
    public string Parameters { get; set; } = default!;
    [Indexed]
    public int Response { get; set; } = default!;

    [Indexed]
    public DateTime Created { get; set; } = default!;
}