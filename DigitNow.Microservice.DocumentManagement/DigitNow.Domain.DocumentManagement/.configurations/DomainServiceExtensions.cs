﻿using System.Reflection;
using DigitNow.Domain.DocumentManagement.Business._Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.configurations.HostedServices;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using Domain.Localization.Client.configurations;
using HTSS.Platform.Core.Files.MicrosoftExtensions;
using HTSS.Platform.Infrastructure.Api.Tools;
using HTSS.Platform.Infrastructure.BusinessValidators.MassTransit.MicrosoftExtensions;
using HTSS.Platform.Infrastructure.MultiTenant;
using HTSS.Platform.Infrastructure.MultiTenant.MicrosoftExtensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftIn.Domain.Authentication.Client;

namespace DigitNow.Domain.DocumentManagement.configurations
{
    public static class DomainServiceExtensions
    {
        private static readonly Assembly _currentAssembly = typeof(DomainServiceExtensions).Assembly;

        public static IServiceCollection AddTenantNotificationDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            // apply multi-tenant default services at the domain level
            if (configuration.GetValue<bool>(MultiTenantOptions.EnableMultiTenant)) services.AddMultiTenant();

            services.AddDbContext(configuration);
            services.AddElasticSearchClient(configuration);

            // apply hosted services
            services.AddHostedService<MigrateDatabaseHostedService>();

            services.AddMediatR(_currentAssembly);
            services.AddAutoMapper(_currentAssembly);
            services.AddBusinessValidators(_currentAssembly);

            services.AddFilesServices(options =>
            {
                options.DefaultSeparator = ",";
                options.LicenseKey = configuration.GetSection("SyncfusionLicenseKey").Value;
            });

            services.AddPipelines();
            services.AddServices();

            services.AddScoped<RouteParameterAccessor>();
            services.AddAuthenticationClientDomainServices(configuration);
            services.AddLocalizationMQServices();

            return services;
        }

        public static IApplicationBuilder UseTenantNotificationDomain(this IApplicationBuilder app)
        {
            return app;
        }

        public static IServiceCollection AddPipelines(this IServiceCollection services)
        {
            return services;
        }
        
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IDocumentsQueryService, DocumentsQueryService>();

            return services;
        }
    }
}