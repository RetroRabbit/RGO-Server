using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Services;
using RGO.Repository;
using RGO.Repository.Repositories;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

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
            builder.Services.AddScoped<IEventsService, EventsService>();
            builder.Services.AddScoped<IEventsRepository, EventsRepository>();
            builder.Services.AddScoped<IUserGroupsRepository, UserGroupsRepository>();
            
            builder.Services.AddDbContext<DatabaseContext>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.Use( async (context, next) =>
            {
                if (!context.Request.Path.ToString().Contains("Authentication"))
                {
                    if (context.Request.Headers.TryGetValue("Authorization", out var authorization))
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(authorization.ToString().Replace("Bearer ",""));

                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token is missing.");
                        return;
                    }
                }
               

                // Call the next delegate/middleware in the pipeline.
                await next(context);
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}