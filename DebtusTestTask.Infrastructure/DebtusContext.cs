using DebtusTestTask.Infrastructure.Configurations;
using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace DebtusTestTask.Infrastructure;

public class DebtusContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    public DebtusContext(DbContextOptions<DebtusContext> options) 
        : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
