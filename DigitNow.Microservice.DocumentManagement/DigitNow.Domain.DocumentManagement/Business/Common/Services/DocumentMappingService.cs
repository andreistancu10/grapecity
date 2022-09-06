using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDocumentMappingService
    {
        Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(int languageId, IList<VirtualDocument> virtualDocuments, CancellationToken token);
        Task<List<ReportViewModel>> MapToReportViewModelAsync(int languageId, IList<VirtualDocument> virtualDocuments, CancellationToken token);
        Task<List<HistoricalArchiveDocumentsViewModel>> MapToHistoricalArchiveDocumentsViewModelAsync(IList<DynamicFormFillingLog> dynamicFormValues, CancellationToken token);
    }

    public class DocumentMappingService : IDocumentMappingService
    {
        #region [ Fields ]

        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        private UserModel _currentUser;
        private readonly DocumentRelationsFetcher _documentRelationsFetcher;
        private readonly DocumentReportRelationsFetcher _documentReportRelationsFetcher;
        private readonly DynamicFormRelationsFetcher _dynamicFormRelationsFetcher;

        #endregion

        #region [ Construction ]

        public DocumentMappingService(
            IServiceProvider serviceProvider,
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;

            _documentRelationsFetcher = new DocumentRelationsFetcher(serviceProvider);
            _documentReportRelationsFetcher = new DocumentReportRelationsFetcher(serviceProvider);
            _dynamicFormRelationsFetcher = new DynamicFormRelationsFetcher(serviceProvider);
        }

        #endregion

        #region [ IDocumentMappingService ]

        public async Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(int languageId, IList<VirtualDocument> virtualDocuments, CancellationToken token)
        {
            if (!virtualDocuments.Any())
            {
                return new List<DocumentViewModel>();
            }

            _currentUser = await _identityService.GetCurrentUserAsync(token);

            await _documentRelationsFetcher
                .UseDocumentsContext(new DocumentsFetcherContext { Documents = virtualDocuments })
                .UseTranslationsContext(new LanguageFetcherContext { LanguageId = languageId })
                .TriggerFetchersAsync(token);

            return MapDocuments(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
                .ToList();
        }

        public async Task<List<ReportViewModel>> MapToReportViewModelAsync(int languageId, IList<VirtualDocument> virtualDocuments, CancellationToken token)
        {
            if (!virtualDocuments.Any()) return new List<ReportViewModel>();

            await _documentReportRelationsFetcher
                .UseDocumentsContext(new DocumentsFetcherContext { Documents = virtualDocuments })
                .UseTranslationsContext(new LanguageFetcherContext { LanguageId = languageId })
                .TriggerFetchersAsync(token);

            return MapDocumentsReports(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
                .ToList();
        }

        public async Task<List<HistoricalArchiveDocumentsViewModel>> MapToHistoricalArchiveDocumentsViewModelAsync(IList<DynamicFormFillingLog> dynamicFormValues, CancellationToken token)
        {
            await _dynamicFormRelationsFetcher
                .UseDynamicFormsContext(new DynamicFormsFetcherContext { DynamicFormValues = dynamicFormValues })
                .TriggerFetchersAsync(token);

            return MapHistoricalArchiveDocuments(dynamicFormValues)
                .ToList();
        }

        #endregion

        #region [ Documents - Utils ]

        private List<DocumentViewModel> MapDocuments(IList<VirtualDocument> documents)
        {
            var result = new List<DocumentViewModel>();

            var incomingDocuments = documents.Where(x => x is IncomingDocument).Cast<IncomingDocument>();
            if (incomingDocuments.Any())
            {
                result.AddRange(MapChildDocuments(incomingDocuments));
            }

            var internalDocuments = documents.Where(x => x is InternalDocument).Cast<InternalDocument>();
            if (internalDocuments.Any())
            {
                result.AddRange(MapChildDocuments(internalDocuments));
            }

            var outgoingDocuments = documents.Where(x => x is OutgoingDocument).Cast<OutgoingDocument>();
            if (outgoingDocuments.Any())
            {
                result.AddRange(MapChildDocuments(outgoingDocuments));
            }

            return result;
        }

        private List<DocumentViewModel> MapChildDocuments<T>(IEnumerable<T> childDocuments)
            where T : VirtualDocument
        {
            var result = new List<DocumentViewModel>();

            foreach (var childDocument in childDocuments)
            {
                var aggregate = new VirtualDocumentAggregate<T>
                {
                    VirtualDocument = childDocument,
                    CurrentUser = _currentUser,
                    Users = _documentRelationsFetcher.DocumentUsers,
                    Departments = _documentRelationsFetcher.DocumentDepartments,
                    Categories = _documentRelationsFetcher.DocumentCategories,
                    InternalCategories = _documentRelationsFetcher.DocumentInternalCategories,
                    DocumentStatusTranslations = _documentRelationsFetcher.DocumentStatusTranslations,
                    DocumentTypeTranslations = _documentRelationsFetcher.DocumentTypesTranslations
                };

                result.Add(_mapper.Map<VirtualDocumentAggregate<T>, DocumentViewModel>(aggregate));
            }

            return result;
        }

        #endregion

        #region [ Documents Reports - Utils ]

        private List<ReportViewModel> MapDocumentsReports(IList<VirtualDocument> documents)
        {
            var result = new List<ReportViewModel>();

            var incomingDocuments = documents.Where(x => x is IncomingDocument).Cast<IncomingDocument>();
            if (incomingDocuments.Any())
            {
                result.AddRange(MapChildReportDocuments(incomingDocuments));
            }

            var internalDocuments = documents.Where(x => x is InternalDocument).Cast<InternalDocument>();
            if (internalDocuments.Any())
            {
                result.AddRange(MapChildReportDocuments(internalDocuments));
            }

            var outgoingDocuments = documents.Where(x => x is OutgoingDocument).Cast<OutgoingDocument>();
            if (outgoingDocuments.Any())
            {
                result.AddRange(MapChildReportDocuments(outgoingDocuments));
            }

            return result;
        }

        private List<ReportViewModel> MapChildReportDocuments<T>(IEnumerable<T> childDocuments)
            where T : VirtualDocument
        {
            var result = new List<ReportViewModel>();

            foreach (var childDocument in childDocuments)
            {
                var aggregate = new VirtualReportAggregate<T>
                {
                    VirtualDocument = childDocument,
                    Users = _documentReportRelationsFetcher.DocumentUsers,
                    Categories = _documentReportRelationsFetcher.DocumentCategories,
                    InternalCategories = _documentReportRelationsFetcher.DocumentInternalCategories,
                    Departments = _documentReportRelationsFetcher.DocumentDepartments,
                    DocumentStatusTranslations = _documentReportRelationsFetcher.DocumentStatusTranslations,
                    DocumentTypeTranslations = _documentReportRelationsFetcher.DocumentTypesTranslationModels
                };

                result.Add(_mapper.Map<VirtualReportAggregate<T>, ReportViewModel>(aggregate));
            }

            return result;
        }
        #endregion

        #region [ Historical Archive Documents - Utils ]

        private List<HistoricalArchiveDocumentsViewModel> MapHistoricalArchiveDocuments(IList<DynamicFormFillingLog> dynamicFormValues)
        {
            var result = new List<HistoricalArchiveDocumentsViewModel>();

            foreach (var dynamicFormValue in dynamicFormValues)
            {
                var aggregate = new DynamicFormAggregate
                {
                    FormValues = dynamicFormValue,
                    Categories = _dynamicFormRelationsFetcher.Categories,
                    Users = _dynamicFormRelationsFetcher.DynamicFormUsers,
                    Departments = _dynamicFormRelationsFetcher.Departments
                };

                result.Add(_mapper.Map<DynamicFormAggregate, HistoricalArchiveDocumentsViewModel>(aggregate));
            }

            return result;
        }

        #endregion
    }
}