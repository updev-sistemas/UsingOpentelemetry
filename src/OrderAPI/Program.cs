using Database.Contexts;
using Domain.Mapper;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderAPI;
using Services;
using Services.Contracts;
using Services.HttpClient;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PoCDbContext>(options => options.UseSqlServer("Server=host.docker.internal,1433;Database=poc_opentelemetry;User Id=sa;Password=LltF8Nx*yo;TrustServerCertificate=True;"));
builder.Services.AddAutoMapper(typeof(ProductMapper), typeof(OrderMapper), typeof(OrderItemMapper), typeof(CustomerMapper));

builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddRefitClient<ICustomerApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7117"));

builder.Services.AddRefitClient<IProductApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7192"));

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(DiagnosticsConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri("http://localhost:4317");
            })
            .AddConsoleExporter()
            .AddOtlpExporter())
    .WithMetrics(metricsProviderBuilder =>
        metricsProviderBuilder
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri("http://localhost:4317");
            })
            .AddConsoleExporter()
            .AddOtlpExporter());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
