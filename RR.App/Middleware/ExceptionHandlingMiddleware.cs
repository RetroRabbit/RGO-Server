using HRIS.Services.Interfaces;
using ATS.Models;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Http.Extensions;


namespace Hris.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorLoggingService _errorLoggingService;

        public ExceptionHandlingMiddleware(RequestDelegate next, IErrorLoggingService errorLoggingService)
        {
            _next = next;
            _errorLoggingService = errorLoggingService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = await GetRequestInfo(context);

            switch (exception)
            {
                case CustomException:
                    errorResponse.StatusCode = StatusCodes.Status404NotFound;
                    errorResponse.Message = exception.Message;
                    break;
                default:
                    _errorLoggingService.LogException(exception);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)errorResponse.StatusCode;

            var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(errorJson);
        }

        private async Task<ErrorLoggingDto> GetRequestInfo(HttpContext context)
        {
            var log = new ErrorLoggingDto
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Internal Server Error. Please try again later."
            };
            var bodyAsString = string.Empty;
            context.Request.EnableBuffering();

            if (context.Request.Body != null)
            {
                context.Request.Body.Position = 0;  
                using StreamReader requestStream = new StreamReader(context.Request.Body, leaveOpen: true);
                bodyAsString = await requestStream.ReadToEndAsync();
                context.Request.Body.Position = 0;  
            }

            log.IpAddress = context.Request.Host.Host;
            log.RequestUrl = context.Request.GetDisplayUrl();
            log.RequestMethod = context.Request.Method;
            log.RequestContentType = context.Request.ContentType;
            log.RequestBody = bodyAsString;

            return log;
        }

    }
}
