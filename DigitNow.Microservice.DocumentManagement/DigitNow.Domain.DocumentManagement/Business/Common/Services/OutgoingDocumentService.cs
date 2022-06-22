using DigitNow.Domain.DocumentManagement.Business.Common.Repositories;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IOutgoingDocumentService
    {
        Task<List<OutgoingDocument>> FindAsync(Expression<Func<OutgoingDocument, bool>> predicate, CancellationToken cancellationToken);
    }

    public class OutgoingDocumentService : IOutgoingDocumentService
    {
        private readonly IDocumentService _documentService;
        private readonly IOutgoingDocumentRepository _outgoingDocumentRepository;

        public OutgoingDocumentService(
            IDocumentService documentService,
            IOutgoingDocumentRepository outgoingDocumentRepository)
        {
            _documentService = documentService;
            _outgoingDocumentRepository = outgoingDocumentRepository;
        }

        public Task<List<OutgoingDocument>> FindAsync(Expression<Func<OutgoingDocument, bool>> predicate, CancellationToken cancellationToken) =>
            _outgoingDocumentRepository.FindByAsync(predicate, cancellationToken);
    }
}
