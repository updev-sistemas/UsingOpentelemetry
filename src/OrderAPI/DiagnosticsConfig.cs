using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace OrderAPI;

public class DiagnosticsConfig
{
    public const string ServiceName = "OrderAPI.WebApi";
    public static ActivitySource ActivitySource = new(ServiceName);

    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter = Meter.CreateCounter<long>("OrderAPI.request_counter");
}
