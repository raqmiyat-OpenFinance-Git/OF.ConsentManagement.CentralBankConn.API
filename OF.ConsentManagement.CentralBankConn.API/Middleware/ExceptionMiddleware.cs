using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.Common;
using System.Text.Json;

namespace OF.ConsentManagement.CentralBankConn.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ExceptionMiddlewareLogger _logger;
    private readonly IHostEnvironment _env;
    private readonly SendPointInitialize _sendPointInitialize;

    public ExceptionMiddleware(RequestDelegate next, ExceptionMiddlewareLogger logger, IHostEnvironment env, SendPointInitialize sendPointInitialize)
    {
        _next = next;
        _logger = logger;
        _env = env;
        _sendPointInitialize = sendPointInitialize;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // continue the pipeline
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception occurred");

            Guid.TryParse(context.Items["X-Correlation-ID"]?.ToString(), out var correlationId);
            var endpoint = context.Request.Path;


            var log = AuditLogFactory.CreateAuditLog(
                correlationId: correlationId,
                sourceSystem: "CentralBankAPI",
                targetSystem: "CoreBankAPI",
                endpoint: endpoint,
                requestPayload: null,
                responsePayload: null,
                statusCode: "500",
                requestType: MessageTypeMappings.ConsentOperations, // fallback
                executionTimeMs: 0,
                errorMessage: ex.Message);

            await _sendPointInitialize.AuditLog!.Send(log);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ErrorResponse1
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                }
                : new ErrorResponse1
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred."
                };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = System.Text.Json.JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
public class ErrorResponse1
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
}

