using System.ComponentModel.DataAnnotations;

namespace DebtusTestTask.Models;

public class Employee
{
    [Key]
    public string Id { get; set; }

    public int EmpNumber { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string MiddleName { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}