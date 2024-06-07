using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using HRIS.Services;
using RR.UnitOfWork;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using HRIS.Models;
using ATS.Services;
using Azure.Messaging.ServiceBus;
using HRIS.Services.Services;

namespace RR.App
{
    public class Program
    {
        public static async Task Main(params string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;
            configuration.AddJsonFile("appsettings.json");
            configuration.AddUserSecrets<Program>();

            var serviceBusConnectionString = Environment.GetEnvironmentVariable("NewEmployeeQueue__ConnectionString");
            var queueName = Environment.GetEnvironmentVariable("ServiceBus__QueueName");
            var serviceBusClient = new ServiceBusClient(serviceBusConnectionString);

            builder.Services.AddSingleton<EmployeeDataConsumer>(new EmployeeDataConsumer(serviceBusClient, queueName));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            /// <summary>
            /// Adds Swagger to the project and configures it to use JWT Bearer Authentication
            /// </summary>
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "HRIS API", Version = "v1" });
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

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Transient);
            builder.Services.RegisterRepository();
            builder.Services.RegisterServicesHRIS();
            builder.Services.RegisterServicesATS();

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

                        ValidIssuer = Environment.GetEnvironmentVariable("Auth__Issuer"),
                        ValidAudience = Environment.GetEnvironmentVariable("Auth__Audience"),
                        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                        {
                            var client = new HttpClient();
                            var jwksUrl = Environment.GetEnvironmentVariable("Auth__Issuer") + ".well-known/jwks.json";
                            var httpClient = new HttpClient();
                            var jwksResponse = httpClient.GetStringAsync(jwksUrl);
                            var keys = new JsonWebKeySet(jwksResponse.Result);
                            return keys.Keys;
                        }
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;

                            Claim? assignedRoleClaim = claimsIdentity!.Claims.FirstOrDefault(c => c.Type == "assignedRole");
                            if (assignedRoleClaim != null)
                            {
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, assignedRoleClaim.Value));
                                claimsIdentity.RemoveClaim(assignedRoleClaim);
                            }

                            Claim? roleClaims = claimsIdentity!.Claims.FirstOrDefault(c => c.Type == "role");
                            if (roleClaims != null)
                            {
                                var roles = JArray.Parse(roleClaims.Value);
                                foreach (var role in roles)
                                {
                                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
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

            var confBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
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
                    policies[key]["Permissions"] = new List<string?>();
                });

            new AuthorizationPolicySettings
            {
                Policies = policies
                .Select(policy => new PolicySettings {
                    Name = policy.Value["Name"].First(),
                    Roles = policy.Value["Roles"],
                    Permissions = policy.Value["Permissions"]})

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
                                policy.RequireClaim("permissions", policySettings.Permissions);
                        });
                });
            });

            var app = builder.Build();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
