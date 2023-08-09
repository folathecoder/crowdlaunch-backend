public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logFilePath;

    public ExceptionLoggingMiddleware(RequestDelegate next, string logFilePath)
    {
        _next = next;
        _logFilePath = logFilePath;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            LogException(ex);
            await context.Response.WriteAsync("There was an error. Please try again later.");
        }
    }

    private void LogException(Exception ex)
    {
        try
        {
            File.AppendAllText(_logFilePath, $"{DateTime.Now} - {ex.ToString()}{Environment.NewLine}");
        }
        catch
        {
            // Ignore exceptions that occur during the logging process to prevent potential infinite loops.

        }
    }
}
