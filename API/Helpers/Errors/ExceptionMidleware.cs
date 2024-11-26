using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Helpers.Errors;

public class MiExceptionMidleware {

    private readonly RequestDelegate _next;
    private readonly ILogger<MiExceptionMidleware> _logger;
    private readonly IWebHostEnvironment _env;

    public MiExceptionMidleware(RequestDelegate next, ILogger<MiExceptionMidleware> logger, IWebHostEnvironment env) {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace!.ToString())
                : new ApiException(context.Response.StatusCode, "Internal Server Error", ex.Message);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
