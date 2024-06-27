using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HRIS.Services.Interfaces;
using RR.UnitOfWork.Entities;
using ATS.Models;
using HRIS.Services;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text;

namespace Hris.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorLoggingService _errorLoggingService;
        private readonly MemoryStream _recyclableMemoryStreamManager;

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

        private async Task<Task> HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var message = "Internal Server Error. Please try again later.";

            switch (exception)
            {
                case CustomException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;
                default:
                    _errorLoggingService.LogException(exception);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var type = exception.GetType().Name;
            var url = context.Request.GetDisplayUrl();
            var error = await GetRequestInfo(context);

            var errorResponse = new ErrorLoggingDto
            {
                StatusCode = statusCode,
                Message = message,
                
            };

            var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);

            return context.Response.WriteAsync(errorJson);
        }

        private async Task<ErrorLoggingDto> GetRequestInfo(HttpContext context)
        {
            var log = new ErrorLoggingDto();
            var bodyAsString = string.Empty;
            context.Request.EnableBuffering();

            if (context.Request.Body != null)
            {
                using StreamReader requestStream = new StreamReader(context.Request.Body);
                bodyAsString = await requestStream.ReadToEndAsync();
            }

            log.IpAddress = $"{context.Request.Host.Host}";
            log.RequestUrl = $"{context.Request.GetDisplayUrl()}";
            log.RequestMethod = $"{context.Request.Method}";
            log.RequestContentType = context.Request.ContentType;
            log.RequestBody = bodyAsString;

            return log;
        }
    }
}
