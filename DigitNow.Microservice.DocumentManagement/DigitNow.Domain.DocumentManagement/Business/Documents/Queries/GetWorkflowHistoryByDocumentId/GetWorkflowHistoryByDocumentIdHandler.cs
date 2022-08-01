using AutoMapper;
using System;
using System.Linq;
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
            var workflowHistoryLogs = await _dbContext.WorkflowHistoryLogs
                .Where(x => x.DocumentId == request.DocumentId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GetWorkflowHistoryByDocumentIdResponse>>(workflowHistoryLogs);
        }
    }
}
