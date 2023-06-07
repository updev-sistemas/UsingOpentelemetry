using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace UsingOpentelemetry;

public static class DiagnosticsConfig
{
    public const string ServiceName = "ProductAPI";
    public static ActivitySource ActivitySource = new(ServiceName);

    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter = Meter.CreateCounter<long>("ProductAPI.request_counter");
}
