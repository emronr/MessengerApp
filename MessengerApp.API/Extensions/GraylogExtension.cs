using System.Reflection;
using Gelf.Extensions.Logging;
using NLog.Web;
using Serilog;

namespace MessengerApp.API.Extensions;

public static class GraylogExtension
{
    public static IWebHostBuilder AddGraylog(this IWebHostBuilder webHostBuilder)
    {
        return webHostBuilder.ConfigureLogging((context, configureLogging) =>
            configureLogging.AddGelf(options =>
            {
                configureLogging.ClearProviders();
                configureLogging.SetMinimumLevel(LogLevel.Information);
                options.Host = context.Configuration.GetSection("Logging:GELF:Host").Value;
                options.Port = Convert.ToInt32(context.Configuration.GetSection("Logging:GELF:Port").Value);
                options.LogSource = context.Configuration.GetSection("Logging:GELF:LogSource").Value;
                options.IncludeMessageTemplates = context.Configuration.GetValue<bool>("Logging:GELF:IncludeMessageTemplates");
                options.AdditionalFields["machine_name"] = Environment.MachineName;
                options.AdditionalFields["app_version"] = Assembly.GetEntryAssembly()
                    ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    ?.InformationalVersion;
            })).UseNLog();
    }
}