using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DebtusTestTask.Models;

public class Order
{
    [Key]
    public ulong Id { get; set; }
    public string Event { get; set; }
    public string Currency { get; set; }
    public string Remarks { get; set; }

    [ForeignKey("employeeId")]
    public string EmployeeId { get; set; }

    public virtual Employee Employee { get; set; }
}