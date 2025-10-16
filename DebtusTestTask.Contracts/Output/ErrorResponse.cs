using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Output;

public record ErrorResponse
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; } = false;
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
};
