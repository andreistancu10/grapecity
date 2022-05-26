using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ShiftIn.Domain.TenantNotification;

namespace Microservice.TenantNotification
{
    public class Startup
    {
        public const string CORSPolicy = "HTSS.DataKlas.CORS";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(Configuration);
            services.AddConsulConfigurations(Configuration);
            services.AddSwaggerConfigurations(Configuration);
            services.AddMassTransitConfigurations(Configuration);
            services.AddDistributedCacheConfigurations(Configuration);

            services.AddCors(options =>
                options.AddPolicy(CORSPolicy, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

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
            app.UseCors(CORSPolicy);
            
            // In order to work, a middleware needs to be defined ahead of UseEndpoints
            app.UseMultiTenant();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}