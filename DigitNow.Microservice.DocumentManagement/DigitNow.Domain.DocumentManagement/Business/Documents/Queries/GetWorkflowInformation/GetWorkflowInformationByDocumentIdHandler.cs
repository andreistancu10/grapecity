using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowInformation
{
    public class GetWorkflowInformationByDocumentIdHandler : IQueryHandler<GetWorkflowInformationByDocumentIdQuery, GetWorkflowInformationByDocumentIdResponse>
    {
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;
        private readonly DocumentManagementDbContext _dbContext;

        public GetWorkflowInformationByDocumentIdHandler(IDocumentService documentService, IIdentityService identityService, DocumentManagementDbContext dbContext)
        {
            _documentService = documentService;
            _identityService = identityService;
            _dbContext = dbContext;
        }

        public async Task<GetWorkflowInformationByDocumentIdResponse> Handle(GetWorkflowInformationByDocumentIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _dbContext.Documents
                .Include(x => x.WorkflowHistories)
                .FirstAsync(x => x.Id == request.DocumentId, cancellationToken);

            var userRole = await _identityService.GetCurrentUserFirstRoleAsync(cancellationToken);

            DateTime? opinionRequestedUntil = null;
            if (document.Status == DocumentStatus.OpinionRequestedAllocated)
            {
                opinionRequestedUntil = ExtractDeadline(document);
            }

            var response = new GetWorkflowInformationByDocumentIdResponse { DocumentStatus = document.Status, UserRole = userRole.Id, OpinionRequestedUntil = opinionRequestedUntil };

            return response;
        }

        private static DateTime? ExtractDeadline(Document document)
        {
            var workflowEntry = document.WorkflowHistories
                .Where(x => x.OpinionRequestedUntil != null)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            return workflowEntry?.OpinionRequestedUntil;
        }
    }
}
