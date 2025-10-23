using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Output;

public record SuccessEventResponse
{
    [JsonPropertyName("data")]
    public SuccessEventDataItemResponse[] Data { get; set; }

    [JsonPropertyName("meta")]
    public object Meta { get; set; }

    [JsonPropertyName("rels")]
    public object Rels { get; set; }

};

public record SuccessEventDataItemResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
};
