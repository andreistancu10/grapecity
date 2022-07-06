using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetById
{
    public class GetOutgoingDocumentByIdHandler : IQueryHandler<GetOutgoingDocumentByIdQuery, GetOutgoingDocumentByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetOutgoingDocumentByIdHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        public async Task<GetOutgoingDocumentByIdResponse> Handle(GetOutgoingDocumentByIdQuery query, CancellationToken cancellationToken)
        {
            var foundOutgoingDocument = await _dbContext.OutgoingDocuments.FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);
            if (foundOutgoingDocument == null) return null;

            return _mapper.Map<GetOutgoingDocumentByIdResponse>(foundOutgoingDocument);
        }
    }
}