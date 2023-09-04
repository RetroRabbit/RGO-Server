using Microsoft.Extensions.DependencyInjection;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;

namespace RGO.UnitOfWork;

public static class RegisterRepositoryExtension
{
    public static void RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
