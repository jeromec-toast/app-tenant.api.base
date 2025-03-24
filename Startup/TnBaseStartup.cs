using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Tenant.API.Base.Filter;
using Tenant.API.Base.Model;
using Tenant.API.Base.Util;

namespace Tenant.API.Base.Startup
{
    public static class TnBaseStartup
    {
        public static ILoggerFactory LoggerFactory;
        public static ILogger Logger;
        //Version of the api
        public static string Version;

        /// <summary>
        /// Startup the specified env.
        /// </summary>
        /// <returns>The startup.</returns>
        /// <param name="env">Env.</param>
        public static IConfigurationBuilder InitializeStartup(IWebHostEnvironment env)
        {

            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddEnvironmentVariables();

            if (env.IsProduction())
            {
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            }
            else
            {
                builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            return builder;
        }

        /// <summary>
        /// Initializes the services.
        /// </summary>
        /// <param name="configuration">I configuration.</param>
        /// <param name="services">Services.</param>
        public static void InitializeServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddCustomScoppedVariables()
                .AddCustomAuthentication(configuration)
                .AddCustomDbContext(configuration)
                .AddCors(options => options.AddPolicy("AllowAllOrigins",
                            builder =>
                            {
                                builder.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader();
                            }))
                .AddDefaultCorrelationId(x =>
                {
                    x.AddToLoggingScope = true;
                    x.UpdateTraceIdentifier = false;
                })
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });
        }

        /// <summary>
        /// Initializes the application.
        /// </summary>
        /// <param name="app">App.</param>
        /// <param name="env">Env.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public static void InitializeApplication(IConfiguration configuration, IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting()
                .UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(x => x.MapControllers())
                .UseCorrelationId()
                .UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"API v{Version}"));

            //add aws logging
            //Create a logging provider based on the configuration information passed through the appsettings.json
            //You can even provide your custom formatting.
            LoggerFactory = loggerFactory;
            //--LoggerFactory.AddAWSProvider(configuration.GetAWSLoggingConfigSection(), formatter: (logLevel, message, exception) => $"[{DateTime.UtcNow}] {logLevel}: {message}");
            Logger = LoggerFactory.CreateLogger("Startup");
        }

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            //adding MVC 
            services.AddMvc();

            //adding controller
            services.AddControllersWithViews();

            //adding MVC Razor page
            services.AddRazorPages();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddCustomScoppedVariables(this IServiceCollection services)
        {
            services.AddScoped<ApiActionFilter>()
                .AddScoped(typeof(Tenant.API.Base.Repository.TnAudit), typeof(Tenant.API.Base.Repository.TnAudit))
                .AddScoped(typeof(Tenant.API.Base.Repository.TnValidation), typeof(Tenant.API.Base.Repository.TnValidation));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        SaveSigninToken = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status406NotAcceptable;

                            ApiResult result = new ApiResult() { Exception = context.Exception.Message };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                        },
                        OnTokenValidated = context =>
                        {
                            var accessToken = context.SecurityToken as JwtSecurityToken;
                            Logger.LogInformation("JWT - OnTokenValidated: " + accessToken);

                            //adding claims to identity
                            ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;

                            if (identity != null)
                            {
                                identity.AddClaims(accessToken.Claims);
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Tenant.API.Base.Context.TnAudit>(options =>
            {
                options.UseSqlServer(TnUtil.DecryptConnection.SetConnectionString(configuration["ConnectionStrings:Default"]));
                options.EnableSensitiveDataLogging(true);
                options.UseLoggerFactory(TnBaseStartup.LoggerFactory);
            })
                    .AddDbContext<Tenant.API.Base.Context.TnValidation>(options =>
                    {
                        options.UseSqlServer(TnUtil.DecryptConnection.SetConnectionString(configuration["ConnectionStrings:Default"]));
                        options.EnableSensitiveDataLogging(true);
                        options.UseLoggerFactory(TnBaseStartup.LoggerFactory);
                    });

            return services;
        }

        #endregion
    }
}
