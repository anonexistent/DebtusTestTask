using DebtusTestTask.Infrastructure;
using DebtusTestTask.Integrations.OrangeHRM;
using DebtusTestTask.Integrations.OrangeHRM.Services;
using DebtusTestTask.Integrations.OrangeHRM.Services.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<DebtusContext>(options =>
    options.UseSqlite(defaultConnectionString), ServiceLifetime.Transient);

services.AddRepositories();
services.AddSingleton<OrangeHttpClient>();
services.AddOrangeServices();

services.AddAutoMapper((config) =>
    {
        config.AddProfile(typeof(OrderCreateBodyProfile));
        config.AddProfile(typeof(EmployeeCreteBodyProfile));
    }
);

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
