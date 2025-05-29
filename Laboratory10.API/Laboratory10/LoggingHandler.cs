// LoggingHandler.cs
using Microsoft.Extensions.Logging;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    // Конструктор принимает только ILogger<LoggingHandler>
    public LoggingHandler(ILogger<LoggingHandler> logger)
        : base(new HttpClientHandler())
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Request: {Method} {Uri}", request.Method, request.RequestUri);
            
            var response = await base.SendAsync(request, ct);
            
            _logger.LogInformation("Response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request failed: {Method} {Uri}", request.Method, request.RequestUri);
            throw;
        }
    }
}