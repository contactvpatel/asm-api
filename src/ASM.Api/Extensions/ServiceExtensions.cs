using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ASM.Api.Extensions.Swagger;
using ASM.Business.Interfaces;
using ASM.Business.Services;
using ASM.Core.Models;
using ASM.Core.Repositories;
using ASM.Core.Repositories.Base;
using ASM.Util.Logging;
using ASM.Infrastructure.Data;
using ASM.Infrastructure.Repositories;
using ASM.Infrastructure.Repositories.Base;
using ASM.Infrastructure.Services;
using ASM.Util.Models;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ASM.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //// Using Scrutor to map the dependencies with scoped lifetime (https://github.com/khellang/Scrutor)
            //services.Scan(scan => scan
            //    .FromCallingAssembly()
            //    .FromApplicationDependencies(c => c.FullName != null && c.FullName.StartsWith("ASM"))
            //    .AddClasses()
            //    .AsMatchingInterface().WithScopedLifetime());

            // Add Database
            ConfigureDatabases(services, configuration);

            // Add Infrastructure Layer
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IModuleTypeRepository, ModuleTypeRepository>();
            services.AddScoped<IAccessGroupRepository, AccessGroupRepository>();
            services.AddScoped<IAccessGroupAssignmentRepository, AccessGroupAssignmentRepository>();
            services.AddScoped<IApplicationSecurityRepository, ApplicationSecurityRepository>();
            services.AddScoped<Core.Services.IMisService, Infrastructure.Services.MisService>();
            services.AddScoped<Core.Services.ISsoService, Infrastructure.Services.SsoService>();

            // Add Business Layer
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IModuleTypeService, ModuleTypeService>();
            services.AddScoped<IAccessGroupService, AccessGroupService>();
            services.AddScoped<IAccessGroupAssignmentService, AccessGroupAssignmentService>();
            services.AddScoped<IApplicationSecurityService, ApplicationSecurityService>();
            services.AddScoped<IMisService, Business.Services.MisService>();
            services.AddScoped<ISsoService, Business.Services.SsoService>();

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // LoggingHelpers
            services.AddTransient<LoggingDelegatingHandler>();

            //External Service Dependency (Example: MISService)
            services.AddTransient<IRestClient>(_ => new RestClient(configuration.GetSection("MisService:Url").Value));
            services.Configure<MisApiModel>(configuration.GetSection("MisService"));
            services.Configure<SsoApiModel>(configuration.GetSection("SsoService"));

            // Add Email Sender
            services.AddScoped<EmailSender>();

            // HealthChecks
            services.AddHealthChecks().AddDbContextCheck<ASMContext>();

            services.AddDataProtection();
        }

        private static void ConfigureDatabases(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ASMContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("ASMDbConnection"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                // Supporting multiple versioning scheme
                // Route (api/v1/accounts)
                // Header (X-version=1)
                // Querystring (api/accounts?api-version=1)
                // Media Type (application/json;v=1)
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-version"), new QueryStringApiVersionReader("api-version"),
                    new MediaTypeApiVersionReader("v"));
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(ConfigureSwaggerGen);
        }

        private static void ConfigureSwaggerGen(SwaggerGenOptions options)
        {
            AddSwaggerDocs(options);

            options.OperationFilter<RemoveVersionFromParameter>();
            options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            options.DocInclusionPredicate((version, desc) =>
            {
                if (!desc.TryGetMethodInfo(out var methodInfo))
                    return false;

                var versions = methodInfo
                    .DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                var maps = methodInfo
                    .GetCustomAttributes(true)
                    .OfType<MapToApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToList();

                return versions?.Any(v => $"v{v}" == version) == true
                       && (!maps.Any() || maps.Any(v => $"v{v}" == version));
            });
        }

        private static void AddSwaggerDocs(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1.0", new OpenApiInfo
            {
                Version = "v1.0",
                Title = "Application Security Module (ASM) API"
            });

            // Future Version
            //options.SwaggerDoc("v2", new OpenApiInfo
            //{
            //    Version = "v2.0",
            //    Title = "Application Security Module (ASM) API"
            //});
        }

        public static void ConfigureRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();

            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);

            services.AddSingleton(redisCacheSettings);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisCacheSettings.ConnectionString;
                options.InstanceName = redisCacheSettings.InstanceName;
            });

            services.AddSingleton<IRedisCacheService, Business.Services.RedisCacheService>();
            services.AddSingleton<Core.Services.IRedisCacheService, Infrastructure.Services.RedisCacheService>();
        }
    }
}