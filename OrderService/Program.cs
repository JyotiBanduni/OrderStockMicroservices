using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Repositories;
using OrderService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration() // injecting logging configuration into the application
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString("OrderDb")));

builder.Services.AddScoped<IOrderRepository,
    OrderRepository>();

builder.Services.AddScoped<IOrderService,
    OrderService.Services.OrderService>();

builder.Services.AddHttpClient<IProductServiceClient,
    ProductServiceClient>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration[
                "ProductServiceUrl"]!);
    });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();