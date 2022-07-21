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
            var document = await _documentService.FindAsync(x => x.Id == request.DocumentId, cancellationToken);
            var userRole = await _identityService.GetCurrentUserFirstRoleAsync(cancellationToken);

            DateTime? opinionRequestedUntil = null;
            if (document.Status == DocumentStatus.OpinionRequestedAllocated)
            {
                opinionRequestedUntil = await ExtractDeadline(document);
            }

            var response = new GetWorkflowInformationByDocumentIdResponse { DocumentStatus = document.Status, UserRole = userRole.Id, OpinionRequestedUntil = opinionRequestedUntil };

            return response;
        }

        private async Task<DateTime?> ExtractDeadline(Document document)
        {
            DateTime? opinionRequestedUntil = null;

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    opinionRequestedUntil = await ExtractDeadlineForSendingOpinion<IncomingDocument>(document);
                    break;
                case DocumentType.Internal:
                    opinionRequestedUntil = await ExtractDeadlineForSendingOpinion<InternalDocument>(document);
                    break;
                case DocumentType.Outgoing:
                    opinionRequestedUntil = await ExtractDeadlineForSendingOpinion<OutgoingDocument>(document);
                    break;
                default:
                    break;
            }

            return opinionRequestedUntil;
        }

        private async Task<DateTime?> ExtractDeadlineForSendingOpinion<T>(Document document) where T : VirtualDocument
        {
            var virtualDocument = await _dbContext.Set<T>().Include(x => x.WorkflowHistory).AsQueryable()
                .FirstOrDefaultAsync(x => x.DocumentId == document.Id);

            var workflowEntry =  virtualDocument.WorkflowHistory.Where(x => x.OpinionRequestedUntil != null)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            return workflowEntry?.OpinionRequestedUntil;
        }
    }
}
