using DigitNow.Domain.DocumentManagement.configurations;
using DigitNow.Domain.DocumentManagement.configurations.Adapters;
using DigitNow.Microservice.DocumentManagement.configurations.Api;
using DigitNow.Microservice.DocumentManagement.configurations.Auth;
using DigitNow.Microservice.DocumentManagement.configurations.Consul;
using DigitNow.Microservice.DocumentManagement.configurations.DistributedCache;
using DigitNow.Microservice.DocumentManagement.configurations.MassTransit;
using DigitNow.Microservice.DocumentManagement.configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DigitNow.Microservice.DocumentManagement
{
    public class Startup
    {
        public const string CorsPolicy = "HTSS.DataKlas.CORS";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAdapters(Configuration);
            services.AddAuthentication(Configuration);
            services.AddConsulConfigurations(Configuration);
            services.AddSwaggerConfigurations(Configuration);
            services.AddMassTransitConfigurations(Configuration);
            services.AddDistributedCacheConfigurations(Configuration);

            services.AddCors(options =>
                options.AddPolicy(CorsPolicy, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTenantNotificationDomainServices(Configuration);
            services.AddControllerConfiguration();

            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseConsulConfigurations(env);
            app.UseSwaggerConfigurations(env);
            app.UseSerilogRequestLogging();

            app.UseTenantNotificationDomain();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(CorsPolicy);
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}