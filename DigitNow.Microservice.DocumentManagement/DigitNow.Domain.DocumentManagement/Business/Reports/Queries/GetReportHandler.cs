﻿using AutoMapper;
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

        public GetReportHandler(IMapper mapper,
            IServiceProvider serviceProvider,
            ILocalizationManager localizationManager)
        {
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            _localizationManager = localizationManager;
        }

        public async Task<List<ExportReportViewModel>> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            IReportProcessor report = new ReportRelatedProcessor(_serviceProvider);
            var reportViewModels = await report.GetDataAsync(request.FromDate, request.ToDate, cancellationToken);

            return await MapToExportReportViewModels(reportViewModels, cancellationToken);
        }

        private async Task<List<ExportReportViewModel>> MapToExportReportViewModels(IEnumerable<ReportViewModel> reportViewModels, CancellationToken cancellationToken)
        {
            var documentTypeDictionary = await GetDocumentTypeTranslationsAsync(cancellationToken);
            var documentStatus = await GetDocumentStatusTranslationsAsync(cancellationToken);

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

        private async Task<Dictionary<DocumentStatus, string>> GetDocumentStatusTranslationsAsync(CancellationToken cancellationToken)
        {
            var documentStatusTranslationsResponse = await _localizationManager.Translate(
                1,
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

        private async Task<Dictionary<DocumentType, string>> GetDocumentTypeTranslationsAsync(CancellationToken cancellationToken)
        {
            var documentTypeTranslationsResponse = await _localizationManager.Translate(
                1,
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