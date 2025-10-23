using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Output;

public record SuccessEmployeeGetResponse
{
    [JsonPropertyName("data")]
    public SuccessEmployeeResponseData[] Data { get; set; }

    [JsonPropertyName("meta")]
    public object Meta { get; set; }

    [JsonPropertyName("rels")]
    public object Rels { get; set; }
};