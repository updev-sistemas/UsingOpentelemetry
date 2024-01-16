using Database.Contexts;
using Domain.Mapper;
using Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using OrderAPI;
using Services;
using Services.Contracts;
using Services.HttpClient;
using Refit;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var dynatraceSection = builder.Configuration.GetSection("Dynatrace");
string dynatraceUrl = dynatraceSection["Url"] ?? string.Empty;
string dynatraceApiToken = dynatraceSection["ApiToken"] ?? string.Empty;
string dynatraceIngestUrl = dynatraceSection["IngestUrl"] ?? string.Empty;
string connectionString = builder.Configuration.GetConnectionString("DB") ?? string.Empty;
string customerApi = builder.Configuration.GetConnectionString("Product") ?? string.Empty;
string productApi = builder.Configuration.GetConnectionString("Customer") ?? string.Empty;

ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
ArgumentNullException.ThrowIfNull(dynatraceApiToken, nameof(dynatraceApiToken));
ArgumentNullException.ThrowIfNull(dynatraceUrl, nameof(dynatraceUrl));
ArgumentNullException.ThrowIfNull(dynatraceIngestUrl, nameof(dynatraceIngestUrl));
ArgumentNullException.ThrowIfNull(customerApi, nameof(customerApi));
ArgumentNullException.ThrowIfNull(productApi, nameof(productApi));

// Add services to the container.
LocalDynatrace.InitOpenTelemetry(builder.Services, dynatraceUrl, dynatraceApiToken);

builder.Host.UseSerilog();
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.WriteTo.Dynatrace(accessToken: dynatraceApiToken, ingestUrl: dynatraceIngestUrl);
    lc.WriteTo.Console(LogEventLevel.Debug);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PoCDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(ProductMapper), typeof(OrderMapper), typeof(OrderItemMapper), typeof(CustomerMapper));

builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddRefitClient<ICustomerApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri(customerApi));

builder.Services.AddRefitClient<IProductApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri(productApi));

#region Jaeger with Opentelemetry
//builder.Services.AddOpenTelemetry()
//    .WithTracing(tracerProviderBuilder =>
//        tracerProviderBuilder
//            .AddSource(DiagnosticsConfig.ActivitySource.Name)
//            .ConfigureResource(resource => resource
//                .AddService(DiagnosticsConfig.ServiceName))
//            .AddHttpClientInstrumentation(options =>
//            {
//                options.EnrichWithHttpWebResponse = (activity, httpWebResponse) =>
//                {
//                    using var reader = new StreamReader(httpWebResponse.GetResponseStream());
//                    var content = reader.ReadToEnd();
//                    activity.SetTag("Http.Response.Body", content ?? "#### Nenhum Data ####");
//                };
//                // Note: Called for all runtimes.
//                options.EnrichWithException = (activity, exception) =>
//                {
//                    activity.SetTag("Http.Response.StackTrace", exception.StackTrace);
//                };
//            })
//            .AddAspNetCoreInstrumentation()
//            .AddSqlClientInstrumentation(options =>
//            {
//                options.Enrich = (activity, eventName, rawObject) =>
//                {
//                    if (rawObject is SqlCommand cmd)
//                    {
//                        activity.SetTag("db.CommandText", eventName);
//                        activity.SetTag("db.Query", cmd.CommandText);

//                        if (cmd.Parameters.Count > 0)
//                        {
//                            foreach (var param in cmd.Parameters)
//                            {
//                                try
//                                {
//                                    var obj = (Microsoft.Data.SqlClient.SqlParameter)param;
//                                    activity.SetTag($"db.Query.Parameter.{obj.ParameterName}", obj.Value);
//                                }
//                                catch
//                                {
//                                    _ = param;
//                                }
//                            }
//                        }
//                    }
//                };
//            })
//            .AddOtlpExporter(o =>
//            {
//                o.Endpoint = new Uri("http://localhost:4317");
//            })
//            .AddConsoleExporter()
//            .AddOtlpExporter())
//    .WithMetrics(metricsProviderBuilder =>
//        metricsProviderBuilder
//            .ConfigureResource(resource => resource
//                .AddService(DiagnosticsConfig.ServiceName))
//            .AddHttpClientInstrumentation()
//            .AddAspNetCoreInstrumentation()
//            .AddOtlpExporter(o =>
//            {
//                o.Endpoint = new Uri("http://localhost:4317");
//            })
//            .AddConsoleExporter()
//            .AddOtlpExporter());
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<DynatraceMiddleware>();

app.Run();
