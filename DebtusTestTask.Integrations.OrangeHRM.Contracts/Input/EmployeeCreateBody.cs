using System.Text.Json;
using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;

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

    public static implicit operator HttpContent(EmployeeCreateBody v)
    {
        var json = JsonSerializer.Serialize(v);
        return new StringContent(json, default , "application/json");
    }
};
