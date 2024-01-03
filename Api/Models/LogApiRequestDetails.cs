using Redis.OM.Modeling;

namespace Api.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { nameof(LogApiRequestDetails) })]
public class LogApiRequestDetails
{
    [Indexed]
    public string HttpMethod { get; set; } = default!;

    [Indexed]
    public string Path { get; set; } = default!;

    [Indexed]
    public string QueryParameters { get; set; } = default!;

    [Indexed]
    public string ActionName { get; set; } = default!;

    [Indexed]
    public string ControllerName { get; set; } = default!;

    [Indexed]
    public int? StatusCode { get; set; } = default!;

    [Indexed]
    public string ExceptionType { get; set; } = default!;

    [Indexed]
    public DateTime Created { get; set; } = default!;
}
