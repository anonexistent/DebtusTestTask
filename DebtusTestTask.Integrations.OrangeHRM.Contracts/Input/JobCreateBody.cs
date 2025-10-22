using System.Text.Json;
using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;

public record JobCreateBody
{
    [JsonPropertyName("joinedDate")]
    public required DateTime JoinedDate { get; set; }

    [JsonPropertyName("jobTitleId")]
    public required int JobTitle { get; set; }

    [JsonPropertyName("jobCategory")]
    public required string JobCategory { get; set; }

    [JsonPropertyName("subUnit")]
    public required string SubUnit { get; set; }

    [JsonPropertyName("location")]
    public required string Location { get; set; }

    [JsonPropertyName("employmentStatus")]
    public required string EmploymentStatus { get; set; }

    public static implicit operator HttpContent(JobCreateBody v)
    {
        var json = JsonSerializer.Serialize(v);
        return new StringContent(json, default, "application/json");
    }
};
