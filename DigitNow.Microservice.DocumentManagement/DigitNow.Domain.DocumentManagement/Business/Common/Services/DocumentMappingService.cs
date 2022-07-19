using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDocumentMappingService
    {
        Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<VirtualDocument> documents, CancellationToken cancellationToken);
        Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<VirtualDocument> documents, CancellationToken cancellationToken);
    }

    public class DocumentMappingService : IDocumentMappingService
    {
        #region [ Fields ]
    
        private readonly IMapper _mapper;
        private readonly DocumentRelationsFetcher _documentRelationsFetcher;
        private readonly DocumentReportRelationsFetcher _documentReportRelationsFetcher;

        #endregion

        #region [ Construction ]

        public DocumentMappingService(
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _mapper = mapper;
            _documentRelationsFetcher = new DocumentRelationsFetcher(serviceProvider);
            _documentReportRelationsFetcher = new DocumentReportRelationsFetcher(serviceProvider);
        }

        #endregion

        #region [ IDocumentMappingService ]

        public async Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<VirtualDocument> virtualDocuments, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = virtualDocuments }, cancellationToken);
            return MapDocuments(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
                .ToList();
        }

        public async Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<VirtualDocument> virtualDocuments, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = virtualDocuments }, cancellationToken);
            return MapDocumentsReports(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
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
                    Users = _documentRelationsFetcher.DocumentUsers,
                    Categories = _documentRelationsFetcher.DocumentCategories,
                    InternalCategories = _documentRelationsFetcher.DocumentInternalCategories
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
                    SpecialRegisterMapping = _documentReportRelationsFetcher.DocumentSpecialRegisterMapping.FirstOrDefault(c => c.DocumentId == childDocument.DocumentId)
                };

                result.Add(_mapper.Map<VirtualReportAggregate<T>, ReportViewModel>(aggregate));
            }

            return result;
        }

        #endregion
    }
}