﻿using HRIS.Services.Interfaces;
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
            finally
            {
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("Unauthorized Access");
                }
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                var errorResponse = await GetRequestInfo(context, exception);
                var message = "Internal Server Error. Please try again later.";

                switch (exception)
                {
                    case CustomException:
                        errorResponse.StatusCode = StatusCodes.Status404NotFound;
                        message = exception.Message;
                        break;
                    default:
                        errorResponse.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                await _errorLoggingService.LogException(errorResponse);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)errorResponse.StatusCode;

                await context.Response.WriteAsync(message);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e);
                Console.ResetColor();
            }
        }

        private async Task<ErrorLoggingDto> GetRequestInfo(HttpContext context, Exception exception)
        {
            var utcNow = DateTime.UtcNow;
            var targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
            var targetLocalTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, targetTimeZone);

            var log = new ErrorLoggingDto
            {
                DateOfIncident = targetLocalTime,
                Message = exception.Message,
                StackTrace = exception.ToString(),
                IpAddress = context.Request.Host.Host,
                RequestUrl = context.Request.GetDisplayUrl(),
                RequestMethod = context.Request.Method,
                RequestContentType = context.Request.ContentType,
            };

            var bodyAsString = string.Empty;
            context.Request.EnableBuffering();

            if (context.Request.Body != null)
            {
                context.Request.Body.Position = 0;
                using var requestStream = new StreamReader(context.Request.Body, leaveOpen: true);
                bodyAsString = await requestStream.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
            log.RequestBody = bodyAsString;

            return log;
        }
    }
}
