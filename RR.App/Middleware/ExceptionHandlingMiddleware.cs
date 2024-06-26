using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HRIS.Services.Interfaces;
using RR.UnitOfWork.Entities;
using ATS.Models;
using HRIS.Services;
using HRIS.Services.Services;

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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
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
                    // Log the exception if it's not a known custom exception
                    _errorLoggingService.LogException(exception);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorLoggingDto
            {
                StatusCode = statusCode,
                Message = message
            };

            var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(errorJson);
        }

    }
}
