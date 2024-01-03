namespace Api.Models;

public class ActionReportInfo
{
    public int CallCount { get; set; }
    public Dictionary<int, int>? StatusCodesCount { get; set; }
}
