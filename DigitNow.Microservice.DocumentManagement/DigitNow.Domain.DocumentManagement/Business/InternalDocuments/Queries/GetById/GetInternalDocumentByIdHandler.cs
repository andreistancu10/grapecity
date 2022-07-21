using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById
{
    public class GetInternalDocumentByIdHandler : IQueryHandler<GetInternalDocumentByIdQuery, GetInternalDocumentByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetInternalDocumentByIdHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        public async Task<GetInternalDocumentByIdResponse> Handle(GetInternalDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var foundInternalDocument = await _dbContext.InternalDocuments
                .Include(x => x.WorkflowHistory)
                .Include(x => x.Document)
                .ThenInclude(x => x.DocumentUploadedFiles)
                .FirstOrDefaultAsync(c => c.DocumentId == request.Id, cancellationToken);
            
            if (foundInternalDocument == null) return null;

            return _mapper.Map<GetInternalDocumentByIdResponse>(foundInternalDocument);
        }
    }
}