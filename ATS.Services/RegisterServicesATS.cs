using ATS.Services.Interfaces;
using ATS.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ATS.Services;

public static class RegisterServicesATSExtension
{
    public static void RegisterServicesATS(this IServiceCollection services)
    {
        services.AddScoped<ICandidateService, CandidateService>();
    }
}
