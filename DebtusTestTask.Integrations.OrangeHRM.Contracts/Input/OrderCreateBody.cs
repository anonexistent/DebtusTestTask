using System.Text.Json;
using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;

public record OrderCreateBody
{
    [JsonPropertyName("claimEventId")]
    public required int ClaimEventId { get; set; }

    [JsonPropertyName("currencyId")]
    public required string CurrencyId { get; set; }

    [JsonPropertyName("remarks")]
    public required string Remarks { get; set; }

    public static implicit operator HttpContent(OrderCreateBody v)
    {
        var json = JsonSerializer.Serialize(v);
        return new StringContent(json, default, "application/json");
    }
};
