using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Output;

public record SuccessEmployeeResponse
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; } = true;
    [JsonPropertyName("employeeId")]
    public required string EmployeeId { get; set; }
};
