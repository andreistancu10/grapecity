using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        private readonly DocumentRelationsFetcher _documentRelationsFetcher;
        private readonly IMapper _mapper;
        
        #endregion

        #region [ Construction ]

        public DocumentMappingService(
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _mapper = mapper;
            _documentRelationsFetcher = new DocumentRelationsFetcher(serviceProvider);
        }

        #endregion

        #region [ IDocumentMappingService ]

        public async Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<VirtualDocument> virtualDocuments, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = virtualDocuments }, cancellationToken);
            return MapDocuments<DocumentViewModel>(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
                .ToList();
        }

        public async Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<VirtualDocument> virtualDocuments, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = virtualDocuments }, cancellationToken);
            return MapDocuments<ReportViewModel>(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
                .ToList();
        }

        #endregion

        #region [ Helpers ]

        private List<TViewModel> MapDocuments<TViewModel>(IList<VirtualDocument> documents)
            where TViewModel : class, new()
        {
            var result = new List<TViewModel>();

            var incomingDocuments = documents.Where(x => x is IncomingDocument).Cast<IncomingDocument>();
            if (incomingDocuments.Any())
            {
                result.AddRange(MapChildDocuments<IncomingDocument, TViewModel>(incomingDocuments));
            }

            var internalDocuments = documents.Where(x => x is InternalDocument).Cast<InternalDocument>();
            if (internalDocuments.Any())
            {
                result.AddRange(MapChildDocuments<InternalDocument, TViewModel>(internalDocuments));
            }

            var outgoingDocuments = documents.Where(x => x is OutgoingDocument).Cast<OutgoingDocument>();
            if (outgoingDocuments.Any())
            {
                result.AddRange(MapChildDocuments<OutgoingDocument, TViewModel>(outgoingDocuments));
            }

            return result;
        }

        private List<TResult> MapChildDocuments<T, TResult>(IEnumerable<T> childDocuments)
            where T : VirtualDocument
            where TResult : class, new()
        {
            var result = new List<TResult>();
            foreach (var childDocument in childDocuments)
            {
                var aggregate = new VirtualDocumentAggregate<T>
                {
                    VirtualDocument = childDocument,
                    Users = _documentRelationsFetcher.DocumentUsers,
                    Categories = _documentRelationsFetcher.DocumentCategories,
                    InternalCategories = _documentRelationsFetcher.DocumentInternalCategories
                };

                result.Add(_mapper.Map<VirtualDocumentAggregate<T>, TResult>(aggregate));
            }
            return result;
        }

        #endregion
    }
}