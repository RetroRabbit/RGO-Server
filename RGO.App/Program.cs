using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RGO.Services;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using System.Security.Claims;
using System.Text;

namespace RGO.App
{
    public class Program
    {
        public static void Main(params string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            /// <summary>
            /// Adds Swagger to the project and configures it to use JWT Bearer Authentication
            /// </summary>
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
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(configuration["ConnectionStrings:Default"]));
            builder.Services.RegisterRepository();
            builder.Services.RegisterServices();

            /// <summary>
            /// Add authentication with JWT bearer token to the application
            /// and set the token validation parameters.
            /// </summary>
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Auth:Issuer"]!,
                        ValidAudience = configuration["Auth:Audience"]!,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:Key"]!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                            Claim? roleClaims = claimsIdentity!.Claims
                                .FirstOrDefault(c => c.Type == ClaimTypes.Role);
                            if (roleClaims != null)
                            {
                                var roles = roleClaims.Value.Split(",");
                                foreach (var role in roles)
                                {
                                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                                }
                                claimsIdentity.RemoveClaim(roleClaims);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            /// <summary>
            /// Authorization policies
            /// e.g: options.AddPolicy([Policy Name], policy => policy.RequireRole([Role in DB and enum]));
            /// </summary>
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "isAdmin",
                    policy => policy.RequireRole("ADMIN"));
                options.AddPolicy(
                    "isGrad",
                    policy => policy.RequireRole("GRAD"));
                options.AddPolicy(
                    "isMentor",
                    policy => policy.RequireRole("MENTOR"));
                options.AddPolicy(
                    "isPresenter",
                    policy => policy.RequireRole("PRESENTER"));
            });

            var app = builder.Build();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}