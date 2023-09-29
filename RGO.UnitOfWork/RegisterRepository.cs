using Microsoft.Extensions.DependencyInjection;

namespace RGO.UnitOfWork;

public static class RegisterRepositoryExtension
{
    public static void RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
