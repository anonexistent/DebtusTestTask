using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Infrastructure;
using DebtusTestTask.Infrastructure.Data;
using DebtusTestTask.Integrations.OrangeHRM;
using DebtusTestTask.Integrations.OrangeHRM.Services;
using DebtusTestTask.Integrations.OrangeHRM.Services.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.WithProperty("Application", Assembly.GetEntryAssembly().GetName().Name);
});

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

services.AddOrangeHRM();

services.AddDbContext<DebtusContext>(options =>
    options
    .UseSqlite(defaultConnectionString)
    //  В EF 9 появились UseSeeding и UseAsyncSeeding методы
    //.UseAsyncSeeding()
    , ServiceLifetime.Singleton);

services.AddRepositories();
services.AddScoped<EmployeeDataSeeder>();
services.AddTransient<IEventRepository, DebtusTestTask.Integrations.OrangeHRM.Services.Repositories.EventRepository>();
services.AddOrangeServices();

services.AddAutoMapper((config) =>
    {
        config.AddProfile(typeof(OrderCreateBodyProfile));
        config.AddProfile(typeof(EmployeeCreteBodyProfile));
        config.AddProfile(typeof(JobCreateBodyProfile));
    }
);

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Выполнение сидинга
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<EmployeeDataSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
