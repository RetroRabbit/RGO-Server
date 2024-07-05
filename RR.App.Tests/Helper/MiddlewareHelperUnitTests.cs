using HRIS.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Tests.Helper
{
    public static class MiddlewareHelperUnitTests
    {
        public static async Task<IActionResult> SimulateHandlingExceptionMiddlewareAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action.Invoke();
            }
            catch (CustomException customException)
            {
                return new NotFoundObjectResult(customException.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}