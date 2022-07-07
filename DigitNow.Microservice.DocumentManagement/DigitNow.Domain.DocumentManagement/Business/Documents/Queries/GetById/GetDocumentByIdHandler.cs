using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetById
{
    public class GetDocumentByIdHandler
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowManagementService _workflowService;

        public GetDocumentByIdHandler(IMapper mapper, IWorkflowManagementService workflowService)
        {
            _mapper = mapper;
            _workflowService = workflowService;
        }
        public async Task<GetDocumentByIdResponse> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _workflowService.GetDocumentById(request.DocumentId, cancellationToken);

            return _mapper.Map<GetDocumentByIdResponse>(document);
        }
    }
}
