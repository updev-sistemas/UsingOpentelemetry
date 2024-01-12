using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using System.Diagnostics;
using OpenTelemetry.Logs;

namespace OrderAPI
{
    public class LocalDynatrace
    {
        public static string DT_API_URL = "https://xap74085.live.dynatrace.com/";
        public static string DT_API_TOKEN = "dt0c01.43JMCNAKPPIKKK3UJBGDMO44.YGOCC6FCB5TXRUE6SURZFNMTCIPUE3EPVOD5ZIXMUGBPJD7GWV67OOAYU4WYWYUV";

        public const string activitySource = "Atlantico.PoC.WebApi";
        public static readonly ActivitySource MyActivitySource = new(activitySource);
        public static ILoggerFactory? loggerFactoryOT;

        public static void InitOpenTelemetry(IServiceCollection services)
        {
            List<KeyValuePair<string, object>> dt_metadata = new List<KeyValuePair<string, object>>();
            foreach (string name in new string[]
                {
                    "dt_metadata_e617c525669e072eebe3d0f08212e8f2.properties",
                    "/var/lib/dynatrace/enrichment/dt_metadata.properties",
                    "/var/lib/dynatrace/enrichment/dt_host_metadata.properties"
                })
            {
                try
                {
                    foreach (string line in System.IO.File.ReadAllLines(name.StartsWith("/var") ? name : System.IO.File.ReadAllText(name)))
                    {
                        var keyvalue = line.Split("=");
                        dt_metadata.Add(new KeyValuePair<string, object>(keyvalue[0], keyvalue[1]));
                    }
                }
                catch { }
            }

            Action<ResourceBuilder> configureResource = r => r
                .AddService(serviceName: "dotnet-quickstart") //TODO Replace with the name of your application
                .AddAttributes(dt_metadata);

            services.AddOpenTelemetry()
                .ConfigureResource(configureResource)
                .WithTracing(builder => {
                    builder
                        .SetSampler(new AlwaysOnSampler())
                        .AddSource(MyActivitySource.Name)
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri($"{DT_API_URL}/v2/traces");
                            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                            options.Headers = $"Authorization=Api-Token {DT_API_TOKEN}";
                        });
                })
                .WithMetrics(builder => {
                    builder
                        .AddMeter("my-meter")
                        .AddOtlpExporter((OtlpExporterOptions exporterOptions, MetricReaderOptions readerOptions) =>
                        {
                            exporterOptions.Endpoint = new Uri($"{DT_API_URL}/v2/metrics");
                            exporterOptions.Headers = $"Authorization=Api-Token {DT_API_TOKEN}";
                            exporterOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                            readerOptions.TemporalityPreference = MetricReaderTemporalityPreference.Delta;
                        });
                });

            var resourceBuilder = ResourceBuilder.CreateDefault();
            configureResource!(resourceBuilder);

            loggerFactoryOT = LoggerFactory.Create(builder => {
                builder
                    .AddOpenTelemetry(options => {
                        options.SetResourceBuilder(resourceBuilder).AddOtlpExporter(options => {
                            options.Endpoint = new Uri($"{DT_API_URL}/v2/logs");
                            options.Headers = $"Authorization=Api-Token {DT_API_TOKEN}";
                            options.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;
                            options.Protocol = OtlpExportProtocol.HttpProtobuf;
                        });
                    })
                    .AddConsole();
            });
            Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                .AddSource(MyActivitySource.Name)
                .ConfigureResource(configureResource);
            // add-logging
        }
    }
}
