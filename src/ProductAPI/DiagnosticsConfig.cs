﻿using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace ProductAPI;

public class DiagnosticsConfig
{
    public const string ServiceName = "ProductAPI.WebApi";
    public static ActivitySource ActivitySource = new(ServiceName);

    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter = Meter.CreateCounter<long>("ProductAPI.request_counter");
}
