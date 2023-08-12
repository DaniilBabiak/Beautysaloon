using Serilog;

namespace BeautySaloon.API;

public class LoggingHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Логгируем заголовки
        Log.Information("Request Headers: {@Headers}", context.Request.Headers);

        await _next(context);
    }
}

