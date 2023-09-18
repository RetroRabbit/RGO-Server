using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RGO.Services;
using RGO.UnitOfWork;
using System.Security.Claims;
using System.Text;
using RGO.Models;
using RGO.Services.Services;
using RabbitMQ.Client;

namespace RGO.App
{
    public class Program
    {
        public static async Task Main(params string[] args)
        {
             ConnectionFactory _factory;
            _factory = new ConnectionFactory();
            _factory.UserName = "my-rabbit";
            _factory.UserName = "guest";
            _factory.Password = "guest";
            EmployeeDataConsumer emailer = new EmployeeDataConsumer(_factory);
            EmployeeService._employeeFactory = _factory;

            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;
            configuration.AddJsonFile("appsettings.json");

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
                            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                            var issuer = context.Principal.FindFirst(JwtRegisteredClaimNames.Iss)?.Value;
                            var audience = context.Principal.FindFirst(JwtRegisteredClaimNames.Aud)?.Value;

                            if (issuer != configuration["Auth:Issuer"] || audience != configuration["Auth:Audience"])
                            {
                                context.Fail("Token is not valid");
                                return Task.CompletedTask;
                            }

                            Claim? roleClaims = claimsIdentity.Claims
                                .FirstOrDefault(c => c.Type == ClaimTypes.Role);
                            
                            if (roleClaims == null) return Task.CompletedTask;

                            var roles = roleClaims.Value.Split(",");

                            foreach (var role in roles)
                            {
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                            }

                            claimsIdentity.RemoveClaim(roleClaims);
                            return Task.CompletedTask;
                        }
                    };
                });

            /// <summary>
            /// Authorization policies
            /// e.g: options.AddPolicy([Policy Name], policy => policy.RequireRole([Role in DB and enum]));
            /// </summary>

            var confBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var policies = confBuilder
                .AsEnumerable()
                .Where(x => x.Key.Contains("Security") && x.Value?.Length > 0)
                .GroupBy(x =>
                {
                    var split = x.Key.Split(":");
                    return $"{split[0]}:{split[1]}:{split[2]}:{split[3]}";
                })
                .ToDictionary(x => x.Key, x =>
                    x.GroupBy(y => y.Key.Split(":")[4])
                    .ToDictionary(
                        y => y.Key,
                        y => y.Select(x => x.Value).ToList()));

            policies
                .Where(p => p.Value.Count == 2)
                .Select(p => p.Key)
                .ToList()
                .ForEach(key =>
                {
                    policies[key]["Permissions"] = new List<string>();
                });

            new AuthorizationPolicySettings
            {
                Policies = policies
                .Select(policy => new PolicySettings(
                    policy.Value["Name"].First(),
                    policy.Value["Roles"],
                    policy.Value["Permissions"]))
                .ToList()
            }.Policies.ForEach(policySettings =>
            {
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy(
                        policySettings.Name,
                        policy =>
                        {
                            policy.RequireRole(policySettings.Roles);
                            if (policySettings.Permissions.Count > 0)
                                policy.RequireClaim("Permission", policySettings.Permissions);
                        });
                });
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