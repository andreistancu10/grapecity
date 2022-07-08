using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowInformation
{
    public class GetWorkflowInformationByDocumentIdHandler : IQueryHandler<GetWorkflowInformationByDocumentIdQuery, GetWorkflowInformationByDocumentIdResponse>
    {
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;

        public GetWorkflowInformationByDocumentIdHandler(IDocumentService documentService, IIdentityService identityService)
        {
            _documentService = documentService;
            _identityService = identityService;
        }

        public async Task<GetWorkflowInformationByDocumentIdResponse> Handle(GetWorkflowInformationByDocumentIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _documentService.FindAsync(x => x.Id == request.DocumentId, cancellationToken);
            var userRole = await _identityService.GetCurrentUserRoleAsync(cancellationToken);

            var response = new GetWorkflowInformationByDocumentIdResponse { DocumentStatus = document.Status, UserRole = userRole };
            
            return response;
        }
    }
}
