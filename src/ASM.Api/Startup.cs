using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASM.Api.Extensions;
using ASM.Api.Filters;
using ASM.Api.HealthCheck;
using ASM.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ApiError = ASM.Api.Middleware.ApiError;

namespace ASM.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // service dependencies         
            services.ConfigureServices(Configuration);

            services.ConfigureRedisCache(Configuration);

            services.ConfigureApiVersioning();

            services.ConfigureCors();

            services.ConfigureSwagger();

            services.AddControllers(options =>
                {
                    //options.ReturnHttpNotAcceptable = true;

                    //Filter to track Action Performance for Entire application's actions
                    //options.Filters.Add(typeof(TrackActionPerformanceFilter));
                    options.Filters.Add<ValidationFilter>();
                })
                .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
                //.AddXmlDataContractSerializerFormatters()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Global Exception Handler Middleware
            app.UseApiExceptionHandler(options =>
            {
                options.AddResponseDetails = UpdateApiErrorResponse;
                options.DetermineLogLevel = DetermineLogLevel;
            });

            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "v1.0");
                //options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2"); // Future Version
                options.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultHealthChecks();
                endpoints.MapControllers();
            });
        }

        private static LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
                ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
            {
                return LogLevel.Critical;
            }

            return LogLevel.Error;
        }

        private void UpdateApiErrorResponse(HttpContext context, Exception ex, Models.Response<ApiError> apiError)
        {
            if (ex.GetType().Name == nameof(SqlException))
            {
                apiError.Message = "Exception was a database exception!";
            }
        }
    }
}
