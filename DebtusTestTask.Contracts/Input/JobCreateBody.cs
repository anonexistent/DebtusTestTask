using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Input;

public record JobCreateBody
{
    [JsonPropertyName("jobTitle")]
    public required string JobTitle { get; set; }
    [JsonPropertyName("jobCategory")]
    public required string JobCategory { get; set; }
    [JsonPropertyName("subUnit")]
    public required string SubUnit { get; set; }
    [JsonPropertyName("location")]
    public required string Location { get; set; }
    [JsonPropertyName("employmentStatus")]
    public required string EmploymentStatus { get; set; }
};
