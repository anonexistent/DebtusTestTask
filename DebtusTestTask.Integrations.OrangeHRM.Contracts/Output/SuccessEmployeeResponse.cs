using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Output;

/*
 {
        "empNumber": 267,
        "lastName": "asd",
        "firstName": "asd_postman",
        "middleName": "asd",
        "employeeId": "048511111",
        "terminationId": null
}
 */

public record SuccessEmployeeResponse
{
    [JsonPropertyName("data")]
    public SuccessEmployeeResponseData Data { get; set; }

    [JsonPropertyName("meta")]
    public object Meta { get; set; }

    [JsonPropertyName("rels")]
    public object Rels { get; set; }
};

public record SuccessEmployeeResponseData
{
    [JsonPropertyName("empNumber")]
    public int EmpNumber { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("middleName")]
    public string MiddleName { get; set; }

    [JsonPropertyName("employeeId")]
    public string EmpId { get; set; }

    [JsonPropertyName("terminationId")]
    public string? TerminationId { get; set; }
};
