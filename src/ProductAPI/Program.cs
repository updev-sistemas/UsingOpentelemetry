using Database.Contexts;
using Domain.Mapper;
using Domain.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProductAPI;
using Serilog;
using Serilog.Events;
using Services;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LocalDynatrace.InitOpenTelemetry(builder.Services);

builder.Host.UseSerilog();
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.WriteTo.Dynatrace(accessToken: "dt0c01.43JMCNAKPPIKKK3UJBGDMO44.YGOCC6FCB5TXRUE6SURZFNMTCIPUE3EPVOD5ZIXMUGBPJD7GWV67OOAYU4WYWYUV", ingestUrl: "https://xap74085.live.dynatrace.com/api/v2/logs/ingest");
    lc.WriteTo.Console(LogEventLevel.Debug);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PoCDbContext>(options => options.UseSqlServer("Server=host.docker.internal,1433;Database=poc_opentelemetry;User Id=sa;Password=LltF8Nx*yo;TrustServerCertificate=True;"));
builder.Services.AddAutoMapper(typeof(ProductMapper), typeof(OrderMapper), typeof(OrderItemMapper), typeof(CustomerMapper));

builder.Services.AddTransient<IProductRepository, ProductRepository>();

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
