using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById
{
    public class GetInternalDocumentByIdHandler : IQueryHandler<GetInternalDocumentByIdQuery, GetInternalDocumentByIdResponse>
    {
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;

        public GetInternalDocumentByIdHandler(
            IMapper mapper, 
            IDocumentService documentService)
        {
            _mapper = mapper;
            _documentService = documentService;
        }
        
        public async Task<GetInternalDocumentByIdResponse> Handle(GetInternalDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var getByIdQuery = await _documentService.GetByIdQueryAsync(request.Id, cancellationToken, applyPermissions: true);

            var foundDocument = await getByIdQuery
                .Include(x => x.WorkflowHistories)
                .Include(x => x.InternalDocument)
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

            return _mapper.Map<GetInternalDocumentByIdResponse>(foundDocument.InternalDocument);
        }
    }
}