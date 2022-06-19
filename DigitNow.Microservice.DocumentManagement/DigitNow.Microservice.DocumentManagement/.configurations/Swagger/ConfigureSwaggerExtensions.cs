using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DigitNow.Microservice.DocumentManagement.configurations.Swagger;

internal static class ConfigureSwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SwaggerOptions>(configuration.GetSection("Swagger"));

        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(s => s.FullName);
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfigurations(this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        IOptions<SwaggerOptions> options = app.ApplicationServices.GetService<IOptions<SwaggerOptions>>();
        var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();
        ILogger<IApplicationBuilder> logger = loggerFactory.CreateLogger<IApplicationBuilder>();

        app.UseSwagger(SwaggerSetup.SwaggerConfigFactory(options.Value));
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(SwaggerSetup.SwaggerEndpointUrlFactory(options.Value), options.Value.Title);
            c.RoutePrefix = string.Empty;
            c.DocExpansion(DocExpansion.None);
        });

        logger.LogInformation("Swagger was configured.");

        return app;
    }
}