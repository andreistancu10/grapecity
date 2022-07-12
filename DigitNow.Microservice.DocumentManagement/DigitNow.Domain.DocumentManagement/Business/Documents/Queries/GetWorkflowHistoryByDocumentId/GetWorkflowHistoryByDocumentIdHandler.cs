using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowHistoryByDocumentId
{
    public class GetWorkflowHistoryByDocumentIdHandler : IQueryHandler<GetWorkflowHistoryByDocumentIdQuery, List<GetWorkflowHistoryByDocumentIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowManagementService _workflowManagementService;

        public GetWorkflowHistoryByDocumentIdHandler(IMapper mapper, IWorkflowManagementService workflowManagementService)
        {
            _mapper = mapper;
            _workflowManagementService = workflowManagementService;
        }

        public async Task<List<GetWorkflowHistoryByDocumentIdResponse>> Handle(GetWorkflowHistoryByDocumentIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _workflowManagementService.GetDocumentById(request.DocumentId, cancellationToken);

            if (document.IncomingDocument != null)
            {
                return _mapper.Map<List<GetWorkflowHistoryByDocumentIdResponse>>(document.IncomingDocument.WorkflowHistory);
            }

            if (document.OutgoingDocument != null)
            {
                return _mapper.Map<List<GetWorkflowHistoryByDocumentIdResponse>>(document.OutgoingDocument.WorkflowHistory);
            }

            return new List<GetWorkflowHistoryByDocumentIdResponse>();
        }
    }
}
