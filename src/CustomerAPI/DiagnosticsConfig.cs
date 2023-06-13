using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace CustomerAPI;

public class DiagnosticsConfig
{
    public const string ServiceName = "CustomerAPI.WebApi";
    public static ActivitySource ActivitySource = new(ServiceName);

    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter = Meter.CreateCounter<long>("CustomerAPI.request_counter");
}
