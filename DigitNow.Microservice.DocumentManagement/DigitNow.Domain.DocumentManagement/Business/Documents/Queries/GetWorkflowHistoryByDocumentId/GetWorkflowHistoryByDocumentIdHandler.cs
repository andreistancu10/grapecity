using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowHistoryByDocumentId
{
    public class GetWorkflowHistoryByDocumentIdHandler : IQueryHandler<GetWorkflowHistoryByDocumentIdQuery, List<GetWorkflowHistoryByDocumentIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly DocumentManagementDbContext _dbContext;

        public GetWorkflowHistoryByDocumentIdHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<List<GetWorkflowHistoryByDocumentIdResponse>> Handle(GetWorkflowHistoryByDocumentIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _dbContext.Documents.FirstAsync(x => x.Id == request.DocumentId);
            VirtualDocument virtualDocument = null;

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    virtualDocument = await _dbContext.IncomingDocuments.Include(x => x.WorkflowHistory).FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId);
                    break;
                case DocumentType.Internal:
                    virtualDocument =  await _dbContext.InternalDocuments.Include(x => x.WorkflowHistory).FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId);
                    break;
                case DocumentType.Outgoing:
                    virtualDocument = await _dbContext.OutgoingDocuments.Include(x => x.WorkflowHistory).FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId);
                    break;
                default:
                    return null;
            }

            if (virtualDocument != null)
            {
                return _mapper.Map<List<GetWorkflowHistoryByDocumentIdResponse>>(virtualDocument.WorkflowHistory);
            }

            return new List<GetWorkflowHistoryByDocumentIdResponse>();
        }
    }
}
