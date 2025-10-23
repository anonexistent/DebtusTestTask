using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DebtusTestTask.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        Console.WriteLine("order configuring...");

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(o=>o.Id)            
            .IsUnique();

        builder
            .Property(e => e.Event)
            .IsRequired();

        builder
            .Property(e => e.Currency)
            .IsRequired();

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId);
    }
}
