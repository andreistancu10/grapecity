using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.SetDocumentsResolution
{
    public class SetDocumentsResolutionHandler
        : ICommandHandler<SetDocumentsResolutionCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;

        public SetDocumentsResolutionHandler(
            DocumentManagementDbContext dbContext,
            IDocumentService documentService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
        }
        public async Task<ResultObject> Handle(SetDocumentsResolutionCommand request, CancellationToken cancellationToken)
        {
            var documentBatchIds = request.Batch.Documents.Select(x => x.Id);

            var foundDocuments =  await _dbContext.Documents
                .Where(x => documentBatchIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var results = new List<bool>
            {
                await ProcessDocumentsAsync(foundDocuments, request.Resolution, request.Remarks, cancellationToken)
            };

            if (results.Any(x => !x))
            {
                throw new InvalidOperationException("Cannot set document resolution!");
            }

            return ResultObject.Ok();
        }

        private async Task<bool> ProcessDocumentsAsync(IEnumerable<long> documents, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            if (documents.Any())
            {
                await _documentService.SetResolutionAsync(documents, resolutionType, remarks, cancellationToken);
            }

            return true;
        }
    }
}