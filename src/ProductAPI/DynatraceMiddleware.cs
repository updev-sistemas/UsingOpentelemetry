using System.Diagnostics;
using System.Reflection;

namespace ProductAPI
{
    public class DynatraceMiddleware
    {
        private readonly RequestDelegate _next;

        public DynatraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyName = assembly!.GetName();

            var version = assemblyName!.Version;
            var buildNumber = version!.Build;

            // Lógica a ser executada antes de chegar ao controller
            using var activity = LocalDynatrace.MyActivitySource.StartActivity("DynatraceMiddleware", ActivityKind.Consumer);

            activity?.SetTag("http.method", "GET");
            activity?.SetTag("net.protocol.version", "1.1");
            activity?.SetTag("application.name", "Dynatrace.PoC.WebApi");
            activity?.SetTag("application.environment", "Localhost (DEV1)");
            activity?.SetTag("application.version", version);
            activity?.SetTag("application.version.build", buildNumber);

            await _next(context);
        }
    }
}
