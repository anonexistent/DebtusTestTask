using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Output;

public record SuccessOrderResponse
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; } = true;
    [JsonPropertyName("referenceId")]
    public required ulong ReferenceId { get; set; }
};
