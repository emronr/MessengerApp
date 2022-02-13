using MessengerApp.API.Middlewares;

namespace MessengerApp.API.Extensions;

public static class RequestResponseMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseMiddleware>();
    }
}