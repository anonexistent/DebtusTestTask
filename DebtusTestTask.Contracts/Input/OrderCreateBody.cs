using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Input;

public record OrderCreateBody
{
    [JsonPropertyName("employeeId")]
    public required string EmployeeId { get; set; }
    [JsonPropertyName("event")]
    public required string Event { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("remarks")]
    public required string Remarks { get; set; }
};
