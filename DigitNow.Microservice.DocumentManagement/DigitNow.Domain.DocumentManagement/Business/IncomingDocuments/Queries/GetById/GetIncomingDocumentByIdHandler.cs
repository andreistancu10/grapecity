using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById
{
    public class GetIncomingDocumentByIdHandler : IQueryHandler<GetIncomingDocumentByIdQuery, GetIncomingDocumentByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetIncomingDocumentByIdHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        public async Task<GetIncomingDocumentByIdResponse> Handle(GetIncomingDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var foundIncomingDocument = await _dbContext.IncomingDocuments.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (foundIncomingDocument == null) return null;

            return _mapper.Map<GetIncomingDocumentByIdResponse>(foundIncomingDocument);
        }
    }
}