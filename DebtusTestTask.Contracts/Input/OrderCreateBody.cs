using System.Text.Json.Serialization;

namespace DebtusTestTask.Contracts.Input;

public record OrderCreateBody
{
    [JsonPropertyName("employeeNumber")]
    public required int EmployeeNumber { get; set; }

    [JsonPropertyName("claimEventId")]
    public required int ClaimEventId { get; set; }

    [JsonPropertyName("currencyId")]
    public required string CurrencyId { get; set; }

    [JsonPropertyName("remarks")]
    public required string Remarks { get; set; }
};
