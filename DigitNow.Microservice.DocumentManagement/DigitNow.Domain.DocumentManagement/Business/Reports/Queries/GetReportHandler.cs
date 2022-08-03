using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.utils;
using Domain.Localization.Client;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries
{
    public class GetReportHandler : IQueryHandler<GetReportQuery, List<ExportReportViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationManager _localizationManager;

        public GetReportHandler(
            IServiceProvider serviceProvider,
            IMapper mapper,
            ILocalizationManager localizationManager)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _localizationManager = localizationManager;
        }

        public async Task<List<ExportReportViewModel>> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            var reportProcessor = new ReportRelatedProcessor(_serviceProvider);

            var reportViewModels = await reportProcessor.ProcessDocumentsAsync(request.FromDate, request.ToDate, cancellationToken);

            return await MapToExportReportViewModels(reportViewModels, request.LanguageId, cancellationToken);
        }

        private async Task<List<ExportReportViewModel>> MapToExportReportViewModels(IEnumerable<ReportViewModel> reportViewModels, int languageId, CancellationToken cancellationToken)
        {
            var documentTypeDictionary = await GetDocumentTypeTranslationsAsync(languageId,cancellationToken);
            var documentStatus = await GetDocumentStatusTranslationsAsync(languageId,cancellationToken);

            return reportViewModels
                .Select(reportViewModel => new ReportViewModelAggregate
                {
                    ReportViewModel = reportViewModel,
                    DocumentTypeTranslations = documentTypeDictionary,
                    StatusTranslations = documentStatus
                })
                .Select(aggregate => _mapper.Map<ExportReportViewModel>(aggregate))
                .ToList();
        }

        private async Task<Dictionary<DocumentStatus, string>> GetDocumentStatusTranslationsAsync(int languageId, CancellationToken cancellationToken)
        {
            var documentStatusTranslationsResponse = await _localizationManager.Translate(
                languageId,
                CustomMappings.DocumentStatusTranslations.Select(c => c.Value),
                cancellationToken);

            var documentStatus = new Dictionary<DocumentStatus, string>();

            foreach (var (key, value) in documentStatusTranslationsResponse.Translations)
            {
                var status = CustomMappings.DocumentStatusTranslations.FirstOrDefault(c => c.Value == key).Key;
                documentStatus.Add(status, value);
            }

            return documentStatus;
        }

        private async Task<Dictionary<DocumentType, string>> GetDocumentTypeTranslationsAsync(int languageId, CancellationToken cancellationToken)
        {
            var documentTypeTranslationsResponse = await _localizationManager.Translate(
                languageId,
                CustomMappings.DocumentTypeTranslations.Select(c => c.Value),
                cancellationToken);

            var documentTypeDictionary = new Dictionary<DocumentType, string>();

            foreach (var (key, value) in documentTypeTranslationsResponse.Translations)
            {
                var documentType = CustomMappings.DocumentTypeTranslations.FirstOrDefault(c => c.Value == key).Key;
                documentTypeDictionary.Add(documentType, value);
            }

            return documentTypeDictionary;
        }
    }
}