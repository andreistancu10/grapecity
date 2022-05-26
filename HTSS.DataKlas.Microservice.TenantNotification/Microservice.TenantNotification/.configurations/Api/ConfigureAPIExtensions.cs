using System.Collections.Generic;
using System.Reflection;
using FluentValidation.AspNetCore;
using HTSS.Platform.Core.FluentValidation.Extensions;
using HTSS.Platform.Infrastructure.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ShiftIn.Domain.TenantNotification;

namespace Microservice.TenantNotification
{
    public static class ConfigureAPIExtensions
    {
        private static readonly Assembly CommonAssembly = typeof(DomainServiceExtensions).Assembly;

        public static IServiceCollection AddControllerConfiguration(this IServiceCollection services)
        {
            List<Assembly> assemblies = new List<Assembly>
            {
                // add assemlies to be scanned 
                CommonAssembly
            };

            services.AddTransient<IValidatorInterceptor, ValidationResultInterceptor>();

            services.Configure<ApiBehaviorOptions>(cfg => cfg.SuppressModelStateInvalidFilter = true);

            IMvcBuilder mvcBuilder = services.AddControllers(config => config.Filters.Add<ModelValidationFilter>());
            mvcBuilder.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblies(assemblies));

            return services;
        }
    }
}