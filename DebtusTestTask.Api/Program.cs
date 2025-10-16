using DebtusTestTask.Infrastructure;
using DebtusTestTask.Services;
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
services.AddServices();

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
