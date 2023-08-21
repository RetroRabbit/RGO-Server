using Microsoft.Extensions.DependencyInjection;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;
using RGO.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.UnitOfWork
{
    public static class RegisterRepositoryExtension
    {
        public static void RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
            services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
            services.AddScoped<IRoleAccessRepository, RoleAccessRepository>();
        }
    }
}
