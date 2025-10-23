using System.Text.Json;
using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;

public record JobCreateBody
{
    [JsonPropertyName("joinedDate")]
    public DateTime? JoinedDate { get; set; }

    [JsonPropertyName("jobTitleId")]
    public required int JobTitleId { get; set; }

    [JsonPropertyName("jobCategoryId")]
    public required int JobCategoryId { get; set; }

    [JsonPropertyName("subunitId")]
    public required int SubUnitId { get; set; }

    [JsonPropertyName("locationId")]
    public required int LocationId { get; set; }
        
    [JsonPropertyName("empStatusId")]
    public required int EmploymentStatusId { get; set; }

    public static implicit operator HttpContent(JobCreateBody v)
    {
        var json = JsonSerializer.Serialize(v);
        return new StringContent(json, default, "application/json");
    }
};
