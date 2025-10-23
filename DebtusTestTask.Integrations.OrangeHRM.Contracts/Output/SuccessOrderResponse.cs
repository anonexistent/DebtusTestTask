using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Output;

public record SuccessOrderResponse
{
    [JsonPropertyName("data")]
    public SuccessOrderResponseData Data { get; set; }

    [JsonPropertyName("meta")]
    public object Meta { get; set; }

    [JsonPropertyName("rels")]
    public object Rels { get; set; }
}

public class SuccessOrderResponseData : Dictionary<string, dynamic>
{ }
