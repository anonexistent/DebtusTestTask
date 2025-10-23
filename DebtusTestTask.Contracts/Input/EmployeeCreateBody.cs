using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Input;

public record EmployeeCreateBody
{
    [JsonPropertyName("employeeId")]
    public required string Id { get; set; }
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }

    [JsonPropertyName("middleName")]
    public required string MiddleName { get; set; }

    [JsonPropertyName("job")]
    public required JobCreateBody Job { get; set; }
};