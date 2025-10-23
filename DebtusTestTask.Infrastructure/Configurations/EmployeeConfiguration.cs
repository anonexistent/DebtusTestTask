using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DebtusTestTask.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        Console.WriteLine("eployee configuring...");

        builder
            .HasIndex(o => o.Id)
            .IsUnique();

        builder
            .Property(e => e.FirstName)
            .IsRequired();

        builder
            .Property(e => e.LastName)
            .IsRequired();

        //builder.HasMany<Order>(x=>x.Orders)
        //    .WithOne();
    }
}
