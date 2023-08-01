using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.OpenApi.Models;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
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

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args)
                        .Build();

                    services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
                })
                .Build();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Grad Onboarding Platform API", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Provide Auth0 Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IEventsService, EventsService>();
            builder.Services.AddScoped<IEventsRepository, EventsRepository>();
            builder.Services.AddScoped<IUserGroupsRepository, UserGroupsRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IWorkshopRepository, WorkshopRepository>();
            builder.Services.AddScoped<IWorkshopService, WorkshopService>();

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
                await next(context);
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}