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
using Newtonsoft.Json;
using HRIS.Services.Session;

namespace RR.App
{
    public class Program
    {
        private static readonly Lazy<JsonWebKeySet> LazyJsonWebKeySet = new Lazy<JsonWebKeySet>(FetchJsonWebKeySet);

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            ConfigureConfigurationSources(configuration);
            RegisterServices(builder.Services, configuration);
            ConfigureSwagger(builder.Services);
            ConfigureAuthentication(builder.Services, configuration);
            ConfigureAuthorizationPolicies(builder.Services, configuration);

            var app = builder.Build();
            ConfigureApp(app);

            await app.RunAsync();
        }

        private static void ConfigureConfigurationSources(ConfigurationManager configuration)
        {
            configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration.AddUserSecrets<Program>();
            configuration.AddEnvironmentVariables();
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusConnectionString = configuration["NewEmployeeQueue:ConnectionString"];
            var serviceBusQueueName = configuration["ServiceBus:QueueName"];
            var serviceBusClient = new ServiceBusClient(serviceBusConnectionString);

            services.AddSingleton(new EmployeeDataConsumer(serviceBusClient, serviceBusQueueName));
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default")), ServiceLifetime.Transient);

            services.RegisterRepository();
            services.RegisterServicesHRIS();
            services.RegisterServicesATS();
            services.AddScoped<AuthorizeIdentity>();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
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
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("AuthManagement");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = authSettings["Issuer"],
                        ValidAudience = authSettings["Audience"],
                        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                            LazyJsonWebKeySet.Value?.Keys ?? throw new InvalidOperationException("JsonWebKeySet is not available.")
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                            var roleClaims = claimsIdentity!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                            if (roleClaims != null)
                            {
                                AddRolesToClaims(claimsIdentity, roleClaims);
                                claimsIdentity.RemoveClaim(roleClaims);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void ConfigureAuthorizationPolicies(IServiceCollection services, IConfiguration configuration)
        {
            var policies = configuration.GetSection("Security:AuthorizationPolicies:Policies").Get<List<PolicySettings>>();

            policies.ForEach(policySettings =>
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(policySettings.Name, policy =>
                    {
                        policy.RequireRole(policySettings.Roles);
                        if (policySettings.Permissions?.Any() == true)
                        {
                            policy.RequireClaim("permissions", policySettings.Permissions);
                        }
                    });
                });
            });
        }

        private static void ConfigureApp(WebApplication app)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }

        private static JsonWebKeySet FetchJsonWebKeySet()
        {
            var jwksUrl = $"{Environment.GetEnvironmentVariable("AuthManagement__Issuer")}.well-known/jwks.json";
            using var httpClient = new HttpClient();
            var jwksResponse = httpClient.GetStringAsync(jwksUrl).Result;
            return new JsonWebKeySet(jwksResponse);
        }

        private static void AddRolesToClaims(ClaimsIdentity claimsIdentity, Claim roleClaims)
        {
            try
            {
                var roles = JArray.Parse(roleClaims.Value);
                foreach (var role in roles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }
            catch (JsonReaderException)
            {
                var roles = roleClaims.Value.Split(',');
                foreach (var role in roles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));
                }
            }
        }
    }
}
