using RGO.Domain.Interfaces.Repository;
using RGO.Repository.Interfaces;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Services;
using RGO.Repository;
using RGO.Repository.Repositories;

namespace ROG.App
{
    public class Program
    {
        public static void Main(params string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IUserGroupsRepository, UserGroupsRepository>();
            builder.Services.AddDbContext<DatabaseContext>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}