using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById
{
    public class GetIncomingDocumentByIdHandler : IQueryHandler<GetIncomingDocumentByIdQuery, GetIncomingDocumentByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public GetIncomingDocumentByIdHandler(
            IMapper mapper, 
            IDocumentService documentService)
        {
            _mapper = mapper;
            _documentService = documentService;
        }

        public async Task<GetIncomingDocumentByIdResponse> Handle(GetIncomingDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var getByIdQuery = await _documentService.GetByIdQueryAsync(request.Id, cancellationToken, applyPermissions: true);
                
            var foundDocument = await getByIdQuery
                .Include(x => x.WorkflowHistories)
                .Include(x => x.IncomingDocument)
                .Include(x => x.IncomingDocument.ContactDetail)
                .Include(x => x.IncomingDocument.DeliveryDetails)
                .Include(x => x.IncomingDocument.ConnectedDocuments)
                .FirstOrDefaultAsync(cancellationToken);

            if (foundDocument == null)
            {
                var existsIdQuery = await _documentService.GetByIdQueryAsync(request.Id, cancellationToken, applyPermissions: false);
                var itExists = await existsIdQuery.CountAsync(cancellationToken) == 1;
                if (itExists)
                {
                    throw new AccessViolationException("Access rights are not met for this resource!");
                }

                return null;
            }

            return _mapper.Map<GetIncomingDocumentByIdResponse>(foundDocument.IncomingDocument);
        }
    }
}