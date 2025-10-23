using System.Text.Json.Serialization;

namespace DebtusTestTask.Integrations.OrangeHRM.Contracts.Output;

public record SuccessJobResponse
{
    [JsonPropertyName("data")]
    public SuccessJobResponseData Data { get; set; }

    [JsonPropertyName("meta")]
    public object Meta { get; set; }

    [JsonPropertyName("rels")]
    public object Rels { get; set; }
    /*
     {
    "data": {
        "empNumber": 128,
        "joinedDate": null,
        "jobTitle": {
            "id": 6,
            "title": "Software Architect",
            "isDeleted": false
        },
        "jobSpecificationAttachment": {
            "id": null,
            "filename": null
        },
        "empStatus": {
            "id": 4,
            "name": "Full-Time Probation"
        },
        "jobCategory": {
            "id": 1,
            "name": "Officials and Managers"
        },
        "subunit": {
            "id": 4,
            "name": "Development",
            "unitId": ""
        },
        "location": {
            "id": 3,
            "name": "Canadian Regional HQ"
        },
        "employeeTerminationRecord": {
            "id": null,
            "date": null
        }
    },
    "meta": [],
    "rels": []
}
     */

};

public class SuccessJobResponseData : Dictionary<string, dynamic>
{
        
};