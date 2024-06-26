using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HRIS.Services.Interfaces;
using RR.UnitOfWork.Entities;
using ATS.Models;
using HRIS.Services;
using HRIS.Services.Services;

namespace YourNamespace.Middleware
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
           _errorLoggingService.LogException(exception, context.Response.StatusCode, exception.ToString());

            context.Response.ContentType = "application/json";

            if (exception is CustomNotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return context.Response.WriteAsync(new ErrorLoggingDto
                {
                    StatusCode = context.Response.StatusCode,
                    Message = exception.Message
                }.ToString());
            }
            else if (exception is CustomBadRequestException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return context.Response.WriteAsync(new ErrorLoggingDto
                {
                    StatusCode = context.Response.StatusCode,
                    Message = exception.Message
                }.ToString());
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return context.Response.WriteAsync(new ErrorLoggingDto
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error. Please try again later."
                }.ToString());
            }
        }
    }
}
