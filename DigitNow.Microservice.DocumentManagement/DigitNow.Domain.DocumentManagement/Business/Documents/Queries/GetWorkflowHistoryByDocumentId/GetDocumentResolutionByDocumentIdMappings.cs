using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowHistoryByDocumentId
{
    public class GetDocumentResolutionByDocumentIdMappings : Profile
    {
        public GetDocumentResolutionByDocumentIdMappings()
        {
            CreateMap<WorkflowHistory, GetWorkflowHistoryByDocumentIdResponse>();
        }
    }
}
