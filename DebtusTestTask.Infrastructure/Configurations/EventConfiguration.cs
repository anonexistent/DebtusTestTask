using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DebtusTestTask.Infrastructure.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasData([
            new Event() { Id = 1, Name = "Accommodation" },
            new Event() { Id = 2, Name = "Medical Reimbursement" },
            new Event() { Id = 3, Name = "Travel Allowance" },
        ]);
    }
}
