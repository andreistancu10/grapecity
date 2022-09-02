using System.Reflection;
using DigitNow.Domain.Catalog.Client.configurations;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Generators;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.configurations.HostedServices;
using DigitNow.Domain.DocumentManagement.Data.Repositories;
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
using DigitNow.Domain.DocumentManagement.Business.Common.Notifications.Mail;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using Domain.Mail.Client;

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
            services.AddCatalogClientDomainServices(configuration);
            services.AddMailClientDomainServices();

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
            // Export
            services.AddScoped<IPdfGenerator, PdfGenerator>();
            services.AddScoped<IPdfDocumentGenerator, PdfDocumentGenerator>();

            // Mail 
            services.AddScoped<IMailSender, MailSender>();
            services.AddScoped<IMailSenderService, MailSenderService>();

            // Business Services
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IIncomingDocumentService, IncomingDocumentService>();
            services.AddTransient<IInternalDocumentService, InternalDocumentService>();
            services.AddTransient<IOutgoingDocumentService, OutgoingDocumentService>();
            services.AddTransient<IVirtualDocumentService, VirtualDocumentService>();
            services.AddTransient<IDocumentResolutionService, DocumentResolutionService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IDocumentMappingService, DocumentMappingService>();
            services.AddTransient<ISpecialRegisterMappingService, SpecialRegisterMappingService>();
            services.AddTransient<ISpecialRegisterService, SpecialRegisterService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IActionService, ActionService>();
            services.AddTransient<IActionFunctionaryService, ActionFunctionaryService>();

            services.AddTransient<IUploadedFileService, UploadedFileService>();
            services.AddTransient<IDynamicFormsServices, DynamicFormsServices>();
            
            services.AddTransient<IDocumentFileService, DocumentFileService>();

            services.AddTransient<IObjectiveService, ObjectiveService>();
            services.AddTransient<IGeneralObjectiveService, GeneralObjectiveService>();
            services.AddTransient<ISpecificObjectiveService, SpecificObjectiveService>();
            services.AddTransient<ISpecificObjectiveFunctionaryService, SpecificObjectiveFunctionaryService>();
            services.AddTransient<IObjectiveDashboardService, ObjectiveDashboardService>();
            services.AddTransient<IObjectiveMappingService, ObjectiveMappingService>();

            return services;
        }
    }
}