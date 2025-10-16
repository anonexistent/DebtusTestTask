using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Input;

public record EmployeeCreateBody
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }

    [JsonPropertyName("middleName")]
    public required string MiddleName { get; set; }

    [JsonPropertyName("joinedDate")]
    public required DateTime JoinedDate { get; set; }

    [JsonPropertyName("job")]
    public required JobCreateBody Job { get; set; }
};