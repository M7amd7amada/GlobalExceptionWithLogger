using Redis.OM.Modeling;

namespace Api.Models;

public class ActionReportInfo
{
    [Indexed]
    public int CallCount { get; set; }

    [Indexed]
    public Dictionary<int, int>? StatusCodesCount { get; set; }
}
